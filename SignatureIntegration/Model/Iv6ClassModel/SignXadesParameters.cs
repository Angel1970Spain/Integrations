
namespace SignatureIntegration.Model.Iv6ClassModel
{
    public class SignXadesParameters
    {
        public SignXadesParameters() 
        {}

        /// <summary>
        /// Signer user role
        /// </summary>
        public string signerrole { get; set; }

        /// <summary>
        /// Include or not the whole certificate certificate chain
        /// </summary>
        public bool? includewholechain { get; set; }

        /// <summary>
        /// Include or not certificate public key
        /// </summary>
        public bool? includekeyvalue { get; set; }

        /// <summary>
        /// XAdES signature version
        /// </summary>
        public int? xadesversion { get; set; }

        /// <summary>
        /// Signature location data, for instance, the city where the signature is performed
        /// </summary>
        public SignLocation location { get; set; }

        /// <summary>
        /// IvSign signature policy object
        /// </summary>
        public SignPolicy policy { get; set; }

        /// <summary>
        /// IvSign time stamp server information object list
        /// </summary>
        public TimeStampServerInfo[] tstampservers { get; set; }

        /// <summary>
        /// Internal reference to the original XML document, must start by '#'
        /// </summary>
        public string envreferencetosign { get; set; }

        /// <summary>
        /// Sets the xmldsign destination node element through document xpath search method
        /// </summary>
        public string envsigdestreference { get; set; }

        /// <summary>
        /// Sets the envsigdestreference xpath search method referred nodes namespace and its prefixes list
        /// </summary>
        public string[][] envnamespacelist { get; set; }

        /// <summary>
        /// ID node namespace to sign, for example, wsu:Id
        /// </summary>
        public string envreferencetosignns { get; set; }
    }
}
