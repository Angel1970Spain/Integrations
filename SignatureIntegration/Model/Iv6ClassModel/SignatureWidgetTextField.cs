
namespace SignatureIntegration.Model.Iv6ClassModel
{
    internal class SignatureWidgetTextField
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
        internal string fieldtype { get; set; }

        /// <summary>
        /// text value
        /// </summary>
        internal string text { get; set; }

        /// <summary>
        /// field label
        /// </summary>
        internal string label { get; set; }

        /// <summary>
        /// font axis x size
        /// </summary>
        internal int? fontsizex { get; set; }

        /// <summary>
        /// font axis y size
        /// </summary>
        internal int? fontsizey { get; set; }

        /// <summary>
        /// Axis x offset from origin(0 left, top and bottom, Width/2  right)
        /// </summary>
        internal int? xoffset { get; set; }

        /// <summary>
        /// Axis y offset from origin(0 left, right and bottom, Height/2 top)
        /// </summary>
        internal int? yoffset { get; set; }

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
        internal string fontresourcename { get; set; }
        
        /// <summary>
        /// string with pdf graphic operator, example : "1.0 0.0 0.0 rg\r" (changes text line to red)
        /// </summary>
        internal string customdata { get; set; }
        
    }
}
