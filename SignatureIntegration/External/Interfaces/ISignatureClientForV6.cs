
using SignatureIntegration.Model.Enums;
using SignatureIntegration.Model.Iv6ClassModel;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace SignatureIntegration.External
{
    public interface ISignatureClientForV6
    {
        /// <summary>
        /// Checkeo para la aplicación de test
        /// </summary>
        /// <returns></returns>
        bool CheckTest();

        /// <summary>
        /// Devuelve un el AccesToken
        /// </summary>
        /// <param name="orgaid"></param>
        /// <param name="login"></param>
        /// <param name="pass"></param>
        /// <param name="module"></param>
        /// <param name="authmethod"></param>
        /// <param name="origin"></param>
        /// <param name="modkey"></param>
        /// <param name="modver"></param>
        /// <param name="deviceinfo"></param>
        /// <returns>AccessToken</returns>
        string GetToken
        (
            string orgaid, 
            string login, 
            string pass, 
            string module,
            AuthMethod authmethod = AuthMethod.PASS, 
            string origin = null, 
            string modkey = null, 
            string modver = null, 
            string deviceinfo = null
        );

        /// <summary>
        /// Devuelve el AccessToken(pos. 1) y el RefreshToken(pos. 2)
        /// </summary>
        /// <param name="orgaid"></param>
        /// <param name="login"></param>
        /// <param name="pass"></param>
        /// <param name="module"></param>
        /// <param name="authmethod"></param>
        /// <param name="origin"></param>
        /// <param name="modkey"></param>
        /// <param name="modver"></param>
        /// <param name="deviceinfo"></param>
        /// <returns>Tuple<AccessToken, RefreshToken></returns>
        Tuple<string,string> GetTokens
        (
            string orgaid,
            string login,
            string pass,
            string module,
            AuthMethod authmethod = AuthMethod.PASS,
            string origin = null,
            string modkey = null,
            string modver = null,
            string deviceinfo = null
        );


        /// <summary>
        /// Obtiene una lista de certificados
        /// </summary>
        /// <param name="userid">Pasado a GetToken como "login"</param>
        /// <param name="orgaid"></param>
        /// <param name="token">El token obtenido en GetToken</param>
        /// <returns>Lista de instancias de certificate</returns>
        List<old_Certificate> GetCertificates(string userid, string orgaid, string token);


        /// <summary>
        /// Convierte un string plano en un modelo SignPadesParams
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        SignPadesParams CastTheParams(string parameters);


        string Sign
        (
            string token,
            string signatureType,
            string certid,
            string certpin,
            string profile,
            string extensions,
            string parameters,
            string document,
            string hashAlgType = "SHA256",
            string envelop = "",
            string detachedsignature = ""
        );

        string Sign
        (
            string token,
            SygnatureType type,
            string certid,
            string certpin,
            string profile,
            string extensions,
            SignPadesParams parameters,
            byte[] document,
            HashAlgType hashAlgType = HashAlgType.SHA256,
            string envelop = "",
            string detachedsignature = ""
        );

    }
}
