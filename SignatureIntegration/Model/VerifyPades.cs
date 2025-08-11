
namespace SignatureIntegration.Model
{
    public class VerifyPades
    {
        /// <summary>
        /// * Requerido
        /// PDF signed document to verify
        /// </summary>
        public byte[] document { get; set; }

        /// <summary>
        /// Document password
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// Verification options
        /// </summary>
        public string options { get; set; }
    }
}
