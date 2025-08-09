
namespace SignatureIntegration.Model.Iv6ClassModel
{
    public class SignLocation
    {
        public SignLocation()
        { }

        /// <summary>
        /// Signature location city
        /// </summary>
        public string locality { get; set; }

        /// <summary>
        /// Signature location region
        /// </summary>
        public string province { get; set; }

        /// <summary>
        /// Signature location city postal code
        /// </summary>
        public string postalcode { get; set; }

        /// <summary>
        /// Signature location country
        /// </summary>
        public string country { get; set; }
    }
}
