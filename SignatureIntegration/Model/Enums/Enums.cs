
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

    public enum Profile
    {
        BASIC, ENHANCED
    }

    public enum SygnatureType
    {
        PADES, XADES, CADES
    }

}
