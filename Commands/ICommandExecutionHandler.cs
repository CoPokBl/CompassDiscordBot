using Discord.WebSocket;

namespace CompassDiscordBot.Commands; 

public interface ICommandExecutionHandler {
    public Task Execute(SocketSlashCommand cmd, DiscordSocketClient client);
}