
using System.Runtime.InteropServices;

namespace SignatureIntegration.Model.Iv6ClassModel
{
    [Guid("75E42C56-87E9-4589-8CBE-75580E68B871"), ComVisible(true), ProgId("SignatureIntegration.referencedata")]
    public class ReferenceData
    {
        public string uri { get; set; }
        public string idprefix { get; set; }
        public string idns { get; set; }
        public string data { get; set; }
        public bool isdigest { get; set; }
        public bool detached { get; set; }
        public string name { get; set; }
        public string mimetype { get; set; }
    }
}
