
namespace SignatureIntegration.Model.Iv6ClassModel
{
    internal class TransparencyMask
    {
        /// <summary>
        /// Red channel
        /// </summary>
        internal int? red { get; set; }

        /// <summary>
        /// Red tolerance
        /// </summary>
        internal int? redtolerance { get; set; }

        /// <summary>
        /// Green channel
        /// </summary>
        internal int? green   { get; set; }

        /// <summary>
        /// Green tolerance
        /// </summary>
        internal int? greentolerance  { get; set; }

        /// <summary>
        /// Blue channel
        /// </summary>
        internal int? blue    { get; set; }

        /// <summary>
        /// Blue tolerance
        /// </summary>
        internal int? bluetolerance   { get; set; }

        /// <summary>
        /// Image tolerance
        /// </summary>
        internal int? tolerance   { get; set; }
    }
}
