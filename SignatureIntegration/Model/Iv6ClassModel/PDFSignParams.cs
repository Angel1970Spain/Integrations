
namespace SignatureIntegration.Model.Iv6ClassModel
{
    public class PDFSignParams
    {
        public PDFSignParams() { }

        /// <summary>
        /// PDF document password
        /// </summary>
        public string pwd { get; set; }

        /// <summary>
        /// Visible signature enabled/disabled flag
        /// </summary>
        public bool? signvisible { get; set; }

        /// <summary>
        /// *** DEPRECATED ***, use PdfSignWidgetProps
        /// Visible signature background image properties.
        /// </summary>
        public PdfSignBackground signbackgroundconfig { get; set; }

        /// <summary>
        /// Visible signature box configuration
        /// </summary>
        public PdfSignWidgetProps widgetprops { get; set; }

        /// <summary>
        /// SignatureField name Acrofield
        /// </summary>
        public string signfieldname { get; set; }
    }
}
