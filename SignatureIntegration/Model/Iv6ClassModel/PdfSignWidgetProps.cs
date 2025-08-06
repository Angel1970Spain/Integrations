
namespace SignatureIntegration.Model.Iv6ClassModel
{
    public class PdfSignWidgetProps
    {
        public PdfSignWidgetProps() { }

        /// <summary>
        /// Visible signature box auto position enabled/disabled flag
        /// </summary>
        public bool? autopos { get; set; }

        /// <summary>
        /// Visible signature box axis X position
        /// </summary>
        public int? offsetx { get; set; }

        /// <summary>
        /// Visible signature box axis Y position
        /// </summary>
        public int? offsety { get; set; }

        /// <summary>
        /// Visible signature size auto stretch enabled/disabled flag
        /// </summary>
        public bool? autosize { get; set; }

        /// <summary>
        /// Visible signature width size
        /// </summary>
        public int? width { get; set; }

        /// <summary>
        /// Visible signature height size
        /// </summary>
        public int? height { get; set; }

        /// <summary>
        /// Visible signature rotation degrees
        /// </summary>
        public int? rotate { get; set; }

        /// <summary>
        /// Specifies on what pages the visible signature is shown, option list, separated by coma: 
        /// all = all the pages, first = first page, last = last page, x = specific page, y-z = page range, 
        /// examples: 'first,last,3,5,10-20,32-50'
        /// </summary>
        public string showonpages { get; set; }

        /// <summary>
        /// Certificate data box enabled/disabled flag
        /// </summary>
        public bool? hidetext { get; set; }

        /// <summary>
        /// Certificate data box heather font size
        /// </summary>
        public float? sizeheader;

        /// <summary>
        /// Certificate data box date font size
        /// </summary>
        public float? sizedatetime;

        /// <summary>
        /// Certificate data box section heather font size
        /// </summary>
        public float? sizetitlesection;

        /// <summary>
        /// Certificate data box content font size
        /// </summary>
        public float? sizetextsection;

        /// <summary>
        /// Signature box page offset
        /// </summary>
        public int? widgetpageoffset { get; set; }

        /// <summary>
        /// Caption singer field
        /// </summary>
        public string captionsigner;

        /// <summary>
        /// Caption singer information field
        /// </summary>
        public string captionsignerinfo;

        /// <summary>
        /// Caption algorithm field
        /// </summary>
        public string captionalgorithm;

        /// <summary>
        /// Caption header field
        /// </summary>
        public string captionheader;

        /// <summary>
        ///  Defines a list of text lines(fields)
        /// </summary>
        public SignatureTextArea[] signaturetextarea {get;set;}

        /// <summary>
        /// Visible signature background image properties
        /// </summary>
        public PdfSignBackground signatureimage { get; set; }
    }
}
