
namespace SignatureIntegration.Model
{
    public class VerifyCades
    {
        /// <summary>
        /// Verification options
        /// </summary>
        public string options { get; set; }

        /// <summary>
        /// * Requerido
        /// Generic signed document to verify
        /// </summary>
        public byte[] document { get; set; }

        /// <summary>
        /// Signature to verify
        /// </summary>
        public string detachedsignature { get; set; }

    }
}
