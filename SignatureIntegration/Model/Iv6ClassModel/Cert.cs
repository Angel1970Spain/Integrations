
namespace SignatureIntegration.Model.Iv6ClassModel
{
    public class Cert
    {
        public Cert() { }

        /// <summary>
        /// IvSign certificate ID
        /// </summary>
        public string certid { get; set; }

        /// <summary>
        /// Certificate access pin
        /// </summary>
        public string pin { get; set; }
    }
}
