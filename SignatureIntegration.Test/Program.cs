using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SignatureIntegration.External;
using SignatureIntegration.External.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;

namespace SignatureIntegration.Test
{
	static class Program
	{
		static void Main(string[] args)
		{
            var uri = new Uri(ConfigurationManager.AppSettings["ApiUrl"]);
            var endpoints = GetEndpoints();
            var orgaid = ConfigurationManager.AppSettings["orgaid"];
            var login = ConfigurationManager.AppSettings["login"];
            var password = ConfigurationManager.AppSettings["pass"];
            var module = ConfigurationManager.AppSettings["module"];
            var certPin = ConfigurationManager.AppSettings["certpin"];
            var certId = ConfigurationManager.AppSettings["certid"];
            var outpdir = ConfigurationManager.AppSettings["ouputdirectory"];

            ISignatureClient client = new SignatureClient(uri, endpoints);

            Console.WriteLine($"Uri: {uri.AbsoluteUri}");
            Console.WriteLine();

            endpoints.ToList().ForEach(x => Console.WriteLine($"Endpoint: {x.Value}"));
            Console.WriteLine();

            ////Initialize
            ////var client = new SignatureClient("https://dev.ivsign.net/Keyman/rest/V5"); var certselected = 7; //V10
            //var client = new SignatureClient("https://firmadev1.ivnosys.net/Keyman/rest/V5"); var certselected = 16; //v9

            ////User token
            //var token = client.GetToken("!ORGA", "!USER", "!PASSWORD", "pass", "","test");

            ////XML of certificates
            //var certificates = client.GetCertificates(token);

            ////example xml serialization/deserialization
            //var xmlDeserialized = certificates.Deserialize<List<Certificate>>();

            ////Select certificate
            //Certificate cert = null;

            //if (xmlDeserialized.Count > 0) {
            //	//Obtain one (per example id 0)
            //	cert = xmlDeserialized[certselected];

            try
            {
                var token = client.GetToken(orgaid, login, password, "pass", null, module);

                Console.WriteLine($"Token: {token}");
                Console.WriteLine();

                var certstr = client.GetCertificates(token);

                var cert = (client as ISignatureClientV6).DeserializeCertificates(certstr).Single(x => x.certid == certId);

                Console.WriteLine($"Certificado: {JsonConvert.SerializeObject(cert)}");
                Console.WriteLine();


                //obtain file
                var filepades = Convert.ToBase64String(File.ReadAllBytes(@"pades.pdf"));
                var filepades40 = Convert.ToBase64String(File.ReadAllBytes(@"pades40.pdf"));
                var filepades60 = Convert.ToBase64String(File.ReadAllBytes(@"pades60.pdf"));
                var filexades = Convert.ToBase64String(File.ReadAllBytes(@"xades.xml"));
                var filecades = Convert.ToBase64String(File.ReadAllBytes(@"cades.txt"));

                Console.WriteLine("Cargados todos los archivos.");
                Console.WriteLine();


                //Sign

                goto JMP;
            JMP:

                JObject jObj = null;

                Console.WriteLine("Firmando signpades:");
                var signpades = client.Sign(token, "pades", cert.certid, certPin, "enhanced", "lt", "cause=test;autopos=true;autosize=true;hidetext=false;policy=policyidentifier=2.16.724.1.3.1.1.2.1.9,policydigest=G7roucf600+f03r/o0bAOQ6WAs0=,policydigestalgorithm=sha1,policiidentifieraddqualifier=true,policyqualifieruri=https://sede.060.gob.es/politica_de_firma_anexo_1.pdf", filepades);
                jObj = JObject.Parse(signpades);
                var sign_filepades = jObj["data"].ToString();
                jObj["data"] = "[Documento firmado en Base64]";
                Console.WriteLine(jObj.ToString());
                Console.WriteLine();

                Console.WriteLine("Firmando signpades40:");
                var signpades40 = client.Sign(token, "pades", cert.certid, certPin, "enhanced", "lt", "cause=test;autopos=true;autosize=true;hidetext=false;policy=policyidentifier=2.16.724.1.3.1.1.2.1.9,policydigest=G7roucf600+f03r/o0bAOQ6WAs0=,policydigestalgorithm=sha1,policiidentifieraddqualifier=true,policyqualifieruri=https://sede.060.gob.es/politica_de_firma_anexo_1.pdf", filepades40);
                jObj = JObject.Parse(signpades40);
                var sign_filepades40 = jObj["data"].ToString();
                jObj["data"] = "[Documento firmado en Base64]";
                Console.WriteLine(jObj.ToString());
                Console.WriteLine();

                Console.WriteLine("Firmando signpades60:");
                var signpades60 = client.Sign(token, "pades", cert.certid, certPin, "enhanced", "lt", "cause=test;autopos=true;autosize=true;hidetext=false;policy=policyidentifier=2.16.724.1.3.1.1.2.1.9,policydigest=G7roucf600+f03r/o0bAOQ6WAs0=,policydigestalgorithm=sha1,policiidentifieraddqualifier=true,policyqualifieruri=https://sede.060.gob.es/politica_de_firma_anexo_1.pdf", filepades60);
                jObj = JObject.Parse(signpades60);
                var sign_filepades60 = jObj["data"].ToString();
                jObj["data"] = "[Documento firmado en Base64]";
                Console.WriteLine(jObj.ToString());
                Console.WriteLine();

                Console.WriteLine("Firmando signxades:");
                var signxades = client.Sign(token, "xades", cert.certid, certPin, "bes", "", "signerrole=test;policy=policydescription=testxades", filexades, envelop: "enveloped");
                jObj = JObject.Parse(signxades);
                var sign_filexades = jObj["data"].ToString();
                jObj["data"] = "[Documento firmado en Base64]";
                Console.WriteLine(jObj.ToString());
                Console.WriteLine();

                Console.WriteLine("Firmando signcades:");
                //var signcades = client.Sign(token, "cades", cert.certid, certPin, "bes", "", "policy=policydescription=testcades", filecades, envelop: "enveloping");
                var signcades = client.Sign(token, "cades", cert.certid, certPin, "t", "lt", "", filecades);
                jObj = JObject.Parse(signcades);
                var sign_filecades = jObj["data"].ToString();
                jObj["data"] = "[Documento firmado en Base64]";
                Console.WriteLine(jObj.ToString());
                Console.WriteLine();


                if (bool.Parse(ConfigurationManager.AppSettings["savesigneddocs"]))
                {
                    //save signed file
                    Directory.CreateDirectory(outpdir);

                    File.WriteAllBytes(Path.Combine(outpdir, "example_pades_signed.pdf"), Convert.FromBase64String(sign_filepades));
                    File.WriteAllBytes(Path.Combine(outpdir, "example_pades_signed_40.pdf"), Convert.FromBase64String(sign_filepades40));
                    File.WriteAllBytes(Path.Combine(outpdir, "example_pades_signed_60.pdf"), Convert.FromBase64String(sign_filepades60));
                    File.WriteAllBytes(Path.Combine(outpdir, "example_xades_signed.xml"), Convert.FromBase64String(sign_filexades));
                    File.WriteAllBytes(Path.Combine(outpdir, "example_cades_signed.p7m"), Convert.FromBase64String(sign_filecades));

                    Console.WriteLine("Guardados todos los archivos firmados.");
                    Console.WriteLine();
                }


                //verify file

                Console.WriteLine("Verificando pades:");
                var verifypades = client.Verify(token, "pades", "", sign_filepades);
                Console.WriteLine($"Resultado: {verifypades}");
                Console.WriteLine();

                Console.WriteLine("Verificando pades40:");
                var verifypades40 = client.Verify(token, "pades", "", sign_filepades40);
                Console.WriteLine($"Resultado: {verifypades40}");
                Console.WriteLine();

                Console.WriteLine("Verificando pades60:");
                var verifypades60 = client.Verify(token, "pades", "", sign_filepades60);
                Console.WriteLine($"Resultado: {verifypades60}");
                Console.WriteLine();

                Console.WriteLine("Verificando xades:");
                var verifyxades = client.Verify(token, "xades", null, sign_filexades);
                Console.WriteLine($"Resultado: {verifyxades}");
                Console.WriteLine();

                Console.WriteLine("Verificando cades:");
                var verifycades = client.Verify(token, "cades", null, sign_filecades);
                Console.WriteLine($"Resultado: {verifycades}");
                Console.WriteLine();


                //	Console.WriteLine($"signpades='example_pades_signed.pdf'");
                //	Console.WriteLine($"verifypades={verifypades}");
                //	Console.WriteLine($"signpades='example_pades_signed_40.pdf'");
                //	Console.WriteLine($"verifypades={verifypades40}");
                //	Console.WriteLine($"signpades='example_pades_signed_60.pdf'");
                //	Console.WriteLine($"verifypades={verifypades60}");
                //	//Console.WriteLine($"signpades='example_xades_signed.xml'");
                //	//Console.WriteLine($"verifyxades={verifyxades}");
                //	//Console.WriteLine($"signpades='example_cades_signed.pdf'");
                //	//Console.WriteLine($"verifycades={verifycades}");

            }
            catch (Exception e) {

            	Console.WriteLine(e.Message);
            }

            Console.WriteLine("Finalizado. Pulse cualquier tecla.");
            Console.ReadKey(true);
        }

        static Dictionary<string, Uri> GetEndpoints()
        {
            var endpointsSection = (NameValueCollection)ConfigurationManager.GetSection("endpoints");

            var endpoints = endpointsSection.AllKeys
                .ToDictionary(key => key, key => new Uri(endpointsSection[key], UriKind.Relative));

            return endpoints;
        }

    }
}
