using System;
using System.Runtime.InteropServices;

namespace SignatureIntegration
{

	//[Guid("2A7C32A5-C1D8-4D46-B3CB-C2561A73FB63"), ComVisible(true), ProgId("SignatureIntegration.certificate")]
	public class Certificate
	{
		public string certid { get; set; }
		public string userid { get; set; }
		public string issuer { get; set; }
		public string subject { get; set; }
		public string serial { get; set; }
		public DateTime validfrom { get; set; }
		public DateTime validto { get; set; }
		public string cer { get; set; }

	}

	internal class Cert {

		public string certid { get; set; }
		public string pin { get; set; }
	}
}
