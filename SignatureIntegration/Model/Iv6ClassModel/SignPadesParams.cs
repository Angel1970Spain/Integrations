
namespace SignatureIntegration.Model.Iv6ClassModel
{
    internal class SignPadesParams
    {
        /// <summary>
        /// Opcional
        /// Signature reason
        /// </summary>
        internal string cause { get; set; }

        /// <summary>
        /// IvSign time stamp server information object, if it is not specified and the signature requires it, the default one will be used
        /// </summary>
        internal TimestampServerInfo[] tstampservers { get; set; }
    }
}
