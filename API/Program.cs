using API.Request;
using API.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API
{
    internal class Program
    {
        internal static async Task RouteRequest(HttpListenerResponse response, HttpListenerRequest request)
        {
            var path = request.Url.AbsolutePath;
            var method = request.HttpMethod;

            if(path.StartsWith("/api/track"))
            {
                if (method == "GET")
                {
                    Logger.Log($"Получен {method} запрос по адресу {path}", consoleColor: ConsoleColor.DarkGray);
                    await TrackRequest.HandleGetTrack(response, request);
                }
            }
        }

        private static async Task StartServer()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8080/api/");
            listener.Start();
            Logger.Log("Сервер успешно запушен по IP http://localhost:8080/api/", consoleColor: ConsoleColor.DarkGray);

            while (true)
            {
                var context = await listener.GetContextAsync();
                await RouteRequest(context.Response, context.Request);
            }
        }

        static void Main(string[] args)
        {
            Task.Run(() => StartServer()).GetAwaiter().GetResult();
        }
    }
}
