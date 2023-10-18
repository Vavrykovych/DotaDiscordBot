using Discord.Commands;

using OpenAI.GPT3;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels;
using Discord;
using System.Collections.Generic;
using DotaBot.GptHelpers;

namespace DotaBot.Modules
{
    public class GptCommands : ModuleBase<SocketCommandContext>
    {
        private readonly OpenAIService _openAiService;
        public GptCommands() 
        {
            _openAiService = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = "ApiKey",
            });
        }
        [Command("ai")]
        public async Task SendChatGptMessage(params string[] message)
        {
            var chatMessage = ChatMessage.FromUser(string.Join(' ', message));
            //var chatMessage = new ChatMessage("user", string.Join(' ', message));

            ChatHistory.ChatMessagesHistory.Add(chatMessage);

            var completionResult = await _openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest()
            {
                Messages = ChatHistory.ChatMessagesHistory,
                Model = OpenAI.GPT3.ObjectModels.Models.ChatGpt3_5Turbo,
            });

            if (completionResult.Successful)
            {
                
                foreach (var choice in completionResult.Choices)
                {
                    ChatHistory.ChatMessagesHistory.Add(ChatMessage.FromAssistant(choice.Message.Content));
                    if(choice.Message.Content.Length > 1000)
                    {
                        foreach(var m in SplitString(choice.Message.Content, 1000))
                        {
                            await ReplyAsync($"{Context.User.Mention} {m}");
                        }
                    }
                    else
                    {
                        await ReplyAsync($"{Context.User.Mention} {choice.Message.Content}");
                    }
                }
            }
            else
            {
                if (completionResult.Error == null)
                {
                    await ReplyAsync($"{Context.User.Mention} Не паше");
                }
                else
                {
                    await ReplyAsync($"{Context.User.Mention} Не паше {completionResult.Error.Code}: {completionResult.Error.Message}");
                }
            }
        }

        [Command("ai-clear")]
        public async Task ClearChatHistory()
        {
            ChatHistory.ChatMessagesHistory = new List<ChatMessage>();
            await ReplyAsync($"{Context.User.Mention} Історію розмови очищено");
        }




        private static List<string> SplitString(string input, int chunkSize)
        {
            List<string> chunks = new List<string>();

            for (int i = 0; i < input.Length; i += chunkSize)
            {
                int length = Math.Min(chunkSize, input.Length - i);
                string chunk = input.Substring(i, length);
                chunks.Add(chunk);
            }

            return chunks;
        }

    }
}
