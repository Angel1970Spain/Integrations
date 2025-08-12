using SignatureIntegration.Model.Iv6ClassModel;
using System;
using System.Runtime.InteropServices;

namespace SignatureIntegration.External.Interfaces
{
    [ComVisible(true), Guid("2F2D893D-2FD8-4D6B-B422-FE41E5F7DC9B")]
    public interface ISignatureClient
    {
        [Obsolete("Utilice el constructor de la clase en su lugar.")]
        void Initialize(string url);


        string GetToken(string orgaid, string login, string password, string method, string modulekey, string module = "signatureintegration");


        string GetCertificates(string token);


        string Sign(string token, string signatureType, string certid, string certpin, string profile, string extensions, string parameters, string document, string hashalgorithm = "SHA256", string envelop = "", string detachedsignature = "");


        bool Verify(string token, string signatureType, string parameters, string document, string documentpassword = "", string detachedsignature = "", ReferenceData[] refdata = null);
    }
}
