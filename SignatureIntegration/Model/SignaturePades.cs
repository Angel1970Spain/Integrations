using _Cert = SignatureIntegration.Model.Iv6ClassModel.Cert;
using _Caller = SignatureIntegration.Model.Iv6ClassModel.Caller;
using _SignPadesParams = SignatureIntegration.Model.Iv6ClassModel.SignPadesParams;

namespace SignatureIntegration.Model
{
    public class SignaturePades
    {
        public SignaturePades() { }

        /// <summary>
        /// * Requerido
        /// </summary>
        public _Cert cert { get; set; }

        /// <summary>
        /// * Requerido
        /// PDF document to sign
        /// </summary>
        public byte[] document { get; set; }

        /// <summary>
        /// Signature in detached mode
        /// </summary>
        public byte[] asyncdata { get; set; }

        /// <summary>
        /// * Requerido
        ///	Signature profile: 'basic' or 'enhanced'
        /// </summary>
        public string profile { get; set; }

        /// <summary>
        /// Hash algorithm: 'SHA1', 'SHA256', 'SHA512' or 'MD5', SHA1 by default
        /// </summary>
        public string hashalgorithm { get; set; }

        /// <summary>
        /// Signature extensions, separated by coma: 
        ///     't'        = Include TimeStamp into the signature, 
        ///     'timestamp'= Add a TimeStamp to the signature (Long Term Validation), 
        ///     'epes'     = Include signature policy, 
        ///     'biometry' = Include biometric data, 
        ///     'revinfo'  = Include certificate revocation information
        /// </summary>
        public string extension { get; set; }

        /// <summary>
        /// Kind of operation to perform: sign, cosign, upgrade, append...
        /// </summary>
        public string operation { get; set; }

        /// <summary>
        /// Signature extra information
        /// </summary>
        public string[][] extradata { get; set; }

        /// <summary>
        /// IvSign signature complementary PAdES parameters
        /// </summary>
        public _SignPadesParams parameters { get; set; }

        /// <summary>
        /// IvSign caller object
        /// </summary>
        public _Caller caller { get; set; }

    }
}
