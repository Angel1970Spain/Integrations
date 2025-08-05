
namespace SignatureIntegration.Model.Iv6ClassModel
{
    internal class PdfSignWidgetProps
    {
        /// <summary>
        /// Visible signature box auto position enabled/disabled flag
        /// </summary>
        internal bool? autopos { get; set; }

        /// <summary>
        /// Visible signature box axis X position
        /// </summary>
        internal int? offsetx { get; set; }

        /// <summary>
        /// Visible signature box axis Y position
        /// </summary>
        internal int? offsety { get; set; }

        /// <summary>
        /// Visible signature size auto stretch enabled/disabled flag
        /// </summary>
        internal bool? autosize { get; set; }

        /// <summary>
        /// Visible signature width size
        /// </summary>
        internal int? width { get; set; }

        /// <summary>
        /// Visible signature height size
        /// </summary>
        internal int? height { get; set; }

        /// <summary>
        /// Visible signature rotation degrees
        /// </summary>
        internal int? rotate { get; set; }

        /// <summary>
        /// Specifies on what pages the visible signature is shown, option list, separated by coma: 
        /// all = all the pages, first = first page, last = last page, x = specific page, y-z = page range, 
        /// examples: 'first,last,3,5,10-20,32-50'
        /// </summary>
        internal string showonpages { get; set; }

        /// <summary>
        /// Certificate data box enabled/disabled flag
        /// </summary>
        internal bool? hidetext { get; set; }

        /// <summary>
        /// Certificate data box heather font size
        /// </summary>
        internal float? sizeheader;

        /// <summary>
        /// Certificate data box date font size
        /// </summary>
        internal float? sizedatetime;

        /// <summary>
        /// Certificate data box section heather font size
        /// </summary>
        internal float? sizetitlesection;

        /// <summary>
        /// Certificate data box content font size
        /// </summary>
        internal float? sizetextsection;

        /// <summary>
        /// Signature box page offset
        /// </summary>
        internal int? widgetpageoffset { get; set; }

        /// <summary>
        /// Caption singer field
        /// </summary>
        internal string captionsigner;

        /// <summary>
        /// Caption singer information field
        /// </summary>
        internal string captionsignerinfo;

        /// <summary>
        /// Caption algorithm field
        /// </summary>
        internal string captionalgorithm;

        /// <summary>
        /// Caption header field
        /// </summary>
        internal string captionheader;

        /// <summary>
        ///  Defines a list of text lines(fields)
        /// </summary>
        internal SignatureTextArea[] signaturetextarea {get;set;}

        /// <summary>
        /// Visible signature background image properties
        /// </summary>
        internal PdfSignBackground signatureimage { get; set; }
    }
}
