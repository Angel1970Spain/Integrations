
using Newtonsoft.Json.Linq;
using SignatureIntegration.Model.Enums;
using SignatureIntegration.Model.Iv6ClassModel;
using System;
using System.Collections.Generic;

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
        List<Certificate> GetCertificates(string userid, string orgaid, string token);


        /// <summary>
        /// Convierte un string plano en un modelo SignPadesParams
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        SignPadesParams CastThePadesParams(string parameters);


        /// <summary>
        /// Convierte un string plano en un modelo SignCadesParams
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        SignCadesParams CastTheCadesParams(string parameters);


        /// <summary>
        /// Convierte un string plano en un modelo SignXadesParams
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        SignXadesParams CastTheXadesParams(string parameters);


        /// <summary>
        /// Firmado de documentos. Sobrecarga compatible con la antigua dll
        /// </summary>
        /// <param name="token"></param>
        /// <param name="signatureType"></param>
        /// <param name="certid"></param>
        /// <param name="certpin"></param>
        /// <param name="profile"></param>
        /// <param name="extensions"></param>
        /// <param name="parameters"></param>
        /// <param name="document"></param>
        /// <param name="hashAlgType"></param>
        /// <param name="envelop"></param>
        /// <param name="detachedsignature"></param>
        /// <returns>Json con el retorno de la api</returns>
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


        /// <summary>
        /// Firmado de documentos.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="type"></param>
        /// <param name="certid"></param>
        /// <param name="certpin"></param>
        /// <param name="profile"></param>
        /// <param name="extensions"></param>
        /// <param name="document"></param>
        /// <param name="parameters"></param>
        /// <param name="hashAlgType"></param>
        /// <param name="envelop"></param>
        /// <param name="detachedsignature"></param>
        /// <returns>JObject con el retorno de la api</returns>
        JObject Sign
        (
            string token,
            SignatureType type,
            string certid,
            string certpin,
            object profile,
            string extensions,
            byte[] document,
            object parameters,
            HashAlgType hashAlgType = HashAlgType.SHA256,
            string envelop = "",
            string detachedsignature = ""
        );


        bool Verify
        (
            string token, 
            string signatureType,
            string parameters,
            string document,
            string documentpassword = null,
            string detachedsignature = null,
            ExternalReferences[] refdata = null
        );

        JObject Verify
        (
            string token,
            SignatureType type,
            byte[] document,
            string options = null,
            string documentpassword = null,
            string detachedsignature = null,
            ExternalReferences[] refdata = null
        );

    }
}
