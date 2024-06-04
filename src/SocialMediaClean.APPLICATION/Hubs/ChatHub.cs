using LinkedFit.DOMAIN.Models.DTOs.Chat;
using Microsoft.AspNetCore.SignalR;

namespace LinkedFit.APPLICATION.Hubs
{
    public class ChatHub: Hub
    {
        public async Task SendMessage(ChatDTO chat)
        {

            await Clients.Group(chat.ChatID).SendAsync("ReceiveMessage", chat);
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
        

        public async Task JoinConversation(string chatID)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatID);
        }
        public async Task LeaveConversation(string chatID)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatID.ToString());
        }
    }

}
