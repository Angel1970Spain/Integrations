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
using System.Threading.Tasks;

namespace SignatureIntegration.External.Common
{
    public abstract class CommonSignatureClient
    {
        private IConnectorForV6 _connector;

        private SignatureAuxLogic _auxLogic;

        protected Uri _baseUri = null;

        protected Dictionary<string, Uri> _endpoints;

        public CommonSignatureClient(Uri baseUri, Dictionary<string, Uri> endpoints)
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


        public void Initialize(string url)
        {
            throw new NotImplementedException();
        }


        public string GetCertificates
        (
            string token
        )
        {
            var endpoint = new Uri(_baseUri, _endpoints["CERTIFICATE"]);

            var jsonBody = new JObject();

            var ojson = _connector.PostAsync(endpoint, jsonBody, token).GetAwaiter().GetResult();

            var certificates = ojson["certlist"].ToObject<List<Certificate>>();

            return certificates.Serialize();
        }

        public List<Certificate> DeserializeCertificates(string certificates) 
        {
            return certificates.Deserialize<List<Certificate>>();   
        }

        public async Task<List<Certificate>> GetCertificatesAsync
        (
            string token,
            string userid = null,
            string orgaid = null
        )
        {
            var endpoint = new Uri(_baseUri, _endpoints["CERTIFICATE"]);

            var jsonBody = new JObject
            {
                ["orgaid"] = orgaid,
                ["userid"] = userid
            };

            var ojson = await _connector.PostAsync(endpoint, jsonBody, token);

            var r = ojson["certlist"].ToObject<List<Certificate>>();

            return r;
        }





        public SignPadesParams CastThePadesParams(string parameters) 
        {
            return _auxLogic.GetSignPadesParams(parameters);
        }

        public SignCadesParams CastTheCadesParams(string parameters)
        {
            return _auxLogic.GetSignCadesParams(parameters);
        }

        public SignXadesParams CastTheXadesParams(string parameters)
        {
            return _auxLogic.GetSignXadesParams(parameters);
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

            object o_params = null;
            object o_profile = null;

            switch (o_type) 
            {
                case SignatureType.PADES:

                    o_params = _auxLogic.GetSignPadesParams(parameters);
                    o_profile = (ProfilePades)Enum.Parse(typeof(ProfilePades), profile, ignoreCase: true);
                    break;

                case SignatureType.CADES: 

                    o_params = _auxLogic.GetSignCadesParams(parameters);
                    o_profile = (ProfileCades)Enum.Parse(typeof(ProfileCades), profile, ignoreCase: true);
                    break;

                case SignatureType.XADES: 

                    o_params = _auxLogic.GetSignXadesParams(parameters);
                    o_profile = (ProfileXades)Enum.Parse(typeof(ProfileXades), profile, ignoreCase: true);
                    break;
            }

            byte[] o_document = Convert.FromBase64String(document);

            HashAlgType o_hashAlgType = (HashAlgType)Enum.Parse(typeof(HashAlgType), hashAlgType, ignoreCase: true);

            var result = SignAsync(token, o_type, certid, certpin, o_profile, extensions, o_document, o_params, o_hashAlgType, envelop, detachedsignature).GetAwaiter().GetResult();
                   
            return result.ToString(Newtonsoft.Json.Formatting.Indented);
        }

