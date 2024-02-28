using API.Settings;
using DataCenter.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.Request
{
    internal class TrackRequest
    {
        internal static async Task HandleGetTrack(HttpListenerResponse response, HttpListenerRequest request)
        {
            try
            {
                using (var db = new dbModel())
                {
                    var trackingInfo = await db.SecurityAccessLog.ToListAsync();

                    var json = JsonConvert.SerializeObject(trackingInfo);

                    await Response.SendResponse(response, json);
                }
            }
            catch (Exception e)
            {
                Logger.Log("При GET запросе произошла ошибка сервера", HttpStatusCode.InternalServerError, ConsoleColor.DarkRed);
                await Response.SendResponse(response, "Произошла внутренняя ошибка сервера", code: HttpStatusCode.InternalServerError);
            }
        }
    }
}
