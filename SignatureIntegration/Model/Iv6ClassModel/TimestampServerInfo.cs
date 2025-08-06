
namespace SignatureIntegration.Model.Iv6ClassModel
{
    public class TimestampServerInfo
    {
        public TimestampServerInfo() { }

        /// <summary>
        /// Server name
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Server Url
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// Server authentication required flag
        /// </summary>
        public bool? httpauth { get; set; }

        /// <summary>
        /// Server authentication user
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// Server authentication password
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// Nonce used on the call to the server flag
        /// </summary>
        public bool? usenonce { get; set; }

        /// <summary>
        /// Server certificate included into the signature flag
        /// </summary>
        public bool? includecertificates { get; set; }

        /// <summary>
        /// Hash algorithm, the server must support it
        /// </summary>
        public string hashalgorithm { get; set; }

        /// <summary>
        /// Time stamp IvSign certificate ID (if applicable)
        /// </summary>
        public string certid { get; set; }

        /// <summary>
        /// Time stamp PFX certificate (if applicable)
        /// </summary>
        public string pfx { get; set; }

        /// <summary>
        /// Certificate/PFX pin
        /// </summary>
        public string pin { get; set; }

    }
}
