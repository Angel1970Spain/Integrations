
namespace SignatureIntegration.Model.Iv6ClassModel
{
    public class Biometry
    {
        /// <summary>
        /// Biometric signature information
        /// </summary>
        public byte[] data { get; set; }

        /// <summary>
        /// Certificate public key
        /// </summary>
        public byte[] cert { get; set; }
    }
}
