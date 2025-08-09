using SignatureIntegration.Model.Enums;

namespace UnitTestProjectForIntegrations.Model
{
    internal class Document
    {
        internal Document(string name, SignatureType signtype) 
        {
            this.Name = name;
            this.SignType = signtype;
            this.Path = "";
        }

        internal string Name { get; set; }

        internal SignatureType SignType { get; set; }

        internal string Path { get; set; }
    }
}
