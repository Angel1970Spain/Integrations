using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Script.Serialization;

namespace SignatureIntegration
{
	internal class Rester
	{
		internal class endpoints {
			public static readonly string authlogin = "/auth/login";

			public static readonly string certlistavailable = "/cert/listavailable";

			public static readonly string signaturepades = "/signature/pades";
			public static readonly string signaturexades = "/signature/xades";
			public static readonly string signaturecades = "/signature/cades";

			public static readonly string verifypades = "/verify/pades";
			public static readonly string verifyxades = "/verify/xades";
			public static readonly string verifycades = "/verify/cades";
		}

		// Variables internas
		private string _useragent;
		private string _baseurl;

		// Propiedades
		public string useragent { get => _useragent; set => _useragent = value; }
		public string baseurl { get => _baseurl; set => _baseurl = value; }

		// Constructor
		public Rester()
		{
			// defaults
			useragent = "Rester";
		}

		// Métodos
		
		public string post(string endpoint, object msg, Dictionary<string, string> headers, NetworkCredential credentials = null)
		{
			string url = baseurl + endpoint;
			
			// Preparamos HttpClientHandler con proxy si es necesario
			var handler = new HttpClientHandler();
			var proxy = WebRequest.GetSystemWebProxy();
			var resource = new Uri(url);
			var resourceproxy = proxy.GetProxy(resource);
			if (resource != resourceproxy) {
				handler.Proxy = new WebProxy() { Credentials = CredentialCache.DefaultCredentials, Address = resourceproxy };
			}

			if (credentials != null)
				handler.Credentials = credentials;

			// Preparamos HttpClient, con post y headers
			var cli = new HttpClient(handler, true);
			cli.DefaultRequestHeaders.UserAgent.ParseAdd(_useragent);
			var js = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
			var post = js.Serialize(msg);

			HttpContent data = new StringContent(post, Encoding.UTF8, "application/json");

			if(headers != null)
			foreach (var h in headers) {
				data.Headers.Add(h.Key, h.Value);
			}

			// Enviamos y recogemos respuesta
			string reply = null;
			try {
				reply = cli.PostAsync(url, data).GetAwaiter().GetResult().Content.ReadAsStringAsync().Result;
			} catch (Exception) {
				throw;
			}

			return reply;

		}

		public static JavaScriptSerializer Json = new JavaScriptSerializer() { MaxJsonLength = int.MaxValue };
	}
}