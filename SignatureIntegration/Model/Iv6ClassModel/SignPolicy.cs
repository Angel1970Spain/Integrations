
namespace SignatureIntegration.Model.Iv6ClassModel
{
    public class SignPolicy
    {
        /// <summary>
        /// Signature's policy identifier
        /// </summary>
        public string policyidentifier { get; set; }

        /// <summary>
        /// Signature's policy add qualifier to the signature flag
        /// </summary>
        public bool? policyidentifieraddqualifier { get; set; }

        /// <summary>
        /// Signature's policy description
        /// </summary>
        public string policydescription { get; set; }

        /// <summary>
        /// Signature policy digest
        /// </summary>
        public byte[] policydigest { get; set; }

        /// <summary>
        /// Signature policy digest algorithm
        /// </summary>
        public string policydigestalgorithm { get; set; }

        /// <summary>
        /// Signature publication URI
        /// </summary>
        public string policyqualifieruri { get; set; }
    }
}
