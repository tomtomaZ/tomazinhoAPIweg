using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WegSolutionTomazinho
{
    class ConsultaHttp
	{
	
		String id_motor { get; set; }
		int min { get; set; }
		int hora { get; set; }
		String data_inicial { get; set; }
		String data_final { get; set; }
		String stringVibra { get; set; }
		String stringTempo { get; set; }
		public ConsultaHttp(String id_motor, String data_inicial, String data_Final) { 
		
		
		}
		public ConsultaHttp() { }


	}
}
