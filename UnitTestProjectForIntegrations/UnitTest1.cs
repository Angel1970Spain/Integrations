using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignatureIntegration.External;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;


namespace UnitTestProjectForIntegrations
{
    [TestClass]
    public class UnitTest1
    {
        private ISignatureClientForV6 _client;

        private string _token = "";

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
        public void TestMethod1()
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

                Assert.IsTrue(true);

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

                Assert.IsTrue(true);
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
                    Assert.IsFalse(string.IsNullOrEmpty(_token), "El AccessToken no debe ser null ni cadena vacía.");
                }

                string orgaid = ConfigurationManager.AppSettings["orgaid"];
                string userid = ConfigurationManager.AppSettings["login"];

                var r = _client.GetCertificates(userid, orgaid, _token);

                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Se lanzó una excepción inesperada: {ex.Message}");
            }
        }

    }
}
