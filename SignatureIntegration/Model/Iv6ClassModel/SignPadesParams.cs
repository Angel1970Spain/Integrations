
namespace SignatureIntegration.Model.Iv6ClassModel
{
    public class SignPadesParams
    {
        /// <summary>
        /// Opcional
        /// Signature reason
        /// </summary>
        public string cause { get; set; }

        /// <summary>
        /// IvSign PDF signature parameters object
        /// </summary>
        public PDFSignParams pdfparameters { get; set; }

        /// <summary>
        /// IvSign time stamp server information object, if it is not specified and the signature requires it, the default one will be used
        /// </summary>
        public TimestampServerInfo[] tstampservers { get; set; }

        /// <summary>
        /// IvSign biometric data object
        /// </summary>
        public Biometry biometry { get; set; }

        /// <summary>
        /// IvSign signature policies object
        /// </summary>
        public SignPolicy policy { get; set; }
    }
}
