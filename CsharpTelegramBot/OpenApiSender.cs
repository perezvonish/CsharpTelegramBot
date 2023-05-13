using CsharpTelegramBot.Config;
using OpenAI_API;

namespace CsharpTelegramBot
{
    public static class OpenApiSender
    {
        public static OpenAI_API.OpenAIAPI api = new(EnvLoader.GetOpenApiKey());

        public static async Task<string> SendOpenApiMessage(string message)
        {
            return await api.Completions.GetCompletion(message);
        }
    }
}
