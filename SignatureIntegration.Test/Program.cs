using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SignatureIntegration.Test
{
	static class Program
	{
		static void Main(string[] args)
		{

			//Initialize
			//var client = new SignatureClient("https://dev.ivsign.net/Keyman/rest/V5"); var certselected = 7; //V10
			var client = new SignatureClient("https://firmadev1.ivnosys.net/Keyman/rest/V5"); var certselected = 16; //v9

			//User token
			var token = client.GetToken("!ORGA", "!USER", "!PASSWORD", "pass", "","test");
			
			//XML of certificates
			var certificates = client.GetCertificates(token);

			//example xml serialization/deserialization
			var xmlDeserialized = certificates.Deserialize<List<Certificate>>();

			//Select certificate
			Certificate cert = null;
			if (xmlDeserialized.Count > 0) {
				//Obtain one (per example id 0)
				cert = xmlDeserialized[certselected];
				
				//obtain file
				var filepades = Convert.ToBase64String(File.ReadAllBytes(@"pades.pdf"));
				var filepades40 = Convert.ToBase64String(File.ReadAllBytes(@"pades40.pdf"));
				var filepades60 = Convert.ToBase64String(File.ReadAllBytes(@"pades60.pdf"));
				var filexades = Convert.ToBase64String(File.ReadAllBytes(@"xades.xml"));
				var filecades = Convert.ToBase64String(File.ReadAllBytes(@"cades.pdf"));

				//sign
				try {

					//Sign
					var signpades = client.Sign(token, "pades", cert.certid, "123123q", "enhanced", "lt", "cause=test;autopos=true;autosize=true;hidetext=false;policy=policyidentifier=2.16.724.1.3.1.1.2.1.9,policydigest=G7roucf600+f03r/o0bAOQ6WAs0=,policydigestalgorithm=sha1,policiidentifieraddqualifier=true,policyqualifieruri=https://sede.060.gob.es/politica_de_firma_anexo_1.pdf", filepades);
					var signpades40 = client.Sign(token, "pades", cert.certid, "123123q", "enhanced", "lt", "cause=test;autopos=true;autosize=true;hidetext=false;policy=policyidentifier=2.16.724.1.3.1.1.2.1.9,policydigest=G7roucf600+f03r/o0bAOQ6WAs0=,policydigestalgorithm=sha1,policiidentifieraddqualifier=true,policyqualifieruri=https://sede.060.gob.es/politica_de_firma_anexo_1.pdf", filepades40);
					var signpades60 = client.Sign(token, "pades", cert.certid, "123123q", "enhanced", "lt", "cause=test;autopos=true;autosize=true;hidetext=false;policy=policyidentifier=2.16.724.1.3.1.1.2.1.9,policydigest=G7roucf600+f03r/o0bAOQ6WAs0=,policydigestalgorithm=sha1,policiidentifieraddqualifier=true,policyqualifieruri=https://sede.060.gob.es/politica_de_firma_anexo_1.pdf", filepades60);
					
					
					var signxades = client.Sign(token, "xades", cert.certid, "123123q", "bes", "", "signerrole=test;policy=policydescription=testxades", filexades, envelop: "enveloped");

					//var signcades = client.Sign(token, "cades", cert.certid, "123123q", "bes", "", "policy=policydescription=testcades", filecades, envelop: "enveloping");

					//save signed file
					File.WriteAllBytes(@"example_pades_signed.pdf", Convert.FromBase64String(signpades));
					File.WriteAllBytes(@"example_pades_signed_40.pdf", Convert.FromBase64String(signpades40));
					File.WriteAllBytes(@"example_pades_signed_60.pdf", Convert.FromBase64String(signpades60));
					//File.WriteAllBytes(@"example_xades_signed.xml", Convert.FromBase64String(signxades));
					//File.WriteAllBytes(@"example_cades_signed.pdf", Convert.FromBase64String(signcades));

					//verify file
					var verifypades = client.Verify(token, SignatureTypes.PADES, "", signpades);
					var verifypades40 = client.Verify(token, SignatureTypes.PADES, "", signpades40);
					var verifypades60 = client.Verify(token, SignatureTypes.PADES, "", signpades60);
					//var verifyxades = client.Verify(token, SignatureTypes.XADES, "", signxades);
					//var verifycades = client.Verify(token, SignatureTypes.CADES, "", signcades);

					Console.WriteLine($"signpades='example_pades_signed.pdf'");
					Console.WriteLine($"verifypades={verifypades}");
					Console.WriteLine($"signpades='example_pades_signed_40.pdf'");
					Console.WriteLine($"verifypades={verifypades40}");
					Console.WriteLine($"signpades='example_pades_signed_60.pdf'");
					Console.WriteLine($"verifypades={verifypades60}");
					//Console.WriteLine($"signpades='example_xades_signed.xml'");
					//Console.WriteLine($"verifyxades={verifyxades}");
					//Console.WriteLine($"signpades='example_cades_signed.pdf'");
					//Console.WriteLine($"verifycades={verifycades}");
					
				} catch (Exception e) {

					Console.WriteLine(e.Message);
				}
				
				Console.ReadKey(true);
			} 
			else {
				Console.WriteLine("Empty certificate list...");
			}
		}

		
	}

}
