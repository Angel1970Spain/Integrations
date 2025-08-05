using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignatureIntegration;
using SignatureIntegration.External;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices.WindowsRuntime;


namespace UnitTestProjectForIntegrations
{
    [TestClass]
    public class UnitTest1
    {
        internal class Documents
        {
            internal string Key { get; set; }
            internal string Path { get; set; }
            internal string CheckResult { get; set; }
        }


        private ISignatureClientForV6 _client;

        private string _token = "";

        private List<old_Certificate> _certs = null;

        public UnitTest1() 
        {
            var uri = new Uri(ConfigurationManager.AppSettings["ApiUrl"]);
            var endpoints = GetEndpoints();

            _client = new SignatureClientForV6(uri, endpoints);
        }

        private Dictionary<string, Uri> GetEndpoints() 
        {
            var endpointsSection = (NameValueCollection)ConfigurationManager.GetSection("endpoints");

            var endpoints = endpointsSection.AllKeys
                .ToDictionary(key => key, key => new Uri(endpointsSection[key], UriKind.Relative));

            return endpoints;
        }

        [TestMethod]
        public void Test()
        {
            var r = _client.CheckTest();

            Assert.AreEqual(true, r);
        }

        [TestMethod]
        public void TestGeneralistic()
        {
            var r = _client.CheckGeneralistic();

            Assert.AreEqual(true, r);
        }



        [TestMethod]
        public void GetToken()
        {
            try
            {
                string orgaid = ConfigurationManager.AppSettings["orgaid"];
                string login = ConfigurationManager.AppSettings["login"];
                string pass = ConfigurationManager.AppSettings["pass"];
                string module = ConfigurationManager.AppSettings["module"];

                var r = _client.GetToken(orgaid, login, pass, module);

                Assert.IsNotNull(r, "El AccessToken no debe ser null.");
                Assert.IsInstanceOfType(r, typeof(string), "El AccessToken debe ser un string.");
                Assert.IsFalse(string.IsNullOrEmpty(r), "El AccessToken no debe ser null ni cadena vacía.");

                _token = r;

            }
            catch (Exception ex)
            {
                Assert.Fail($"Se lanzó una excepción inesperada: {ex.Message}");
            }
        }

        [TestMethod]
        public void GetTokens()
        {
            try
            {
                string orgaid = ConfigurationManager.AppSettings["orgaid"];
                string login = ConfigurationManager.AppSettings["login"];
                string pass = ConfigurationManager.AppSettings["pass"];
                string module = ConfigurationManager.AppSettings["module"];

                var r = _client.GetTokens(orgaid, login, pass, module);

                Assert.IsNotNull(r.Item1, "El AccessToken no debe ser null.");
                Assert.IsNotNull(r.Item2, "El RefreshToken no debe ser null.");

                Assert.IsInstanceOfType(r.Item1, typeof(string), "El AccessToken debe ser un string.");
                Assert.IsInstanceOfType(r.Item2, typeof(string), "El RefreshToken debe ser un string.");

                Assert.IsFalse(string.IsNullOrEmpty(r.Item1), "El AccessToken no debe ser null ni cadena vacía.");
                Assert.IsFalse(string.IsNullOrEmpty(r.Item2), "El RefreshToken no debe ser null ni cadena vacía.");

                _token = r.Item1;
            }
            catch (Exception ex)
            {
                Assert.Fail($"Se lanzó una excepción inesperada: {ex.Message}");
            }
        }

        [TestMethod]
        public void GetCertificates() 
        {
            try {

                if (string.IsNullOrEmpty(_token)) 
                {
                    GetToken();

                    if (string.IsNullOrEmpty(_token))
                    {
                        Assert.Fail("El AccessToken no debe ser null ni cadena vacía.");
                        return;
                    } 
                }

                string orgaid = ConfigurationManager.AppSettings["orgaid"];
                string userid = ConfigurationManager.AppSettings["login"];

                _certs = _client.GetCertificates(userid, orgaid, _token);

                Assert.IsNotNull(_certs, "La lista de certificados no debe ser null.");
                Assert.IsTrue(_certs.Count > 0, "La lista de certificados está vacía.");

            }
            catch (Exception ex)
            {
                Assert.Fail($"Se lanzó una excepción inesperada: {ex.Message}");
            }
        }

        [TestMethod]
        public void SignDocs()
        {
            if (_certs == null || _certs.Count == 0) 
            {
                GetCertificates();

                if (_certs == null || _certs.Count == 0)
                {
                    Assert.Fail("La lista de certificados no debe ser null o no puede está vacía.");
                    return;
                }
            }

            old_Certificate cert = _certs.First();

            List<Documents> documents = GetConfDocs();

            foreach (var doc in documents) 
            {
                var file = Convert.ToBase64String(File.ReadAllBytes(doc.Path));

                var signed = _client
                    .Sign
                    ( token:          _token, 
                      signatureType: "pades",
                      certid:        cert.certid, 
                      certpin:       "123123q", 
                      profile:       "enhanced", 
                      extensions:    "lt", 
                      parameters:    @"cause=test;autopos=true;autosize=true;hidetext=false;policy=policyidentifier=2.16.724.1.3.1.1.2.1.9,policydigest=G7roucf600+f03r/o0bAOQ6WAs0=,policydigestalgorithm=sha1,policiidentifieraddqualifier=true,policyqualifieruri=https://sede.060.gob.es/politica_de_firma_anexo_1.pdf", 
                      document:      file);
            } 

            Assert.IsTrue(true);
        }








        #region privathe methods

        internal List<Documents> GetConfDocs()
        {
            List<Documents> documents = new List<Documents>();

            var confDocs = ConfigurationManager.GetSection("documents") as NameValueCollection;

            foreach (var key in confDocs.AllKeys)
            {
                documents.Add(new Documents
                {
                    Key = key,
                    Path = Path.ChangeExtension(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Documents", key), ".pdf"),
                    CheckResult = confDocs[key]
                });
            }

            return documents;
        }

        #endregion privathe methods

    }
}
