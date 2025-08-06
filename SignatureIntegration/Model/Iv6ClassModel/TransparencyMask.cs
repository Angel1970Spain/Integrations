
namespace SignatureIntegration.Model.Iv6ClassModel
{
    public class TransparencyMask
    {
        public TransparencyMask() { }

        /// <summary>
        /// Red channel
        /// </summary>
        public int? red { get; set; }

        /// <summary>
        /// Red tolerance
        /// </summary>
        public int? redtolerance { get; set; }

        /// <summary>
        /// Green channel
        /// </summary>
        public int? green   { get; set; }

        /// <summary>
        /// Green tolerance
        /// </summary>
        public int? greentolerance  { get; set; }

        /// <summary>
        /// Blue channel
        /// </summary>
        public int? blue    { get; set; }

        /// <summary>
        /// Blue tolerance
        /// </summary>
        public int? bluetolerance   { get; set; }

        /// <summary>
        /// Image tolerance
        /// </summary>
        public int? tolerance   { get; set; }
    }
}
