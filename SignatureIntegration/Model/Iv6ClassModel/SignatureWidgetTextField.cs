
namespace SignatureIntegration.Model.Iv6ClassModel
{
    public class SignatureWidgetTextField
    {
        /// <summary>
        /// Field type, possible values : 
        ///     'freetext' (free text), 
        ///     'subjectcn' (recovered from the certificate), 
        ///     'organization' (recovered from the certificate), 
        ///     'organizationunit' (recovered from the certificate), 
        ///     'title' (recovered from the certificate), 
        ///     'signerserialnumber' (recovered from the certificate), 
        ///     'issuercn' (recovered from the certificate), 
        ///     'signingtime' (recovered from signature), 
        ///     'reason' (parameter 'cause'), 
        ///     'location' (parameter 'location')
        /// </summary>
        public string fieldtype { get; set; }

        /// <summary>
        /// text value
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// field label
        /// </summary>
        public string label { get; set; }

        /// <summary>
        /// font axis x size
        /// </summary>
        public int? fontsizex { get; set; }

        /// <summary>
        /// font axis y size
        /// </summary>
        public int? fontsizey { get; set; }

        /// <summary>
        /// Axis x offset from origin(0 left, top and bottom, Width/2  right)
        /// </summary>
        public int? xoffset { get; set; }

        /// <summary>
        /// Axis y offset from origin(0 left, right and bottom, Height/2 top)
        /// </summary>
        public int? yoffset { get; set; }

        /// <summary>
        /// Pdf viewer font name, possible values : 
        ///     'Helvetica',
        ///     'Helvetica-Bold',
        ///     'Helvetica-Oblique', 
        ///     'Helvetica-BoldOblique',
        ///     'Times-Roman', 
        ///     'Times-Bold', 
        ///     'Times-Italic', 
        ///     'Times-BoldItalic', 
        ///     'Courier', 
        ///     'Courier-Bold', 
        ///     'Courier-Oblique', 
        ///     'Courier-BoldOblique', 
        ///     'Symbol', 
        ///     'ZapfDingbats' . 
        /// The fonts are not embedded in the document
        /// </summary>
        public string fontresourcename { get; set; }
        
        /// <summary>
        /// string with pdf graphic operator, example : "1.0 0.0 0.0 rg\r" (changes text line to red)
        /// </summary>
        public string customdata { get; set; }
        
    }
}
