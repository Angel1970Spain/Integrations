namespace SignatureIntegration.InternalLogic.Interfaces
{
    internal interface ICrypto
    {
        string Encode(string userId);

        string Decode(string userId, string cipherText);
    }
}
