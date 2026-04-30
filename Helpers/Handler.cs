using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RestTestsKozlenko.Helpers
{
    internal class Handler
    {
        private static string HandleResponse(string url, HttpMethod method)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                HttpResponseMessage response = null;

                if (method == HttpMethod.Get)
                {
                    response = client.GetAsync(url).Result;
                }
                else if (method == HttpMethod.Post)
                {
                    response = client.PostAsync(url, null).Result;
                }
                else if (method == HttpMethod.Put)
                {
                    response = client.PutAsync(url, null).Result;
                }
                else if (method == HttpMethod.Delete)
                {
                    response = client.DeleteAsync(url).Result;
                }

                string resp = response.StatusCode.ToString();

                var statusCodes = new Dictionary<string, string>
                {
                    {"Continue","100"},
                    {"SwitchingProtocols","101"},
                    {"OK","200"},
                    {"Created","201"},
                    {"Accepted","202"},
                    {"NonAuthoritativeInformation","203"},
                    {"NoContent","204"},
                    {"ResetContent","205"},
                    {"PartialContent","206"},
                    {"MultipleChoices","300"},
                    {"MovedPermanently","301"},
                    {"MovedTemporarily","302"},
                    {"SeeOther", "303"},
                    {"NotModified","304"},
                    {"TemporaryRedirect","307" },
                    {"PermanentRedirect","308"},
                    {"BadRequest","400"},
                    {"Unauthorized","401"},
                    {"Forbidden","403"},
                    {"NotFound","404"},
                    {"MethodNotAllowed","405"},
                    {"NotAcceptable","406"},
                    {"InternalServerError","500"},
                    {"NotImplemented","501"},
                    {"BadGateway","502"},
                    {"ServiceUnavailable","503"},
                    {"GatewayTimeout","504"},
                    {"HttpVersionNotSupported","505" },
                    {"NetworkAuthenticationRequired","511"}
                };

                if (statusCodes.ContainsKey(resp))
                {
                    return statusCodes[resp];
                }

                return "";
            }
        }
    }
}
