using _Cert = SignatureIntegration.Model.Iv6ClassModel.Cert;
using _Caller = SignatureIntegration.Model.Iv6ClassModel.Caller;
using _SignPadesParams = SignatureIntegration.Model.Iv6ClassModel.SignPadesParams;

namespace SignatureIntegration.Model
{
    internal class SignaturePades
    {
        /// <summary>
        /// * Requerido
        /// </summary>
        internal _Cert cert { get; set; }

        /// <summary>
        /// * Requerido
        /// PDF document to sign
        /// </summary>
        internal byte[] document { get; set; }

        /// <summary>
        /// Signature in detached mode
        /// </summary>
        internal byte[] asyncdata { get; set; }

        /// <summary>
        /// * Requerido
        ///	Signature profile: 'basic' or 'enhanced'
        /// </summary>
        internal string profile { get; set; }

        /// <summary>
        /// Hash algorithm: 'SHA1', 'SHA256', 'SHA512' or 'MD5', SHA1 by default
        /// </summary>
        internal string hashalgorithm { get; set; }

        /// <summary>
        /// Signature extensions, separated by coma: 
        ///     't'        = Include TimeStamp into the signature, 
        ///     'timestamp'= Add a TimeStamp to the signature (Long Term Validation), 
        ///     'epes'     = Include signature policy, 
        ///     'biometry' = Include biometric data, 
        ///     'revinfo'  = Include certificate revocation information
        /// </summary>
        internal string extension { get; set; }

        /// <summary>
        /// Kind of operation to perform: sign, cosign, upgrade, append...
        /// </summary>
        internal string operation { get; set; }

        /// <summary>
        /// Signature extra information
        /// </summary>
        internal string[][] extradata { get; set; }

        /// <summary>
        /// IvSign signature complementary PAdES parameters
        /// </summary>
        internal _SignPadesParams parameters { get; set; }

        /// <summary>
        /// IvSign caller object
        /// </summary>
        internal _Caller caller { get; set; }

    }
}
