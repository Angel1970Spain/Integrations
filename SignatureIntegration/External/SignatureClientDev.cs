using Newtonsoft.Json.Linq;
using SignatureIntegration.Connector;
using SignatureIntegration.External.Common;
using SignatureIntegration.Model.Enums;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SignatureIntegration.External
{
    public class SignatureClientDev: CommonSignatureClient, ISignatureClientV6
    {
        private IConnectorForV6 _connector;

        public SignatureClientDev(Uri baseUri, Dictionary<string, Uri> endpoints) : base(baseUri, endpoints)
        {
            _connector = new ConnectorForV6();
        }


        public string GetToken
        (
            string orgaid, 
            string login, 
            string password, 
            string method, 
            string modulekey, 
            string module = "signatureintegration"
        )
        {
            AuthMethod o_method = (AuthMethod)Enum.Parse(typeof(AuthMethod), method, ignoreCase: true);

            return GetTokenAsync(orgaid, login, password, module, o_method, null, modulekey, null, null).GetAwaiter().GetResult();
        }

        public async Task<string> GetTokenAsync
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

            var ojson = await _connector.PostAsync(endpoint, jsonBody, credentials);

            var accessToken = ojson["token"].ToString();

            return accessToken;
        }

    }
}
