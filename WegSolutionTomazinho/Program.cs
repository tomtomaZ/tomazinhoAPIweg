using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
namespace WegSolutionTomazinho
{
    class Program
    {
        struct MessageSchema
        {
            char name;
            char format;
            char fields;
        };
        struct TelemetrySchema
        {
            MessageSchema messageSchema;
        };
        struct TelemetryProperties
        {
            TelemetrySchema axialvibration;
            TelemetrySchema radialvibration;
            TelemetrySchema verticalvibration;
            TelemetrySchema motortemperature;
            TelemetrySchema environmenttemperature;
        };
        struct Motor
        {
            // Reported properties
            char protocol;
            char supportedMethods;
            char type;
            char firmware;
            // FIRMWARE_UPDATE_STATUS firmwareUpdateStatus;
            char location;
            double latitude;
            double longitude;
            TelemetryProperties telemetry;

            // Manage firmware update process
            char new_firmware_version;
            char new_firmware_URI;
        };

        public class Motor_telemetry
        {

            public Motor_telemetry()
            {
                this.deviceid = " ";
                this.axialvibration = 0;
                this.verticalvibration = 0;
                this.radialvibration = 0;
                this.motortemperature = 0;
                this.environmenttemperature = 0;
                //cadeiaJson = new List<Motor_telemetry>();
            }
            public Motor_telemetry(Motor_telemetry motor)
            {
                this.deviceid = motor.deviceid;
                this.axialvibration = motor.axialvibration;
                this.verticalvibration = motor.verticalvibration;
                this.radialvibration = motor.radialvibration;
                this.motortemperature = motor.motortemperature;
                this.environmenttemperature = motor.environmenttemperature;
                //cadeiaJson = new List<Motor_telemetry>();
            }
            public List<Motor_telemetry> cadeiaJson { get; set; }
            [JsonProperty("DEVICEID")]
            public String deviceid { get; set; }
            [JsonProperty("AXIALVIBRATION")]
            public double axialvibration { get; set; }
            [JsonProperty("VERTICALVIBRATION")]
            public double verticalvibration { get; set; }
            [JsonProperty("RADIALVIBRATION")]
            public double radialvibration { get; set; }
            [JsonProperty("MOTORTEMPERATURE")]
            public double motortemperature { get; set; }
            [JsonProperty("ENVIRONMENTTEMPERATURE")]
            public double environmenttemperature { get; set; }
            //public bool Equals(String motorvibra_device)
            //{//essa comparação vai ser criada para juntar o primeiro elemento criado pela requisição feita com primeiro da temp
            // //fazer update para realizar a comparação atraves da string data que pode ser consultada atravez do uso do Consultahttp
            //    if (this.deviceid == motorvibra_device)
            //        return true;
            //    else
            //        return false;

            //}
            public void juntaTudo(Motor_telemetry vibra)
            {
                this.motortemperature = vibra.motortemperature;
                this.environmenttemperature = vibra.environmenttemperature;
               
            }
            override public String ToString()
            {
                return "deviceid: " + this.deviceid + ",\n axialvibration: " + this.axialvibration + ",\n verticalvibration:" + this.verticalvibration +
                    ",\n radialvibration: " + this.radialvibration + ",\n motortemperature: " + this.motortemperature + "\n environmenttemperature:" + this.environmenttemperature;

            }
        };
        //static List<Motor_telemetry> listJson = new List<Motor_telemetry>();
       static Motor_telemetry motor_atual;
        //funções para a requisição 
        static String precaria = "http://iot-connect.weg.net/v1/customer/measurement/plant/a290495e74b24266881947ae5bcdce80/devices/ceb5c72000c0/variables/AXIALVIBRATION,RADIALVIBRATION,VERTICALVIBRATION/2020-03-10T02:00:00.000Z/2020-03-10T02:59:59.999Z?varSet=motor-fft-measurement&aggregateFunction=AVG&groupby=SECOND";
        
