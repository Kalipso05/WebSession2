using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.Settings
{
    internal class Response
    {
        internal static async Task SendResponse(HttpListenerResponse response, string content, string contentType = "application/json", HttpStatusCode code = HttpStatusCode.OK)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(content);
            response.ContentLength64 = buffer.Length;
            response.ContentType = contentType;
            response.StatusCode = (int)code;

            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            response.Close();
        }
    }
}
