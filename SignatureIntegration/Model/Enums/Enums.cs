
namespace SignatureIntegration.Model.Enums
{
    public enum AuthMethod
    {
        PASS, WIN, FEDERATED
    }

    public enum HashAlgType
    {
        SHA1, SHA256, SHA512, MD5
    }

    public enum ProfilePades
    {
        BASIC, ENHANCED
    }

    public enum ProfileCades
    {
       CMS, T, C, X, XL
    }

    public enum ProfileXades
    {
        XMLDSIG, BES, EPES, T, C, XL, A
    }

    public enum SignatureType
    {
        PADES, XADES, CADES
    }

}
