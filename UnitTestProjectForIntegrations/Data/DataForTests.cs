using SignatureIntegration.Model.Enums;
using System.Collections.Generic;
using UnitTestProjectForIntegrations.Model;

namespace UnitTestProjectForIntegrations.Data
{
    internal static class DataForTests
    {
        internal static string ParametersPades = @"cause=test;autopos=true;autosize=true;hidetext=false;policy=policyidentifier=2.16.724.1.3.1.1.2.1.9,policydigest=G7roucf600+f03r/o0bAOQ6WAs0=,policydigestalgorithm=sha1,policiidentifieraddqualifier=true,policyqualifieruri=https://sede.060.gob.es/politica_de_firma_anexo_1.pdf";

        internal static string CheckCastParsPades = @"{""cause"":""test"",""pdfparameters"":{""pwd"":null,""signvisible"":null,""signbackgroundconfig"":null,""widgetprops"":{""sizeheader"":null,""sizedatetime"":null,""sizetitlesection"":null,""sizetextsection"":null,""captionsigner"":null,""captionsignerinfo"":null,""captionalgorithm"":null,""captionheader"":null,""autopos"":true,""offsetx"":null,""offsety"":null,""autosize"":true,""width"":null,""height"":null,""rotate"":null,""showonpages"":null,""hidetext"":false,""widgetpageoffset"":null,""signaturetextarea"":null,""signatureimage"":null},""signfieldname"":null},""tstampservers"":null,""biometry"":null,""policy"":{""policyidentifier"":""2.16.724.1.3.1.1.2.1.9"",""policyidentifieraddqualifier"":null,""policydescription"":null,""policydigest"":""G7roucf600+f03r/o0bAOQ6WAs0="",""policydigestalgorithm"":""sha1"",""policyqualifieruri"":""https://sede.060.gob.es/politica_de_firma_anexo_1.pdf""}}";

        internal static string ParametersCades = @"";

        internal static string CheckCastParsCades = @"{""tstampservers"":null,""policy"":null,""includewholechain"":false,""addsigningcertificatev2"":false}";

        internal static List<Document> Documents = new List<Document>
        {
            new Document("cades.txt", SignatureType.CADES ),
            new Document("pades.pdf", SignatureType.PADES ),
            new Document("pades40.pdf",SignatureType.PADES ),
            new Document("pades60.pdf", SignatureType.PADES ),
            new Document( "xades.xml", SignatureType.XADES )
        };

    }

}

