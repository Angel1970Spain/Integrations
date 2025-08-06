using SignatureIntegration.Model.Iv6ClassModel;
using System;
using System.Linq;
using System.Text;

namespace SignatureIntegration.InternalLogic
{
    internal class SignatureAuxLogic
    {
        internal SignatureAuxLogic() 
        { }

        internal SignPadesParams CastTheParams(string parameters) 
        {
            if (string.IsNullOrWhiteSpace(parameters)) throw new Exception("Parameters cannot be null or empty.");

            var spp = new SignPadesParams 
            { 
                pdfparameters = new PDFSignParams 
                { 
                    signbackgroundconfig = new PdfSignBackground(), 
                    widgetprops = new PdfSignWidgetProps() 
                } 
            };

            foreach (var par in parameters.Split(';')) 
            {
                var keyval = par.Split( new char[] { '=' }, 2);
                var key = keyval[0].ToLower();
                var val = keyval.Count() > 0 ? keyval[1] : "";

                switch (key)
                {
                    case "cause": spp.cause = val; break;
                    case "pwd": spp.pdfparameters.pwd = val; break;
                    case "signback": spp.pdfparameters.signbackgroundconfig.signback = Encoding.UTF8.GetBytes(val); break;
                    case "signbackautostretch": spp.pdfparameters.signbackgroundconfig.signbackautostretch = Convert.ToBoolean(val); break;
                    case "signvisible": spp.pdfparameters.signvisible = Convert.ToBoolean(val); break;
                    case "autopos": spp.pdfparameters.widgetprops.autopos = Convert.ToBoolean(val); break;
                    case "autosize": spp.pdfparameters.widgetprops.autosize = Convert.ToBoolean(val); break;
                    case "hidetext": spp.pdfparameters.widgetprops.hidetext = Convert.ToBoolean(val); break;
                    case "offsetx": spp.pdfparameters.widgetprops.offsetx = Convert.ToInt32(val); break;
                    case "offsety": spp.pdfparameters.widgetprops.offsety = Convert.ToInt32(val); break;
                    case "showonpages": spp.pdfparameters.widgetprops.showonpages = val; break;
                    case "widgetpageoffset": spp.pdfparameters.widgetprops.widgetpageoffset = Convert.ToInt32(val); break;
                    case "rotate": spp.pdfparameters.widgetprops.rotate = Convert.ToInt32(val); break;
                    case "width": spp.pdfparameters.widgetprops.width = Convert.ToInt32(val); break;
                    case "sizeheader": spp.pdfparameters.widgetprops.sizeheader = Convert.ToInt32(val); break;
                    case "sizedatetime": spp.pdfparameters.widgetprops.sizedatetime = Convert.ToInt32(val); break;
                    case "sizetitlesection": spp.pdfparameters.widgetprops.sizetitlesection = Convert.ToSingle(val); break;
                    case "sizetextsection": spp.pdfparameters.widgetprops.sizetextsection = Convert.ToSingle(val); break;
                    case "captionsigner": spp.pdfparameters.widgetprops.captionsigner = val; break;
                    case "captionsignerinfo": spp.pdfparameters.widgetprops.captionsignerinfo = val; break;
                    case "captionalgorithm": spp.pdfparameters.widgetprops.captionalgorithm = val; break;
                    case "captionheader": spp.pdfparameters.widgetprops.captionheader = val; break;
                    case "tstampserver": spp.tstampservers = GetTimestampServerInfoPars(val); break;
                    case "policy": spp.policy = GetSignPolicyPars(val); break;
                    default: break;
                }
            }

            return spp;
        }

        private TimestampServerInfo[] GetTimestampServerInfoPars(string tsa)
        {
            TimestampServerInfo tsaserver = new TimestampServerInfo();

            foreach (var par in tsa.Split(','))
            {
                var keyval = par.Split(new char[] { '=' }, 2);
                var key = keyval[0].ToLower();
                var val = keyval.Count() > 0 ? keyval[1] : "";

                switch (key)
                {
                    case "name": tsaserver.name = val; break;
                    case "url": tsaserver.url = val; break;
                    case "httpauth": tsaserver.httpauth = Convert.ToBoolean(val); break;
                    case "username": tsaserver.username = val; break;
                    case "password": tsaserver.password = val; break;
                    case "usenonce": tsaserver.usenonce = Convert.ToBoolean(val); break;
                    case "includecertificates": tsaserver.includecertificates = Convert.ToBoolean(val); break;
                    case "hashalgorithm": tsaserver.hashalgorithm = val; break;
                    case "certid": tsaserver.certid = val; break;
                    case "pfx": tsaserver.pfx = val; break;
                    case "pin": tsaserver.pin = val; break;
                    default: break;
                }
            }

            return new TimestampServerInfo[] { tsaserver };
        }

        private SignPolicy GetSignPolicyPars(string valor)
        {
            var policy = new SignPolicy();

            foreach (var par in valor.Split(','))
            {
                var keyval = par.Split(new char[] { '=' }, 2);
                var key = keyval[0].ToLower();
                var val = keyval.Count() > 0 ? keyval[1] : "";

                switch (key)
                {
                    case "policyidentifier": policy.policyidentifier = val; break;
                    case "policyidentifieraddqualifier": policy.policyidentifieraddqualifier = Convert.ToBoolean(val); break;
                    case "policydescription": policy.policydescription = val; break;
                    case "policydigest": policy.policydigest = Convert.FromBase64String(val); break;
                    case "policydigestalgorithm": policy.policydigestalgorithm = val; break;
                    case "policyqualifieruri": policy.policyqualifieruri = val; break;
                    default: break;
                }
            }

            return policy;
        }
    }
}
