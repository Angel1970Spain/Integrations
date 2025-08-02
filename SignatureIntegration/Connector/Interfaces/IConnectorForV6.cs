using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SignatureIntegration.Connector
{
    internal interface IConnectorForV6
    {

        /// <summary>
        /// Ejecuta un metodo Post asíncrono con un Body JSON y un AccessToken contra el endopoint pasado
        /// Devuelve un JObject con la respuesta o un throw
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="body"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<JObject> PostAsync(Uri endpoint, JObject body, string token);

        /// <summary>
        /// Ejecuta un metodo Post asíncrono con un Body JSON y NetworkCredential contra el endopoint pasado
        /// Devuelve un JObject con la respuesta o un throw
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="body"></param>
        /// <param name="credentials">NetworkCredential</param>
        /// <param name="token">opcional</param>
        /// <returns></returns>
        Task<JObject> PostAsync(Uri endpoint, JObject body, NetworkCredential credentials, string token = "");

    }
}
