

namespace SignatureIntegration.Model.Iv6ClassModel
{
    public class SignatureTextArea
    {
        public SignatureTextArea() { }

        /// <summary>
        /// Position inside the signature box, possible values : top, bottom, right, left. Leave empty or'custom' value to occupy the entire surface of the box
        /// </summary>
        public string position { get; set; }

        /// <summary>
        /// Text area lines
        /// </summary>
        public SignatureWidgetTextField[] signaturewidgettextfields { get; set; }
    }
}
