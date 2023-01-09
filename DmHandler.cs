using Discord.WebSocket;

namespace CompassDiscordBot; 

public static class DmHandler {

    public static async void Run(SocketMessage msg) {
        await msg.Channel.SendMessageAsync("Try using my slash commands! Click '/'");
    }
    
}