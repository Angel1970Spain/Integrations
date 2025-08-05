

namespace SignatureIntegration.Model.Iv6ClassModel
{
    internal class SignatureTextArea
    {
        /// <summary>
        /// Position inside the signature box, possible values : top, bottom, right, left. Leave empty or'custom' value to occupy the entire surface of the box
        /// </summary>
        internal string position { get; set; }

        /// <summary>
        /// Text area lines
        /// </summary>
        internal SignatureWidgetTextField[] signaturewidgettextfields { get; set; }
    }
}
