
using SignatureIntegration.Model.Iv6ClassModel;

namespace SignatureIntegration.Model
{
    public class VerifyXades
    {
        /// <summary>
        /// Verification options
        /// </summary>
        public string options { get; set; }

        /// <summary>
        /// * Requerido
        /// XML signed document to verify
        /// </summary>
        public byte[] document { get; set; }

        /// <summary>
        /// Signature to verify
        /// </summary>
        public string detachedsignature { get; set; }

        /// <summary>
        /// No figura en la doc oficial, pero si en la dll antigua como ReferenceData y en el json del swagger.
        /// </summary>
        public ExternalReferences[] ExternalReferences { get; set; }
    }
}
