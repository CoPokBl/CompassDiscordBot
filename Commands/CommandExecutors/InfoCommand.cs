using Discord.WebSocket;

namespace CompassDiscordBot.Commands.CommandExecutors; 

public class InfoCommand : ICommandExecutionHandler {
    
    public async Task Execute(SocketSlashCommand cmd, DiscordSocketClient client) {
        await cmd.RespondAsync($"I am Compass Discord Bot {Program.Version}");
    }
    
}