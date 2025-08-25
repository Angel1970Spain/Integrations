using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SignatureIntegration.External;
using SignatureIntegration.Model;
using SignatureIntegration.Model.Enums;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UnitTestProjectForIntegrations.Data;
using UnitTestProjectForIntegrations.Model;

namespace UnitTestProjectForIntegrations
{
    [TestClass]
    public class UnitTest1
    {
        private NameValueCollection _settings;

        private ISignatureClientV6 _client;

        private string _token = "";

        private List<Certificate> _certs = null;

        private string _certPin = "";

        private string _certId = "";


        public UnitTest1() 
        {
            Uri uri;
            var endpoints = GetEndpoints();

            switch (ConfigurationManager.AppSettings["environment"]) 
            {
                case "DEV":

                    _settings = (NameValueCollection)ConfigurationManager.GetSection("appSettings_DEV");

                    uri = new Uri(_settings["ApiUrl"]);

                    _client = new SignatureClientDev(uri, endpoints);

                    break;

                case "PRO":

                    _settings = (NameValueCollection)ConfigurationManager.GetSection("appSettings_PRO");

                    uri = new Uri(_settings["ApiUrl"]);

                    _client = new SignatureClient(uri, endpoints);

                    break;
            }


            _certPin = _settings["certpin"];
            _certId = _settings["certid"];

            DataForTests.Documents
                .ForEach(d => d.Path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Documents", d.Name));
        }








        [TestMethod]
        public void GetToken_Compatible()
        {
            try
            {
                string orgaid = _settings["orgaid"];
                string login = _settings["login"];
                string password = _settings["pass"];
                string module = _settings["module"];

                var r = _client.GetToken(orgaid, login, password, "pass", null, module);

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
        public async Task GetToken()
        {
            try
            {
                string orgaid = _settings["orgaid"];
                string login = _settings["login"];
                string pass = _settings["pass"];
                string module = _settings["module"];

                var r = await _client.GetTokenAsync(orgaid, login, pass, module);

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
        public void GetCertificates_Compatible()
        {
            try
            {

                if (string.IsNullOrEmpty(_token))
                {
                    GetToken_Compatible();
                    if (string.IsNullOrEmpty(_token)) { Assert.Fail("El AccessToken no debe ser null ni cadena vacía."); return; }
                }

                var certstr = _client.GetCertificates(_token);
                _certs = _client.DeserializeCertificates(certstr);

                Assert.IsNotNull(_certs, "La lista de certificados no debe ser null.");
                Assert.IsTrue(_certs.Count > 0, "La lista de certificados está vacía.");

            }
            catch (Exception ex)
            {
                Assert.Fail($"Se lanzó una excepción inesperada: {ex.Message}");
            }
        }


        [TestMethod]
        public async Task GetCertificates() 
        {
            try {

                if (string.IsNullOrEmpty(_token)) 
                {
                    await GetToken();
                    if (string.IsNullOrEmpty(_token)) { Assert.Fail("El AccessToken no debe ser null ni cadena vacía."); return; } 
                }


                string orgaid = _settings["orgaid"];
                string userid = _settings["login"];

                _certs = await _client.GetCertificatesAsync(_token, userid, orgaid );

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

                var jsonXades = JsonConvert.SerializeObject(_client.CastTheXadesParams(DataForTests.ParametersXades));

                Assert.AreEqual(jsonPades, DataForTests.CheckCastParsPades, "El casteo para el documento Pades no se corresponde con el esperado");
                
                Assert.AreEqual(jsonCades, DataForTests.CheckCastParsCades, "El casteo para el documento Cades no se corresponde con el esperado");

                Assert.AreEqual(jsonXades, DataForTests.CheckCastParsXades, "El casteo para el documento Cades no se corresponde con el esperado");
            }
            catch (Exception ex)
            {
                Assert.Fail($"Se lanzó una excepción inesperada: {ex.Message}");
            }
        }






        [TestMethod]
        public void SignDocs_Compatible()
        {
            try
            {
                if (_certs == null || _certs.Count == 0)
                {
                    GetCertificates_Compatible();
                    if (_certs == null || _certs.Count == 0) { Assert.Fail("La lista de certificados no debe ser null o no puede está vacía."); return; }
                }


                Certificate cert = _certs.Single(x => x.certid == _certId);

                foreach (var doc in DataForTests.Documents)//.Where(x => x.SignType == SignatureType.PADES).Take(1)) 
                {
                    var b64file = Convert.ToBase64String(File.ReadAllBytes(doc.Path));

                    JObject jObj = null;

                    switch (doc.SignType)
                    {
                        case SignatureType.PADES:

                            jObj = JObject.Parse(_client.Sign(token: _token,
                                                              signatureType: "pades",
                                                              certid: cert.certid,
                                                              certpin: _certPin,
                                                              profile: "enhanced",
                                                              extensions: "lt",
                                                              parameters: DataForTests.ParametersPades,
                                                              document: b64file));

                            Assert.AreEqual((string)jObj["error"]["message"], "OK", $"Error cod. {jObj["error"]["code"]} en la firma del documento: {jObj["error"]["message"]} ");

                            break;

                        case SignatureType.CADES:

                            jObj = JObject.Parse(_client.Sign(token: _token,
                                                               signatureType: "cades",
                                                               certid: cert.certid,
                                                               certpin: _certPin,
                                                               profile: "t",
                                                               extensions: "lt",
                                                               parameters: DataForTests.ParametersCades,
                                                               document: b64file));

                            Assert.AreEqual((string)jObj["error"]["message"], "OK", $"Error cod. {jObj["error"]["code"]} en la firma del documento: {jObj["error"]["message"]} ");

                            break;

                        case SignatureType.XADES:

                            jObj = JObject.Parse(_client.Sign(token: _token,
                                                               signatureType: "xades",
                                                               certid: cert.certid,
                                                               certpin: _certPin,
                                                               profile: "bes",
                                                               extensions: "lt",
                                                               parameters: DataForTests.ParametersCades,
                                                               document: b64file));

                            Assert.AreEqual((string)jObj["error"]["message"], "OK", $"Error cod. {jObj["error"]["code"]} en la firma del documento: {jObj["error"]["message"]} ");

                            break;
                    }

                    if (bool.Parse(ConfigurationManager.AppSettings["savesigneddocs"])) SaveDoc(jObj, doc);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"Se lanzó una excepción inesperada: {ex.Message}");
            }
        }


        [TestMethod]
        public async Task SignDocs()
        {
            try 
            { 
                if (_certs == null || _certs.Count == 0) 
                {
                    await GetCertificates();
                    if (_certs == null || _certs.Count == 0) { Assert.Fail("La lista de certificados no debe ser null o no puede está vacía."); return; }
                }


                Certificate cert = _certs.Single(x => x.certid == _certId);

                foreach (var doc in DataForTests.Documents) //.Where(x => x.SignType == SignatureType.XADES).Take(1)) 
                {
                    var file = File.ReadAllBytes(doc.Path);

                    JObject jObj = null;

                    switch (doc.SignType) 
                    {
                        case SignatureType.PADES:

                            jObj = await _client.SignAsync(token: _token,
                                                           type: doc.SignType,
                                                           certid: cert.certid,
                                                           certpin: _certPin,
                                                           profile: ProfilePades.ENHANCED,
                                                           extensions: "lt",
                                                           parameters: _client.CastThePadesParams(DataForTests.ParametersPades),
                                                           document: file);

                            Assert.AreEqual((string)jObj["error"]["message"], "OK", $"Error cod. {jObj["error"]["code"]} en la firma del documento: {jObj["error"]["message"]} ");

                            break;

                        case SignatureType.CADES:

                            jObj = await _client.SignAsync(token: _token,
                                                           type: doc.SignType,
                                                           certid: cert.certid,
                                                           certpin: _certPin,
                                                           profile: ProfileCades.T,
                                                           extensions: "lt",
                                                           parameters: _client.CastTheCadesParams(DataForTests.ParametersCades),
                                                           document: file);

                            Assert.AreEqual((string)jObj["error"]["message"], "OK", $"Error cod. {jObj["error"]["code"]} en la firma del documento: {jObj["error"]["message"]} ");

                            break;

                        case SignatureType.XADES:

                            jObj = await _client.SignAsync(token: _token,
                                                           type: doc.SignType,
                                                           certid: cert.certid,
                                                           certpin: _certPin,
                                                           profile: ProfileXades.BES,
                                                           extensions: "lt",
                                                           parameters: _client.CastTheXadesParams(DataForTests.ParametersCades),
                                                           document: file);

                            Assert.AreEqual((string)jObj["error"]["message"], "OK", $"Error cod. {jObj["error"]["code"]} en la firma del documento: {jObj["error"]["message"]} ");

                            break;
                    }

                    if( bool.Parse(ConfigurationManager.AppSettings["savesigneddocs"]) ) SaveDoc(jObj, doc);
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"Se lanzó una excepción inesperada: {ex.Message}");
            }
        }







        [TestMethod]
        public void VerifyDocs_Compatible() 
        {
            try
            {
                if (string.IsNullOrEmpty(_token))
                {
                    GetToken_Compatible();
                    if (string.IsNullOrEmpty(_token)) { Assert.Fail("El AccessToken no debe ser null ni cadena vacía."); return; }
                }

                var docs = LoadDocs().Where(x => x.SignType == SignatureType.XADES);

                foreach (var doc in docs)
                {
                    bool bol = _client.Verify(_token, doc.SignType.ToString(), null, doc.B64File);

                    Assert.IsTrue(bol, $"Error en la verificación del documento.");
                }

                Assert.IsTrue(true);

            }
            catch (Exception ex)
            {
                Assert.Fail($"Se lanzó una excepción inesperada: {ex.Message}");
            }
        }


        [TestMethod]
        public async Task VerifyDocs()
        {
            try
            {
                if (string.IsNullOrEmpty(_token))
                {
                    await GetToken();
                    if (string.IsNullOrEmpty(_token)) { Assert.Fail("El AccessToken no debe ser null ni cadena vacía."); return; }
                }

                var docs = LoadDocs();

                foreach (var doc in docs) 
                {
                    var jObj = await _client.VerifyAsync(_token, doc.SignType, doc.File, null);

                    Assert.AreEqual((string)jObj["error"]["message"], "OK", $"Error cod. {jObj["error"]["code"]} en la verificación del documento: {jObj["error"]["message"]} ");
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($"Se lanzó una excepción inesperada: {ex.Message}");
            }
        }



        #region privathe methods

        private Dictionary<string, Uri> GetEndpoints()
        {
            var endpointsSection = (NameValueCollection)ConfigurationManager.GetSection("endpoints");

            var endpoints = endpointsSection.AllKeys
                .ToDictionary(key => key, key => new Uri(endpointsSection[key], UriKind.Relative));

            return endpoints;
        }

        private void SaveDoc(JObject jObj, Document document) 
        {
            try
            {
                byte[] pdfBytes = Convert.FromBase64String(jObj["data"]?.ToString());

                string ext = "";

                switch (document.SignType) 
                {
                    case SignatureType.PADES: ext = "pdf"; break;
                    case SignatureType.XADES: ext = "xml"; break;
                    case SignatureType.CADES: ext = "p7m"; break;
                }

                string filepath = 
                    Path.Combine(ConfigurationManager.AppSettings["ouputdirectory"], $"{Path.GetFileNameWithoutExtension(document.Name)}.{ext}")
                    .Replace('/', Path.DirectorySeparatorChar)
                    .Replace('\\', Path.DirectorySeparatorChar);

                Directory.CreateDirectory(Path.GetDirectoryName(filepath));
                File.WriteAllBytes(filepath, pdfBytes);
            }
            catch { }
        }

        private List<Document> LoadDocs()
        {
            List<Document> documentos = new List<Document>();

            var files = Directory.GetFiles(ConfigurationManager.AppSettings["ouputdirectory"], "*.*", SearchOption.AllDirectories);

            foreach (string file in files)//.Where(x => x.EndsWith(".pdf")).Take(1))
            {
                SignatureType type = SignatureType.CADES;

                switch (Path.GetExtension(file).ToLowerInvariant()) 
                {
                    case ".pdf": type = SignatureType.PADES; break;
                    case ".xml": type = SignatureType.XADES; break;
                    default: type = SignatureType.CADES; break;
                }

                var doc = new Document(Path.GetFileName(file), type, File.ReadAllBytes(file));

                documentos.Add(doc);
            }

            return documentos;
        }

        #endregion privathe methods

    }
}
