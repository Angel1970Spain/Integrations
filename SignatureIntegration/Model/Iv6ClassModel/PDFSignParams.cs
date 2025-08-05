
namespace SignatureIntegration.Model.Iv6ClassModel
{
    internal class PDFSignParams
    {
        /// <summary>
        /// PDF document password
        /// </summary>
        internal string pwd { get; set; }

        /// <summary>
        /// Visible signature enabled/disabled flag
        /// </summary>
        internal bool? signvisible { get; set; }

        /// <summary>
        /// *** DEPRECATED ***, use PdfSignWidgetProps
        /// Visible signature background image properties.
        /// </summary>
        internal PdfSignBackground signbackgroundconfig { get; set; }

        /// <summary>
        /// Visible signature box configuration
        /// </summary>
        internal PdfSignWidgetProps widgetprops { get; set; }

        /// <summary>
        /// SignatureField name Acrofield
        /// </summary>
        internal string signfieldname { get; set; }
    }
}
