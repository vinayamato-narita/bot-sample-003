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
            await context.PostAsync($"���{�̓s�s�̓V�C�\��(��������3����)�𒲂ׂ�A�V�C�\��Bot�ł��B");
            context.Done<object>(null);
        }

        [LuisIntent("GetWeather")]
        private async Task GetWeatherAsync(IDialogContext context, LuisResult result)
        {
            var selectedDay = "";
            var cityName = "";

            // LUIS �̔��茋�ʂ��� Entity ���擾 | Get Entities from LUIS result
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
                await context.PostAsync($"�S�����i�T�C�A������Ȃ������ł��B���{�̓s�s�������ĂˁB");
                context.Done<object>(null);
            }
            else
            {
                //�@�ԓ����b�Z�[�W��Post | Post message
                await context.PostAsync(cityName);
                context.Done<object>(null);

            }
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // �ԓ����b�Z�[�W���쐬 | Create return message to user
            var message = context.MakeMessage();

            //�@�ԓ����b�Z�[�W��Post | Post message
            await context.PostAsync(message);
            context.Wait(MessageReceivedAsync);
        }
    }
}