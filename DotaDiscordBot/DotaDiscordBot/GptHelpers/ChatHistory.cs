using OpenAI.GPT3.ObjectModels.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotaBot.GptHelpers
{
    public static class ChatHistory
    {
        public static List<ChatMessage> ChatMessagesHistory { get; set; } = new List<ChatMessage>();
    }
}