        static void AtribuindoValoresVibra( String consultaAtual)
        {
            //chaama as funções que traria a cadeia de conexão completa e atualizada
            try
            {

                String uri = consultaAtual;
               var requisicaoWeb = WebRequest.Create(uri);
                if (requisicaoWeb != null)
                {
                    requisicaoWeb.Method = "GET";//Neste código estamos criando um objeto HttpWebRequest e passando a URL para a qual queremos fazer a requisição GET.
                    requisicaoWeb.Timeout = 20000;
                    requisicaoWeb.ContentType = "application/json";
                    requisicaoWeb.Headers.Add("X-Api-Key", "67f39b57-f8c6-49e8-985e-6a845cdb2836");
                    requisicaoWeb.Headers.Add("X-Api-Secret", "ck$nxx.CDEy54-duWaf4hV#aE5cdfpM5kV-Lv$GdsTxozW6Zgphnq#M+PNF)au#q");
                    HttpWebResponse myHttpWebResponse = (HttpWebResponse)requisicaoWeb.GetResponse();

                    using (var resposta = requisicaoWeb.GetResponse())
                    {
                        var streamDados = resposta.GetResponseStream();
                        StreamReader reader = new StreamReader(streamDados);
                        object objResponse = reader.ReadToEnd();//le o fluxo de json e já separa em array 
                        string cadeia = @"" + objResponse.ToString();
                        var motor = JsonConvert.DeserializeObject<List<Motor_telemetry>>(cadeia);
                        Console.WriteLine("no vibra: "+motor[0].ToString()+"\n");
                        streamDados.Close();
                        resposta.Close();
                        AtribuindoValoresTemp(motor);
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

            }

        }
         static void AtribuindoValoresTemp(List<Motor_telemetry> motorAtual)
        {
            //chaama as funções que traria a cadeia de conexão completa e atualizada
            try
            {
                //https://iot-connect.weg.net/v1/customer/measurement/plant/a290495e74b24266881947ae5bcdce80/devices/ceb5c72000c0/variables/STATUS,MOTORTEMPERATURE,ENVIRONMENTTEMPERATURE/2019-08-18T16:00:00.000Z/2019-08-18T23:59:59.999Z?aggregateFunction=AVG&varSet=motor-short-measurement&groupby=SECOND#aE5cdfpM5kV-Lv$GdsTxozW6Zgphnq#M+PNF)au#q
                String uri = "https://iot-connect.weg.net/v1/customer/measurement/plant/a290495e74b24266881947ae5bcdce80/devices/ceb5c72000c0/variables/STATUS,MOTORTEMPERATURE,ENVIRONMENTTEMPERATURE/2020-03-10T02:00:00.000/2020-03-10T02:09:09.999Z?aggregateFunction=AVG&varSet=motor-short-measurement&groupby=SECOND";   
                    var requisicaoWeb = WebRequest.Create(uri);
                if (requisicaoWeb != null)
                {
                    requisicaoWeb.Method = "GET";//Neste código estamos criando um objeto HttpWebRequest e passando a URL para a qual queremos fazer a requisição GET.
                    requisicaoWeb.Timeout = 20000;
                    requisicaoWeb.ContentType = "application/json";
                    requisicaoWeb.Headers.Add("X-Api-Key", "67f39b57-f8c6-49e8-985e-6a845cdb2836");
                    requisicaoWeb.Headers.Add("X-Api-Secret", "ck$nxx.CDEy54-duWaf4hV#aE5cdfpM5kV-Lv$GdsTxozW6Zgphnq#M+PNF)au#q");
                    HttpWebResponse myHttpWebResponse = (HttpWebResponse)requisicaoWeb.GetResponse();

                    using (var resposta = requisicaoWeb.GetResponse())
                    {
                        var streamDados = resposta.GetResponseStream();
                        StreamReader reader = new StreamReader(streamDados);
                        object objResponse = reader.ReadToEnd();//le o fluxo de json e já separa em array 
                        string cadeia = @" " + objResponse.ToString();
                        List<Motor_telemetry> motorTemp =JsonConvert.DeserializeObject<List<Motor_telemetry>>(cadeia);
                        // listJson.AddRange(JsonConvert.DeserializeObject<List<Motor_telemetry>>(cadeia));uma ideia que fica, porem problematica.
                        // if (motorAtual.Equals(motorTemp.deviceid));
                        int i = 0;
                        while (i < motorAtual.Count)
                        {
                            motorAtual[i].juntaTudo(motorTemp[i]);
                            i++;
                        }
                        motor_atual = new Motor_telemetry(motorAtual[0]);
                        Console.WriteLine("objeto Completo:"motor_atual.ToString());
                        Console.ReadLine();
                        streamDados.Close();
                        resposta.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

            }

        }

         static void Main(string[] args)
        {
         
            AtribuindoValoresVibra(precaria);
        }
    }


}
