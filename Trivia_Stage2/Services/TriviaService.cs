using Trivia_Stage2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Trivia_Stage2.Services
{
    public class TriviaService
    {
        Player LoggedPlayer;
        HttpClient httpClient;//אובייקט לשליחת בקשות וקבלת תשובות מהשרת

        JsonSerializerOptions options;//פרמטרים שישמשו אותנו להגדרות הjson

        const string URL = $@"https://qsc714b9-7128.euw.devtunnels.ms/TriviaApi/";//כתובת השרת

        public TriviaService()
        {
            //http client
            httpClient = new HttpClient();

            //options when doing serialization/deserialization
            options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
        }
        public async Task<bool> LogPlayer(string email, string password)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true };
                string jsonString = JsonSerializer.Serialize(new SentPlayer() { email=email, password=password}, options);
                HttpResponseMessage message = await httpClient.PostAsync(URL + "/Login", new StringContent(jsonString, Encoding.UTF8, "application/json"));
                if (message.IsSuccessStatusCode)
                {
                    ReturnedPlayer p = JsonSerializer.Deserialize<ReturnedPlayer>(await message.Content.ReadAsStringAsync());
                    LoggedPlayer = new Player()
                    {
                        PlayerId = p.playerId,
                        Email = p.playerEmail,
                        Password = p.playerPassword,
                        PlayerName = p.playerName,
                        Points = p.playerScore,
                        RankId = p.playerRank.rankId
                    };
                }
                return LoggedPlayer != null;
            }
            catch
            {
                return false;
            }
        }
    }


}
