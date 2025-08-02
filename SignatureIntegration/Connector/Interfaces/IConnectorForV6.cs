using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SignatureIntegration.Connector
{
    internal interface IConnectorForV6
    {
        /// <summary>
        /// Ejecuta un metodo Post asíncrono con un Body JSON y opcionalmente un AccessToken contra el endopoint pasado
        /// Devuelve un JObject con la respuesta o un throw
        /// </summary>
        /// <param name="endopoint">Uri</param>
        /// <param name="body">JObject</param>
        /// <param name="credentials">NetworkCredential opcional</param>
        /// <param name="token">opcional</param>
        /// <returns>JObject con la respuesta</returns>
        Task<JObject> PostAsync(Uri endpoint, JObject body, NetworkCredential credentials = null, string token = "");
    }
}
