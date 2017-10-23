using System;
using System.Threading.Tasks;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    [LuisModel("f56b650f-83bd-4ddb-b0b4-da8103767d81", "b8a78dd763c44225851f7c47bb1e0ffa")]
    [Serializable]
    public class EchoDialog : LuisDialog<object>
    {
        //public async Task StartAsync(IDialogContext context)
        //{
        //    context.Wait(MessageReceivedAsync);
        //}
        [LuisIntent("")]
        [LuisIntent("None")]
        private async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"日本の都市の天気予報(今日から3日間)を調べる、天気予報Botです。");
            context.Done<object>(null);
        }

        [LuisIntent("GetWeather")]
        private async Task GetWeatherAsync(IDialogContext context, LuisResult result)
        {
            var selectedDay = "";
            var cityName = "";

            // LUIS の判定結果から Entity を取得 | Get Entities from LUIS result
            foreach (var entity in result.Entities)
            {
                if (entity.Type == "place")
                {
                    cityName = entity.Entity.ToString();

                }
                else if (entity.Type.Substring(0, 3) == "day")
                {
                    selectedDay = entity.Type.Substring(5);
                }
            }
            
            if (cityName == "")
            {
                await context.PostAsync($"ゴメンナサイ、分からなかったです。日本の都市名を入れてね。");
                context.Done<object>(null);
            }
            else
            {
                //　返答メッセージをPost | Post message
                await context.PostAsync(cityName);
                context.Done<object>(null);

            }
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // 返答メッセージを作成 | Create return message to user
            var message = context.MakeMessage();

            //　返答メッセージをPost | Post message
            await context.PostAsync(message);
            context.Wait(MessageReceivedAsync);
        }
    }
}