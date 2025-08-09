
using SignatureIntegration.Model.Iv6ClassModel;

namespace SignatureIntegration.Model
{
    public class SignatureCades
    {
        public SignatureCades() { }

        /// <summary>
        /// * Requerido
        /// </summary>
        public Cert cert { get; set; }

        /// <summary>
        /// * Requerido
        /// PDF document to sign
        /// </summary>
        public byte[] document { get; set; }

        /// <summary>
        /// Signature in detached mode
        /// </summary>
        public byte[] signdata { get; set; }

        /// <summary>
        /// * Requerido
        ///	Signature profile: 'basic' or 'enhanced'
        /// </summary>
        public string profile { get; set; }

        /// <summary>
        /// Signature options, for example: 
        ///     T include TimeStamp into the signature, 
        ///     EPES include signature policy o LTV re stamp the signature
        /// </summary>
        public string extension { get; set; }

        /// <summary>
        /// Hash algorithm: 'SHA1', 'SHA256', 'SHA512' or 'MD5', SHA1 by default
        /// </summary>
        public string hashalgorithm { get; set; }

        /// <summary>
        /// Signature format: 
        ///     'enveloped'  = The signature includes the original document, 
        ///     'enveloping' = A new XML document is generated with the original document on one of its nodes
        /// </summary>
        public string envelop { get; set; }

        /// <summary>
        /// Kind of operation to perform: sign, cosign, upgrade, append...
        /// </summary>
        public string operation { get; set; }

        /// <summary>
        /// IvSign signature complementary CAdES parameters
        /// </summary>
        public SignCadesParams parameters { get; set; }

        /// <summary>
        /// Signature extra information
        /// </summary>
        public string[][] extradata { get; set; }

        /// <summary>
        /// IvSign caller object
        /// </summary>
        public Caller caller { get; set; }
    }
}
