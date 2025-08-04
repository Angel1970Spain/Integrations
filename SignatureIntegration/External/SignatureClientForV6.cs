using Newtonsoft.Json.Linq;
using SignatureIntegration.Connector;
using SignatureIntegration.Model.Enums;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SignatureIntegration.External
{
    public class SignatureClientForV6: ISignatureClientForV6
    {
        private IConnectorForV6 _connector;

        private Uri _baseUri = null;

        private Dictionary<string, Uri> _endpoints;

        public SignatureClientForV6(Uri baseUri, Dictionary<string, Uri> endpoints) 
        {
            _connector = new ConnectorForV6();
            _baseUri = baseUri;
            _endpoints = endpoints;

            CheckUrls();
        }

        private void CheckUrls() 
        {
            if (!_baseUri.ToString().EndsWith("/"))
            {
                _baseUri = new Uri(_baseUri.ToString() + "/");
            }

            foreach (var key in _endpoints.Keys.ToList())
            {
                Uri uri = _endpoints[key];

                if (uri.IsAbsoluteUri == false && uri.OriginalString.StartsWith("/"))
                {
                    _endpoints[key] = new Uri(uri.OriginalString.Substring(1), UriKind.Relative);
                }
            }
        }


        public bool CheckTest() => true;


        public string GetToken
        (
            string orgaid,
            string login,
            string pass,
            string module,
            AuthMethod authmethod = AuthMethod.PASS,
            string origin = null,
            string modkey = null,
            string modver = null,
            string deviceinfo = null
        )
        {
            var tokens = GetTokens(orgaid, login, pass, module, authmethod, origin, modkey, modver, deviceinfo);

            return tokens.Item1;
        }


        public Tuple<string, string> GetTokens(
            string orgaid,
            string login,
            string pass,
            string module,
            AuthMethod authmethod = AuthMethod.PASS,
            string origin = null,
            string modkey = null,
            string modver = null,
            string deviceinfo = null
        )
        {
            var endpoint = new Uri(_baseUri, _endpoints["TOKEN"]);

            var jsonBody = new JObject
            {
                ["orgaid"] = orgaid,
                ["login"] = login,
                ["pass"] = pass,
                ["module"] = module,
                ["authmethod"] = authmethod.ToString()
            };

            if (origin != null) jsonBody["origin"] = origin;
            if (modkey != null) jsonBody["modkey"] = modkey;
            if (modver != null) jsonBody["modver"] = modver;
            if (deviceinfo != null) jsonBody["deviceinfo"] = deviceinfo;

            NetworkCredential credentials = authmethod == AuthMethod.WIN 
                ? CredentialCache.DefaultNetworkCredentials 
                : null;

            var ojson = _connector.PostAsync(endpoint, jsonBody, credentials).GetAwaiter().GetResult();

            var accessToken = ojson["token"].ToString();
            var refreshToken = ojson["refreshToken"].ToString();

            return new Tuple<string,string>(accessToken, refreshToken);
        }

        public List<Certificate> GetCertificates(string userid, string orgaid, string token)
        {
            var endpoint = new Uri(_baseUri, _endpoints["CERTIFICATE"]);

            var jsonBody = new JObject
            {
                ["orgaid"] = orgaid,
                ["userid"] = userid
            };

            var ojson = _connector.PostAsync(endpoint, jsonBody, token).GetAwaiter().GetResult();

            var r = ojson["certlist"].ToObject<List<Certificate>>();

            return r;
        }


        public string Sign
        (
            string token,
            string signatureType,
            string certid,
            string certpin,
            string profile,
            string extensions,
            string parameters,
            string document,
            string hashalgorithm = "SHA256",
            string envelop = "",
            string detachedsignature = ""
        )
        {



            return "";
        }
    }
}
