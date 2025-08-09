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
using UnitTestProjectForIntegrations.Data;

namespace UnitTestProjectForIntegrations
{
    [TestClass]
    public class UnitTest1
    {

        private ISignatureClientForV6 _client;

        private string _token = "";

        private List<Certificate> _certs = null;

        public UnitTest1() 
        {
            var uri = new Uri(ConfigurationManager.AppSettings["ApiUrl"]);
            var endpoints = GetEndpoints();

            _client = new SignatureClientForV6(uri, endpoints);

            DataForTests.Documents
                .ForEach(d => d.Path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Documents", d.Name));
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
                var jsonPades = JsonConvert.SerializeObject( _client.CastThePadesParams(DataForTests.ParametersPades));

                var jsonCades = JsonConvert.SerializeObject(_client.CastTheCadesParams(DataForTests.ParametersCades));

                Assert.AreEqual(jsonPades, DataForTests.CheckCastParsPades, "El casteo para el documento Pades no se corresponde con el esperado");
                
                Assert.AreEqual(jsonCades, DataForTests.CheckCastParsCades, "El casteo para el documento Cades no se corresponde con el esperado");
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

            foreach (var doc in DataForTests.Documents.Where(x => x.SignType == SignatureType.PADES).Take(1)) 
            {
                var file = File.ReadAllBytes(doc.Path);

                var b64 = Convert.ToBase64String(file);

                JObject jObj = null;

                switch (doc.SignType) 
                {
                    case SignatureType.PADES:

                        jObj = JObject.Parse( _client.Sign(token: _token,
                                                           type: doc.SignType,
                                                           certid: cert.certid,
                                                           certpin: "Abc123",
                                                           profile: Profile.ENHANCED,
                                                           extensions: "lt",
                                                           parameters: _client.CastThePadesParams(DataForTests.ParametersPades),
                                                           document: file));

                        Assert.AreEqual((string)jObj["error"]["message"], "OK", $"Error cod. {jObj["error"]["code"]} en la firma del documento: {jObj["error"]["message"]} ");

                        break;

                    case SignatureType.CADES:

                        jObj = JObject.Parse( _client.Sign(token: _token,
                                                           type: doc.SignType,
                                                           certid: cert.certid,
                                                           certpin: "Abc123",
                                                           profile: Profile.T,
                                                           extensions: "lt",
                                                           parameters: _client.CastTheCadesParams(DataForTests.ParametersCades),
                                                           document: file));

                        Assert.AreEqual((string)jObj["error"]["message"], "OK", $"Error cod. {jObj["error"]["code"]} en la firma del documento: {jObj["error"]["message"]} ");

                        break;

                    case SignatureType.XADES:

                        break;
                }

                if( bool.Parse(ConfigurationManager.AppSettings["savesigneddocs"]) ) SaveDoc(jObj, doc.SignType);
            } 

            Assert.IsTrue(true);
        }








        #region privathe methods

        private void SaveDoc(JObject jObj, SignatureType stype) 
        {
            try
            {
                byte[] pdfBytes = Convert.FromBase64String(jObj["data"]?.ToString());

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

                string filepath = 
                    Path.Combine(ConfigurationManager.AppSettings["ouputdirectory"], $"{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}.{ext}")
                    .Replace('/', Path.DirectorySeparatorChar)
                    .Replace('\\', Path.DirectorySeparatorChar);

                Directory.CreateDirectory(Path.GetDirectoryName(filepath));
                File.WriteAllBytes(filepath, pdfBytes);
            }
            catch { }
        }

        #endregion privathe methods

    }
}
