using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http.Headers;
namespace WegSolutionTomazinho
{
    class Program
    {
         struct MessageSchema
        {
            char name;
            char format;
            char fields;
        } ;
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
        public class Motor_Json
        {
            
            public List<Motor_telemetry> cadeiaJson { get; set; }
            public Motor_Json()
            {
                cadeiaJson = new List<Motor_telemetry>();
            }


        };
            public class Motor_telemetry
        {
           
            public Motor_telemetry() {
                this.deviceid = " ";
                this.axialvibration = 0;
                this.verticalvibration = 0;
                this.radialvibration = 0;
                this.motortemperature = 0;
                this.environmenttemperature = 0;

            }

            [JsonProperty("DEVICEID")]
            String deviceid { get; set; }
            [JsonProperty("AXIALVIBRATION")]
            double axialvibration { get; set; }
            [JsonProperty("VERTICALVIBRATION")]
            double verticalvibration { get; set; }
            [JsonProperty("RADIALVIBRATION")]
            double radialvibration { get; set; }
            [JsonProperty("name")]
            double motortemperature { get; set; }
            [JsonProperty("name")]
            double environmenttemperature { get; set; }

           override public String ToString()
            {
                return this.deviceid + ", " + this.axialvibration+", "+this.verticalvibration+
                    ", "+this.radialvibration+"\n";

            }
        };


        //funções para a requisição 
        static void AtribuindoValoresVibra() {
            //chaama as funções que traria a cadeia de conexão completa e atualizada
            try
            {
               
                String uri= "http://iot-connect.weg.net/v1/customer/measurement/plant/a290495e74b24266881947ae5bcdce80/devices/ceb5c72000c0/variables/AXIALVIBRATION,RADIALVIBRATION,VERTICALVIBRATION/2020-03-10T02:00:00.000Z/2020-03-10T02:59:59.999Z?varSet=motor-fft-measurement&aggregateFunction=AVG&groupby=SECOND";
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
                        List<Motor_Json> listJson = JsonConvert.DeserializeObject<List<Motor_Json>>(cadeia);

                     
                        Console.WriteLine(listJson.ToString());
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
            AtribuindoValoresVibra();
        }
    }


}
