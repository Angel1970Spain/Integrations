using SignatureIntegration.Model.Iv6ClassModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SignatureIntegration.InternalLogic
{
    internal class SignatureAuxLogic
    {

        public SignPadesParams CastTheParams(string parameters) 
        {
            if (string.IsNullOrWhiteSpace(parameters)) throw new Exception("Parameters cannot be null or empty.");

            var r_pars = new SignPadesParams 
            { 
                pdfparameters = new PDFSignParams 
                { 
                    signbackgroundconfig = new PdfSignBackground(), 
                    widgetprops = new PdfSignWidgetProps() 
                } 
            };

            Dictionary<string, string> dict = parameters
                .Split(';')
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Select(p => p.Split('=')) 
                .Where(p => p.Length == 2)
                .ToDictionary(p => p[0].Trim(), parts => parts[1].Trim());




            return r_pars;
        }
    }
}
