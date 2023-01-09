using CompassDiscordBot.EventHandlers;
using Discord;
using Discord.WebSocket;

namespace CompassDiscordBot.Commands.CommandExecutors; 

public class LoginCommand : ICommandExecutionHandler {
    public async Task Execute(SocketSlashCommand cmd, DiscordSocketClient client) {
        await cmd.RespondWithModalAsync(ModalHandler.GetLoginModal());
    }
}