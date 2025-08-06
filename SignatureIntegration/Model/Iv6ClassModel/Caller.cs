
namespace SignatureIntegration.Model.Iv6ClassModel
{
    public class Caller
    {
        public Caller() { }

        /// <summary>
        /// Integration module
        /// </summary>
        public string app { get; set; }

        /// <summary>
        /// Client host
        /// </summary>
        public string host { get; set; }

        /// <summary>
        /// Client URL location
        /// </summary>
        public string location { get; set; }

        /// <summary>
        /// Client host user
        /// </summary>
        public string remoteuser { get; set; }
    }
}
