
namespace SignatureIntegration.Model.Iv6ClassModel
{
    public class SignCadesParams
    {
        public SignCadesParams() { }

        /// <summary>
        /// IvSign time stamp server information object list
        /// </summary>
        public TimeStampServerInfo[] tstampservers { get; set; }

        /// <summary>
        /// IvSign signature policy object
        /// </summary>
        public SignPolicy policy { get; set; }

        /// <summary>
        /// bool to include complete certificate chain in the signature certificates field, default false
        /// </summary>
        public bool? includewholechain { get; set; }

        /// <summary>
        /// Bool to add signing certificate hash v2 to the signed attributes
        /// </summary>
        public bool? addsigningcertificatev2 { get; set; }
    }
}
