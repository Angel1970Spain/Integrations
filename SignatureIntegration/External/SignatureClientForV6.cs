using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SignatureIntegration.Connector;
using SignatureIntegration.InternalLogic;
using SignatureIntegration.Model;
using SignatureIntegration.Model.Enums;
using SignatureIntegration.Model.Iv6ClassModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;

namespace SignatureIntegration.External
{
    public class SignatureClientForV6: ISignatureClientForV6
    {
        private IConnectorForV6 _connector;

        private SignatureAuxLogic _auxLogic;

        private Uri _baseUri = null;

        private Dictionary<string, Uri> _endpoints;

        public SignatureClientForV6(Uri baseUri, Dictionary<string, Uri> endpoints) 
        {
            _connector = new ConnectorForV6();
            _auxLogic = new SignatureAuxLogic();

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


        public bool CheckTest() 
        {

            return true;
        } 


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





        public List<Certificate> GetCertificates
        (
            string userid, 
            string orgaid, 
            string token
        )
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




        public SignPadesParams CastTheParams(string parameters) 
        {
            return _auxLogic.GetSignPadesParams(parameters);
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
            string hashAlgType = "SHA256",
            string envelop = "",
            string detachedsignature = ""
        )
        {
            SignatureType o_type = (SignatureType)Enum.Parse(typeof(SignatureType), signatureType, ignoreCase: true);

            Profile o_profile = (Profile)Enum.Parse(typeof(Profile), profile, ignoreCase: true);

            SignPadesParams o_params = _auxLogic.GetSignPadesParams(parameters);

            byte[] o_document = Convert.FromBase64String(document);

            HashAlgType o_hashAlgType = (HashAlgType)Enum.Parse(typeof(HashAlgType), hashAlgType, ignoreCase: true);

            return Sign( token, o_type, certid, certpin, o_profile, extensions, o_document, o_params, o_hashAlgType, envelop, detachedsignature );
        }

        public string Sign
        (
            string token,
            SignatureType type,
            string certid,
            string certpin,
            Profile profile,
            string extensions,
            byte[] document,
            SignPadesParams parameters = null,
            HashAlgType hashAlgType = HashAlgType.SHA256,
            string envelop = "",
            string detachedsignature = ""
        )
        {
            Uri endpoint = null;
            SignaturePades body = null;

            switch (type) 
            {
                case SignatureType.PADES:

                    endpoint = new Uri(_baseUri, _endpoints["SIGN_PADES"]);

                    body = new SignaturePades 
                    {
                        cert = new Cert { certid = certid, pin = certpin },
                        document = document,
                        profile = profile.ToString().ToLower(),
                        hashalgorithm = hashAlgType.ToString().ToLower(),
                        parameters = parameters,
                        asyncdata = string.IsNullOrWhiteSpace(detachedsignature) ? null : Convert.FromBase64String(detachedsignature),
                        extension = extensions,
                        operation = "sign"
                    };

                    break;

                case SignatureType.CADES:

                    endpoint = new Uri(_baseUri, _endpoints["SIGN_CADES"]);

                    body = new SignaturePades
                    {
                        cert = new Cert { certid = certid, pin = certpin },
                        document = document,
                        profile = profile.ToString().ToLower(),
                        hashalgorithm = HashAlgType.SHA256.ToString().ToLower(),
                        parameters = parameters,
                        asyncdata = string.IsNullOrWhiteSpace(detachedsignature) ? null : Convert.FromBase64String(detachedsignature),
                        extension = extensions,
                        operation = "sign"
                    };

                    break;

                case SignatureType.XADES:

                    return "";
            }

            var jsonBody = JObject.FromObject(body);

            var ojson = _connector.PostAsync(endpoint, jsonBody, token).GetAwaiter().GetResult();

            string json = ojson.ToString(Newtonsoft.Json.Formatting.Indented);

            return json;
        }
    }
}
