

namespace SignatureIntegration.Model.Iv6ClassModel
{
    internal class PdfSignBackground
    {
        /// <summary>
        /// Image in bytes(ONLY JPG, PNG, GIF, BMP FORMAT)
        /// </summary>
        byte[] signback { get; set; }

        /// <summary>
        /// Alpha threshold for the binary mask(ONLY image formats with alpha channel png, bmp, gif ) values between 0 y 254
        /// </summary>
        internal int? maskalphathreshold { get; set; }

        /// <summary>
        /// Background image auto stretch enabled/disabled flag
        /// </summary>
        internal bool? signbackautostretch { get; set; }

        /// <summary>
        /// Axis X auto stretch
        /// </summary>
        internal int? strechx { get; set; }

        /// <summary>
        /// Axis Y auto stretch
        /// </summary>
        internal int? strechy { get; set; }

        /// <summary>
        /// JPG image transparency mask
        /// </summary>
        internal TransparencyMask transparencymask { get; set; }

        /// <summary>
        /// Position inside the signature box, possible values : top, bottom, right, left.Leave empty or'custom' value to occupy the entire surface of the box
        /// </summary>
        internal string position { get; set; }

        /// <summary>
        /// Axis x offset from origin(0 left, top and bottom, Width/2  right)
        /// </summary>
        internal int? xoffset { get; set; }

        /// <summary>
        /// Axis y offset from origin(0 left, right and bottom, Height/2 top)
        /// </summary>
        internal int? yoffset { get; set; }

        /// <summary>
        /// Padding between signature box and image.
        /// </summary>
        internal int? padding { get; set; }
    }
}
