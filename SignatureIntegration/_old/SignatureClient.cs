using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SignatureIntegration
{
	[Guid("E55FE3E0-7CC7-4272-B7C3-A0782CD4BA57")]
	[ClassInterface(ClassInterfaceType.None), ProgId("SignatureIntegration.client"), ComVisible(true)]
	public class SignatureClient : ISignatureClient
	{

        #region Variables

        private Rester restclient;

		#endregion

		#region Properties

		#endregion

		#region Constructor
		
		public SignatureClient()
		{
			
		}

		public SignatureClient(string url) {
			restclient = new Rester();
			restclient.baseurl = url;
		}

		/// <summary>
		/// Initialize rester
		/// </summary>
		/// <param name="url"></param>
		public void Initialize(string url) {
			restclient = new Rester();
			restclient.baseurl = url;
		}

		#endregion



		#region Public Methods
		
		/// <summary>
		/// Obtain token
		/// </summary>
		/// <param name="orgaid"></param>
		/// <param name="login"></param>
		/// <param name="password"></param>
		/// <param name="method"></param>
		/// <returns>token</returns>
		public string GetToken(string orgaid, string login, string password, string method, string modulekey, string module = "signatureintegration")
		{
			//prepare request
			old_AuhtLogin request = new old_AuhtLogin() {
				orgaid = orgaid,
				login = login,
				pass = password,
				module = module,
				modkey = modulekey,
				authmethod = method
			};

			NetworkCredential credential = null;
			if (method.ToLower() == AuthMethod.WIN.ToString().ToLower())
				credential = CredentialCache.DefaultNetworkCredentials;

			//do request
			dynamic response = Rester.Json.Deserialize<dynamic>(restclient.post(Rester.endpoints.authlogin, request, null, credential));

			//check response
			if (response["error"]["code"] != "K0000")
				throw new Exception($"Error on GetToken() ({response["error"]["message"]}) [{response["error"]["traceid"]}]");

			return response["token"];
		}

		/// <summary>
		/// Obtain certificates
		/// </summary>
		/// <param name="token"></param>
		/// <returns>List of certifficates</returns>
		public string GetCertificates(string token)
		{
			List<Certificate> listCertificates = null;
			
			//prepare request
			string request = "";
			Dictionary<string, string> headers = new Dictionary<string, string>() {
				{"Authentication", token }
			};
			
			//do request
			dynamic response = Rester.Json.Deserialize<dynamic>(restclient.post(Rester.endpoints.certlistavailable, request, headers));
			
			//check response
			if (response["error"]["code"] != "K0000")
				throw new Exception($"Error on GetToken() ({response["error"]["message"]}) [{response["error"]["traceid"]}]");
			
			//prepare response
			listCertificates = new List<Certificate>();
			for (int i = 0; i < response["certlist"].Length; i++) {

				Certificate cert = new Certificate() {
					certid = response["certlist"][i]["certid"],
					issuer = response["certlist"][i]["issuer"],
					serial = response["certlist"][i]["serial"],
					subject = response["certlist"][i]["subject"],
					userid = response["certlist"][i]["userid"],
					validfrom = Convert.ToDateTime(response["certlist"][i]["validfrom"]),
					validto = Convert.ToDateTime(response["certlist"][i]["validto"])
				};

				listCertificates.Add(cert);
			}

			return listCertificates.Serialize();
		}

		/// <summary>
		/// Sign document
		/// </summary>
		/// <param name="token"></param>
		/// <param name="signatureType"></param>
		/// <param name="certid"></param>
		/// <param name="certpin"></param>
		/// <param name="parameters"></param>
		/// <param name="detachedsignature"></param>
		/// <param name="document"></param>
		/// <returns></returns>
		public string Sign(string token, string signatureType, string certid, string certpin, string profile, string extensions, string parameters, string document, string hashalgorithm = "SHA256", string envelop = "", string detachedsignature = "")
		{
			//Prepare request
			Dictionary<string, string> headers = new Dictionary<string, string>() {
				{"Authentication", token }
			};

			dynamic response;

			switch (signatureType.ToLower()) {
				case "pades":
					var requestpades = new old_SignPades() {
						cert = new old_Cert() { certid = certid, pin = certpin },
						document = document,
						profile = profile,
						hashalgorithm = hashalgorithm,
						parameters = GetSignPadesParams(parameters),
						asyncdata = string.IsNullOrWhiteSpace(detachedsignature) ? null : Convert.FromBase64String(detachedsignature),
						force = false,
						extensions = extensions,
						operation = "sign"
					};

					response = Rester.Json.Deserialize<dynamic>(restclient.post(Rester.endpoints.signaturepades, requestpades, headers));

					break;
				case "xades":
					var requestxades = new old_SignXades() {
						cert = new old_Cert() { certid = certid, pin = certpin },
						document = document,
						profile = profile,
						hashalgorithm = "SHA256",
						parameters = GetSignXadesParams(parameters),
						signdata = string.IsNullOrWhiteSpace(detachedsignature) ? null : Convert.FromBase64String(detachedsignature),
						force = false,
						extensions = extensions,
						operation = "sign",
						envelop = envelop

					};

					response = Rester.Json.Deserialize<dynamic>(restclient.post(Rester.endpoints.signaturexades, requestxades, headers));
					break;
				case "cades":
					var requestcades = new old_SignCades() {
						cert = new old_Cert() { certid = certid, pin = certpin },
						document = document,
						profile = profile,
						hashalgorithm = "SHA256",
						parameters = GetSignCadesParams(parameters),
						signdata = string.IsNullOrWhiteSpace(detachedsignature) ? null : Convert.FromBase64String(detachedsignature),
						force = false,
						extensions = extensions,
						operation = "sign",
						envelop = envelop

					};

					response = Rester.Json.Deserialize<dynamic>(restclient.post(Rester.endpoints.signaturecades, requestcades, headers));
					break;
					
				default:
					throw new Exception("Invalid signature type");
			}

			if (response["error"]["code"] != "K0000")
				throw new Exception($"Error on Sign() ({response["error"]["message"]}) [{response["error"]["traceid"]}]");

			return response["data"];

		}

		/// <summary>
		/// Verify document
		/// </summary>
		/// <param name="token"></param>
		/// <param name="signatureType">enum</param>
		/// <param name="parameters"></param>
		/// <param name="detachedsignature"></param>
		/// <param name="document"></param>
		/// <returns></returns>
		public bool Verify(string token, SignatureTypes signatureType, string parameters,  string document, string documentpassword = "", string detachedsignature = "", ReferenceData[] refdata = null)
		{
			return Verify(token, signatureType.ToString().ToLower(), parameters, document, documentpassword, detachedsignature, refdata);
		}

		/// <summary>
		/// Verify document
		/// </summary>
		/// <param name="token"></param>
		/// <param name="signatureType"></param>
		/// <param name="parameters"></param>
		/// <param name="detachedsignature"></param>
		/// <param name="document"></param>
		/// <returns></returns>
		public bool Verify(string token, string signatureType, string parameters, string document, string documentpassword = "", string detachedsignature = "", ReferenceData[] refdata = null)
		{
			Dictionary<string, string> headers = new Dictionary<string, string>() {
				{"Authentication", token }
			};

			dynamic response;

			switch (signatureType.ToLower()) {

				case "pades":
					var requestpades = new old_VerifyPades() {
						document = document,
						options = parameters,
						password = documentpassword,
					};

					response = Rester.Json.Deserialize<dynamic>(restclient.post(Rester.endpoints.verifypades, requestpades, headers));
					break;
				case "xades":
					var requestxades = new old_VerifyXades() {
						document = document,
						options = parameters,
						detachedsignature = string.IsNullOrWhiteSpace(detachedsignature) ? null : Convert.FromBase64String(detachedsignature),
						externaldatareferences = refdata
					};

					response = Rester.Json.Deserialize<dynamic>(restclient.post(Rester.endpoints.verifyxades, requestxades, headers));
					break;
				case "cades":
					var requestcades = new old_VerifyCades() {
						document = document,
						options = parameters,
						detachedsignature = string.IsNullOrWhiteSpace(detachedsignature) ? null : Convert.FromBase64String(detachedsignature)
					};

					response = Rester.Json.Deserialize<dynamic>(restclient.post(Rester.endpoints.verifycades, requestcades, headers));
					break;

				default:
					throw new Exception("Invalid signature type");
			}

			if (response["error"]["code"] != "K0000")
				throw new Exception($"Error on Verify() ({response["error"]["message"]}) [{response["error"]["traceid"]}]");

			return response["valid"];
		}

		#endregion

		#region Private Methods
		
		private old_SignCadesParams GetSignCadesParams(string parameters)
		{
			if (string.IsNullOrWhiteSpace(parameters)) throw new Exception("Insuficient parameters");

			var paramsplit = parameters.Split(';');
			old_SignCadesParams cpp = new old_SignCadesParams();
			cpp.policy = new old_SignPolicy();

			foreach (var item in paramsplit) {

				var data = item.Split(new char[] { '=' }, 2);

				var paramname = data[0];
				var paramvalue = data[1];

				switch (paramname.ToLower()) {


					case "policy": cpp.policy = GetPolicyParameters(paramvalue); break;
					case "tsaparameters": cpp.tstampservers = new old_TimestampServerInfo[] { GetTSAParameters(paramvalue) }; break;

					default:
						break;
				}

			}

			return cpp;
		}

		private old_SignXadesParams GetSignXadesParams(string parameters)
		{

			if (string.IsNullOrWhiteSpace(parameters)) throw new Exception("Insuficient parameters");

			var paramsplit = parameters.Split(';');
			old_SignXadesParams xpp = new old_SignXadesParams();
			

			foreach (var item in paramsplit) {

				var data = item.Split(new char[] { '=' }, 2);

				var paramname = data[0];
				var paramvalue = data[1];

				switch (paramname.ToLower()) {
					case "signerrole": xpp.signerrole = paramvalue; break;
					case "policy": xpp.policy = GetPolicyParameters(paramvalue); break;
					case "tsaparameters": xpp.tstampserver = GetTSAParameters(paramvalue); break;

					default:
						break;
				}
			}

			return xpp;
		}

		private old_SignPadesParams GetSignPadesParams(string parameters)
		{

			if (string.IsNullOrWhiteSpace(parameters)) throw new Exception("Insuficient parameters");

			var paramsplit = parameters.Split(';');
			old_SignPadesParams spp = new old_SignPadesParams();

			spp.pdfparameters = new old_PDFSignParams();
			spp.pdfparameters.signbackgroundconfig = new old_PdfSignBackground();
			spp.pdfparameters.widgetprops = new old_PdfSignWidgetProps();

			spp.policy = new old_SignPolicy();

			foreach (var item in paramsplit) {

				var data = item.Split(new char[] { '=' },2);

				var paramname = data[0];
				var paramvalue = data[1];

				switch (paramname.ToLower()) {
					case "cause": spp.cause = paramvalue; break;
					case "pwd": spp.pdfparameters.pwd = paramvalue; break;
					case "signback": spp.pdfparameters.signbackgroundconfig.signback = Encoding.UTF8.GetBytes(paramvalue); break;
					case "signbackautostretch": spp.pdfparameters.signbackgroundconfig.signbackautostretch = Convert.ToBoolean(paramvalue); break;
					case "signvisible": spp.pdfparameters.signvisible = Convert.ToBoolean(paramvalue); break;
					case "autopos": spp.pdfparameters.widgetprops.autopos = Convert.ToBoolean(paramvalue); break;
					case "autosize": spp.pdfparameters.widgetprops.autosize = Convert.ToBoolean(paramvalue); break;
					case "hidetext": spp.pdfparameters.widgetprops.hidetext = Convert.ToBoolean(paramvalue); break;
					case "offsetx": spp.pdfparameters.widgetprops.offsetx = Convert.ToInt32(paramvalue); break;
					case "offsety": spp.pdfparameters.widgetprops.offsety = Convert.ToInt32(paramvalue); break;
					case "showonpages": spp.pdfparameters.widgetprops.showonpages = paramvalue; break;
					case "widgetpageoffset": spp.pdfparameters.widgetprops.widgetpageoffset = Convert.ToInt32(paramvalue); break;
					case "rotate": spp.pdfparameters.widgetprops.rotate = Convert.ToInt32(paramvalue); break;
					case "width": spp.pdfparameters.widgetprops.width = Convert.ToInt32(paramvalue); break;
					case "sizeheader": spp.pdfparameters.widgetprops.sizeheader = Convert.ToInt32(paramvalue); break;
					case "sizedatetime": spp.pdfparameters.widgetprops.sizedatetime = Convert.ToInt32(paramvalue); break;
					case "sizetitlesection": spp.pdfparameters.widgetprops.sizetitlesection = Convert.ToSingle(paramvalue); break;
					case "sizetextsection": spp.pdfparameters.widgetprops.sizetextsection = Convert.ToSingle(paramvalue); break;
					case "captionsigner": spp.pdfparameters.widgetprops.captionsigner = paramvalue; break;
					case "captionsignerinfo": spp.pdfparameters.widgetprops.captionsignerinfo = paramvalue; break;
					case "captionalgorithm": spp.pdfparameters.widgetprops.captionalgorithm = paramvalue; break;
					case "captionheader": spp.pdfparameters.widgetprops.captionheader = paramvalue; break;
					case "tstampserver": spp.tstampserver = new old_TimestampServerInfo[] { GetTSAParameters(paramvalue) }; break;
					case "policy": spp.policy = GetPolicyParameters(paramvalue); break;


					default:
						break;
				}

			}

			return spp;
		}

		private old_SignPolicy GetPolicyParameters(string valor)
		{
			var policy = new old_SignPolicy();
			var policyparams = valor.Split(',');

			foreach (var policyitem in policyparams)
			{
				var data = policyitem.Split(new char[] { '=' }, 2);

				var policyparamname = data[0];
				var policyparamvalue = data[1];

				switch (policyparamname.ToLower())
				{
					case "policyidentifier": policy.policyidentifier = policyparamvalue; break;
					case "policyidentifieraddqualifier": policy.policyidentifieraddqualifier = Convert.ToBoolean(policyparamvalue); break;
					case "policydigest": policy.policydigest = Convert.FromBase64String(policyparamvalue); break;
					case "policydigestalgorithm": policy.policydigestalgorithm = policyparamvalue; break;
					case "policyqualifieruri": policy.policyqualifieruri = policyparamvalue; break;
					case "policydescription": policy.policydescription = policyparamvalue; break;
					default:
						break;
				}
			}

			return policy;
		}

		private old_TimestampServerInfo GetTSAParameters(string valor)
		{
			var tsa = valor.Split(',');
			old_TimestampServerInfo tsaserver = new old_TimestampServerInfo();

			foreach (var tsaitem in tsa) {
				var data = tsaitem.Split(new char[] { '=' }, 2);

				var tsaname = data[0];
				var tsavalue = data[1];

				switch (tsaname.ToLower()) {
					case "certid": tsaserver.certid = tsavalue; break;
					case "hashalgorithm": tsaserver.hashalgorithm = tsavalue; break;
					case "httpauth": tsaserver.httpauth = Convert.ToBoolean(tsavalue); break;
					case "includecertificates": tsaserver.includecertificates = Convert.ToBoolean(tsavalue); break;
					case "name": tsaserver.name = tsavalue; break;
					case "password": tsaserver.password = tsavalue; break;
					case "pfx": tsaserver.pfx = tsavalue; break;
					case "pin": tsaserver.pin = tsavalue; break;
					case "policy": tsaserver.policy = tsavalue; break;
					case "url": tsaserver.url = tsavalue; break;
					case "usenonce": tsaserver.usenonce = Convert.ToBoolean(tsavalue); break;
					case "username": tsaserver.username = tsavalue; break;
					default:
						break;
				}
			}

			return tsaserver;
		}


		#endregion
		

    }

    public static class MyExtensions
	{
		public static T Deserialize<T>(this string toDeserialize)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			using (StringReader textReader = new StringReader(toDeserialize)) {
				return (T)xmlSerializer.Deserialize(textReader);
			}
		}

		public static string Serialize<T>(this T value)
		{
			if (value == null) {
				return string.Empty;
			}
			try {
				var xmlserializer = new XmlSerializer(typeof(T));
				var stringWriter = new StringWriter();
				using (var writer = XmlWriter.Create(stringWriter)) {
					xmlserializer.Serialize(writer, value);
					return stringWriter.ToString();
				}
			} catch (Exception ex) {
				throw new Exception("An error occurred", ex);
			}
		}
	}

}
