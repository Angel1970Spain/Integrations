using System;
using System.Runtime.InteropServices;

namespace SignatureIntegration
{
	#region SIGN
	internal class old_SignPades
	{
		public old_Cert cert { get; set; }
		public string document { get; set; } //documento que vamos a firmar, puede estar firmado o no
		public byte[] asyncdata { get; set; } //firma en modo detached cuando sea necesaria
		public string profile { get; set; } //hasta donde sabemos puede ser BASIC ó ENHANCED (enhanced por defecto es BES, si queremos EPES lo pasamos por la extension... que es lo que mas seajusta a la realidad)
		public string extensions { get; set; } // T , EPES , LTV  - t: aplicar sellado de tiempo en la firma - EPES: añadir la politica de firmado - LTV: resellar la firma (tanto ves como epes con o sin tiempo). NOTA: a Valide le gusta resellados donde la primera firma NO TENGA T, ok? :)
		public string hashalgorithm { get; set; } // sha1....
		public string operation { get; set; } // sign, cosign, upgrade, append 
		public bool? force { get; set; }//saltar las validaciones de certificado y cadena 
		public string[][] extradata { get; set; }
		public old_SignPadesParams parameters { get; set; }
	}
	internal class old_SignXades
	{
		public old_Cert cert { get; set; }
		public string document { get; set; } //documento que vamos a firmar, puede estar firmado o no
		public byte[] signdata { get; set; } //firma en modo detached cuando sea necesaria
		public string profile { get; set; } //hasta donde sabemos puede ser BASIC ó ENHANCED (enhanced por defecto es BES, si queremos EPES lo pasamos por la extension... que es lo que mas seajusta a la realidad)
		public string extensions { get; set; } // T , EPES , LTV  - t: aplicar sellado de tiempo en la firma - EPES: añadir la politica de firmado - LTV: resellar la firma (tanto ves como epes con o sin tiempo). NOTA: a Valide le gusta resellados donde la primera firma NO TENGA T, ok? :)
		public string hashalgorithm { get; set; } // sha1....
		public string envelop { get; set; } // enveloped , enveloping , detached
		public string operation { get; set; } // sign, cosign, upgrade, append 
		public bool? force { get; set; } //saltar las validaciones de certificado y cadena
		public string[][] extradata { get; set; }
		public old_SignXadesParams parameters { get; set; }
	}
	internal class old_SignCades
	{
		public old_Cert cert { get; set; }
		public string document { get; set; } //documento que vamos a firmar, puede estar firmado o no
		public byte[] signdata { get; set; } //firma en modo detached cuando sea necesaria
		public string profile { get; set; } //hasta donde sabemos puede ser BASIC ó ENHANCED (enhanced por defecto es BES, si queremos EPES lo pasamos por la extension... que es lo que mas seajusta a la realidad)
		public string extensions { get; set; } // T , EPES , LTV  - t: aplicar sellado de tiempo en la firma - EPES: añadir la politica de firmado - LTV: resellar la firma (tanto ves como epes con o sin tiempo). NOTA: a Valide le gusta resellados donde la primera firma NO TENGA T, ok? :)
		public string hashalgorithm { get; set; } // sha1....
		public string envelop { get; set; } // enveloped , enveloping , detached
		public string operation { get; set; } // sign, cosign, upgrade, append 
		public bool? force { get; set; }//saltar las validaciones de certificado y cadena
		public string[][] extradata { get; set; }
		public old_SignCadesParams parameters { get; set; }
	}
	internal class old_SignPadesParams
	{
		public string cause { get; set; }
		public old_SignPolicy policy { get; set; }
		public old_PDFSignParams pdfparameters { get; set; }
		public old_TimestampServerInfo[] tstampserver { get; set; }
		public old_Biometry biometry { get; set; }

	}
	internal class old_SignXadesParams
	{
		public string signerrole { get; set; }
		public old_SignPolicy policy { get; set; }
		public old_TimestampServerInfo tstampserver { get; set; }
	}
	internal class old_SignCadesParams
	{
		public old_SignPolicy policy { get; set; }
		public old_TimestampServerInfo[] tstampservers { get; set; }
	}
	internal class old_TimestampServerInfo
	{
		public string name { get; set; }
		public string url { get; set; }
		public bool? httpauth { get; set; }
		public string username { get; set; }
		public string password { get; set; }
		public bool? usenonce { get; set; }
		public bool? includecertificates { get; set; }
		public string hashalgorithm { get; set; }
		public string certid { get; set; }
		public string pfx { get; set; }
		public string pin { get; set; }
		public string policy { get; set; }

	}
	internal class old_SignPolicy
	{
		public string policyidentifier { get; set; }
		public bool? policyidentifieraddqualifier { get; set; }
		public string policydescription { get; set; }
		public byte[] policydigest { get; set; }
		public string policydigestalgorithm { get; set; }
		public string policyqualifieruri { get; set; }
	}
	internal class old_PDFSignParams
	{
		public string pwd { get; set; }
		public bool? signvisible { get; set; }
		public old_PdfSignBackground signbackgroundconfig { get; set; }
		public old_PdfSignWidgetProps widgetprops { get; set; }
	}
	internal class old_PdfSignBackground
	{
		public byte[] signback { get; set; }
		public bool? signbackautostretch { get; set; }
		public int? stretchx { get; set; }
		public int? stretchy { get; set; }
		public old_TransparencyMask transparencymask { get; set; }
	}
	internal class old_TransparencyMask
	{
		public int? red { get; set; }
		public int? redtolerance { get; set; }
		public int? green { get; set; }
		public int? greentolerance { get; set; }
		public int? blue { get; set; }
		public int? bluetolerance { get; set; }
		public int? tolerance { get; set; }
	}
	internal class old_PdfSignWidgetProps
	{
		public bool? autopos { get; set; }
		public int? offsetx { get; set; }
		public int? offsety { get; set; }
		public bool? autosize { get; set; }
		public int? height { get; set; }
		public int? width { get; set; }
		public int? rotate { get; set; }
		public string showonpages { get; set; }
		public int? widgetpageoffset { get; set; }
		public bool? hidetext { get; set; }

		public float? sizeheader;

		public float? sizedatetime;

		public float? sizetitlesection;

		public float? sizetextsection;

		public string captionsigner;

		public string captionsignerinfo;

		public string captionalgorithm;

		public string captionheader;
	}
	internal class old_Biometry
	{
		public byte[] data { get; set; }
		public byte[] cer { get; set; }
	}

	#endregion

	#region VERIFY

	internal class old_VerifyPades
	{
		public string options { get; set; }
		public string document { get; set; }
		public string password { get; set; }
	}

	internal class old_VerifyXades
	{

		public string options { get; set; }
		public string document { get; set; }
		public byte[] detachedsignature { get; set; }
		public ReferenceData[] externaldatareferences { get; set; }
	}

	internal class old_VerifyCades
	{

		public string options { get; set; }
		public string document { get; set; }
		public byte[] detachedsignature { get; set; }
	}

	[Guid("75E42C56-87E9-4589-8CBE-75580E68B871"), ComVisible(true), ProgId("SignatureIntegration.referencedata")]
	public class ReferenceData
	{
		public byte[] data { get; set; }
		public string uri { get; set; }
		public bool isdigest { get; set; }
		public string idprefix { get; set; }
		public string idns { get; set; }
		public bool detached { get; set; }
		public string mimetype { get; set; }
	}

		#endregion

	}