        public async Task<JObject> SignAsync
        (
            string token,
            SignatureType type,
            string certid,
            string certpin,
            object profile,
            string extensions,
            byte[] document,
            object parameters = null,
            HashAlgType hashAlgType = HashAlgType.SHA256,
            string envelop = "",
            string detachedsignature = ""
        )
        {
            Uri endpoint = null;
            JObject jsonBody = null;

            switch (type) 
            {
                case SignatureType.PADES:

                    endpoint = new Uri(_baseUri, _endpoints["SIGN_PADES"]);

                    var bodyp = new SignaturePades 
                    {
                        cert = new Cert { certid = certid, pin = certpin },
                        document = document,
                        profile = ((ProfilePades)profile).ToString().ToLower(),
                        hashalgorithm = hashAlgType.ToString().ToLower(),
                        parameters = parameters as SignPadesParams,
                        asyncdata = string.IsNullOrWhiteSpace(detachedsignature) ? null : Convert.FromBase64String(detachedsignature),
                        extension = extensions,
                        operation = "sign"
                    };

                    jsonBody = JObject.FromObject(bodyp);

                    break;

                case SignatureType.CADES:

                    endpoint = new Uri(_baseUri, _endpoints["SIGN_CADES"]);

                    var bodyc = new SignatureCades
                    {
                        cert = new Cert { certid = certid, pin = certpin },
                        document = document,
                        profile = ((ProfileCades)profile).ToString().ToLower(),
                        hashalgorithm = HashAlgType.SHA256.ToString().ToLower(),
                        parameters = parameters as SignCadesParams,
                        signdata = string.IsNullOrWhiteSpace(detachedsignature) ? null : Convert.FromBase64String(detachedsignature),
                        extension = extensions,
                        operation = "sign",
                        envelop = envelop
                    };

                    jsonBody = JObject.FromObject(bodyc);

                    break;

                case SignatureType.XADES:

                    endpoint = new Uri(_baseUri, _endpoints["SIGN_XADES"]);

                    var bodyx = new SignatureXades
                    {
                        cert = new Cert { certid = certid, pin = certpin },
                        document = document,
                        profile = ((ProfileXades)profile).ToString().ToLower(),
                        hashalgorithm = HashAlgType.SHA256.ToString().ToLower(),
                        parameters = parameters as SignXadesParams,
                        signdata = string.IsNullOrWhiteSpace(detachedsignature) ? null : Convert.FromBase64String(detachedsignature),
                        extension = extensions,
                        operation = "sign",
                        envelop = envelop
                    };

                    var str = JsonConvert.SerializeObject(bodyx);

                    jsonBody = JObject.FromObject(bodyx);

                    break;
            }

            var ojson = await _connector.PostAsync(endpoint, jsonBody, token);

            return ojson;
        }


        public bool Verify
        (
            string token, 
            string signatureType, 
            string parameters, 
            string document, 
            string documentpassword = "", 
            string detachedsignature = "", 
            ReferenceData[] refdata = null
        )
        {
            SignatureType o_type = (SignatureType)Enum.Parse(typeof(SignatureType), signatureType, ignoreCase: true);

            byte[] o_document = Convert.FromBase64String(document);

            if (documentpassword == "") documentpassword = null;
            if (detachedsignature == "") detachedsignature = null;

            var jObj = VerifyAsync(token, o_type, o_document, options: parameters, documentpassword, detachedsignature, refdata).GetAwaiter().GetResult();

            return jObj["error"]["message"].ToString().ToLower() == "ok";
        }

        public async Task<JObject> VerifyAsync
        (
            string token, 
            SignatureType type, 
            byte[] document,
            string options = null,
            string documentpassword = null,
            string detachedsignature = null,
            ReferenceData[] refdata = null
        )
        {
            Uri endpoint = null;
            JObject jsonBody = null;

            switch (type)
            {
                case SignatureType.PADES:

                    endpoint = new Uri(_baseUri, _endpoints["VERIFY_PADES"]);

                    var bodyp = new VerifyPades
                    {
                        document = document,
                        password = documentpassword,
                        options = options
                    };
                    
                    jsonBody = JObject.FromObject(bodyp);

                    break;

                case SignatureType.CADES:

                    endpoint = new Uri(_baseUri, _endpoints["VERIFY_CADES"]);

                    var bodyc = new VerifyCades
                    {
                        options = options,
                        document = document, 
                        detachedsignature = detachedsignature

                    };

                    jsonBody = JObject.FromObject(bodyc);

                    break;

                case SignatureType.XADES:

                    endpoint = new Uri(_baseUri, _endpoints["VERIFY_XADES"]);

                    var bodyx = new VerifyXades
                    {
                        options = options,
                        document = document,
                        detachedsignature = detachedsignature,
                        ExternalReferences = refdata
                    };

                    jsonBody = JObject.FromObject(bodyx);

                    break;
            }

            var ojson = await _connector.PostAsync(endpoint, jsonBody, token);

            return ojson;
        }

    }
}
