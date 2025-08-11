using SignatureIntegration.Model.Enums;
using System;

namespace UnitTestProjectForIntegrations.Model
{
    internal class Document
    {
        internal Document(string name, SignatureType signtype) 
        {
            this.Name = name;
            this.SignType = signtype;
            this.Path = "";
            this.File = null;
            this.B64File = null;    
        }

        internal Document(string name, SignatureType signtype, byte[] file)
        {
            this.Name = name;
            this.SignType = signtype;
            this.Path = "";
            this.File = file;
            this.B64File = Convert.ToBase64String(file);
        }

        internal string Name { get; set; }

        internal SignatureType SignType { get; set; }

        internal string Path { get; set; }

        internal byte[] File { get; set; }

        internal string B64File { get; set; }
    }
}
