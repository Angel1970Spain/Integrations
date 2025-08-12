
using Newtonsoft.Json.Linq;
using SignatureIntegration.External.Interfaces;
using SignatureIntegration.Model;
using SignatureIntegration.Model.Enums;
using SignatureIntegration.Model.Iv6ClassModel;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SignatureIntegration.External
{
    [ComVisible(true)]
    [Guid("8B1E0E52-6B1B-4A43-8E85-99B8746D7889")] 
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface ISignatureClientV6 : ISignatureClient
    {
        /// <summary>
        /// Devuelve el AccesToken
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
        Task<string> GetTokenAsync
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
        /// <param name="token">El token obtenido en GetToken</param>
        /// <param name="userid">Opcional. Pasado a GetToken como "login"</param>
        /// <param name="orgaid">Opcional</param>
        /// <returns>Lista de instancias de certificate</returns>
        Task<List<Certificate>> GetCertificatesAsync(string token, string userid = null, string orgaid = null);

        /// <summary>
        /// Deserializa los certificados de modo texto a List<Certificates>
        /// </summary>
        /// <param name="certificates"></param>
        /// <returns></returns>
        List<Certificate> DeserializeCertificates(string certificates);

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
        Task<JObject> SignAsync
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

        /// <summary>
        /// Verificación de documento firmado. Sobrecarga compatible con la antigua dll
        /// </summary>
        /// <param name="token"></param>
        /// <param name="signatureType"></param>
        /// <param name="parameters"></param>
        /// <param name="document"></param>
        /// <param name="documentpassword"></param>
        /// <param name="detachedsignature"></param>
        /// <param name="refdata"></param>
        /// <returns></returns>
        bool Verify
        (
            string token, 
            string signatureType,
            string parameters,
            string document,
            string documentpassword = null,
            string detachedsignature = null,
            ReferenceData[] refdata = null
        );

        /// <summary>
        /// Verificación de archivo firmado
        /// </summary>
        /// <param name="token"></param>
        /// <param name="type"></param>
        /// <param name="document"></param>
        /// <param name="options"></param>
        /// <param name="documentpassword"></param>
        /// <param name="detachedsignature"></param>
        /// <param name="refdata"></param>
        /// <returns>JObject con el retorno de la api</returns>
        Task<JObject> VerifyAsync
        (
            string token,
            SignatureType type,
            byte[] document,
            string options = null,
            string documentpassword = null,
            string detachedsignature = null,
            ReferenceData[] refdata = null
        );

    }
}
