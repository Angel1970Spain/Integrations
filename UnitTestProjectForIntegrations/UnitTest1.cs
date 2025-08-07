using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SignatureIntegration;
using SignatureIntegration.External;
using SignatureIntegration.Model.Enums;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using UnitTestProjectForIntegrations.Data;


namespace UnitTestProjectForIntegrations
{
    [TestClass]
    public class UnitTest1
    {


        private class Doc
        {
            internal SignatureType SignType { get; set; }
            internal string Path { get; set; }
        }


        private ISignatureClientForV6 _client;

        private string _token = "";

        private List<Certificate> _certs = null;

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
        public void CastTheParams() 
        {
            try
            {
                var json = JsonConvert.SerializeObject( _client.CastTheParams(DataForTests.ParametersCades));

                Assert.AreEqual(json, DataForTests.JsonCastParsCades, "El casteo no se corresponde con el esperado");
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

            Certificate cert = _certs.First();

            var documents = GetConfDocs();

            foreach (var doc in documents.Where(x => x.SignType == SignatureType.CADES).Take(1)) 
            {
                var file = File.ReadAllBytes(doc.Path);

                var b64 = Convert.ToBase64String(file);

                string signed = "";

                switch (doc.SignType) 
                {
                    case SignatureType.PADES:

                        signed = _client.Sign(token: _token,
                                              type: doc.SignType,
                                              certid: cert.certid,
                                              certpin: "Abc123",
                                              profile: Profile.ENHANCED,
                                              extensions: "lt",
                                              document: file,
                                              parameters: _client.CastTheParams(DataForTests.ParametersPades));
                        break;

                    case SignatureType.CADES:

                        signed = _client.Sign(token: _token,
                                              type: doc.SignType,
                                              certid: cert.certid,
                                              certpin: "Abc123",
                                              profile: Profile.T,
                                              extensions: "lt",
                                              document: file, 
                                              parameters: null);
                        break;

                    case SignatureType.XADES:

                        break;
                }

                SaveDoc(signed, doc.SignType);
            } 

            Assert.IsTrue(true);
        }








        #region privathe methods

        private void SaveDoc(string signed, SignatureType stype) 
        {
            if (string.IsNullOrEmpty(signed)) return;

            try
            {
                byte[] pdfBytes = Convert.FromBase64String((JObject.Parse(signed))["data"]?.ToString());

                string ext = "";

                switch (stype) 
                {
                    case SignatureType.PADES:

                        ext = "pdf";

                        break;

                    default:

                        ext = "p7m";

                        break;
                }
                                
                File.WriteAllBytes($"c:/work/z_testdocs/{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}.{ext}", pdfBytes);
            }
            catch { }
        }


        private List<Doc> GetConfDocs()
        {
            List<Doc> documents = new List<Doc>();

            var confDocs = ConfigurationManager.GetSection("documents") as NameValueCollection;

            foreach (var key in confDocs.AllKeys)
            {
                documents.Add(new Doc
                {
                    SignType = (SignatureType)Enum.Parse(typeof(SignatureType), confDocs[key], ignoreCase: true),
                    Path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Documents", key)
                });
            }

            return documents;
        }

        #endregion privathe methods

    }
}
