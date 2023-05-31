using CompassApi;
using CompassDiscordBot.Data;
using Discord;
using Discord.WebSocket;

namespace CompassDiscordBot.Commands.CommandExecutors; 

public class GetProfileCommand : ICommandExecutionHandler {
    public async Task Execute(SocketSlashCommand cmd, DiscordSocketClient client) {
        if (!cmd.User.IsUserLoggedIn(out CompassLoginState? state)) {
            await cmd.RespondWithEmbedAsync("Error", "You are not logged in. Please use /login to log in.", ResponseType.Error);
            return;
        }
        
        bool? hide = cmd.GetArgument<bool>("hide");
        hide ??= true;

        CompassClient compass = new(state!);
        CompassUser? profile = await compass.GetUserProfile();
        if (profile == null!) {
            await cmd.RespondWithEmbedAsync("Error", "An error occurred while fetching your profile. Code: Null profile", ResponseType.Error);
            return;
        }

        string desc = $"Email: {profile.Email}\n" +
                      $"House: {profile.House}\n" +
                      $"Homegroup: {profile.HomeGroup}\n" +
                      $"Year: {profile.YearLevel}\n" +
                      $"Student Code: {profile.StudentCode}\n" +
                      $"School Website: {profile.SchoolWebsite}\n" +
                      $"Preferred First Name: {profile.PreferredFirstName}\n" +
                      $"Preferred Last Name: {profile.PreferredLastName}\n";
        EmbedBuilder embedBuilder = new EmbedBuilder().WithTitle(profile.FullName).WithDescription(desc)
            .WithColor(Color.Blue).WithImageUrl(profile.SquarePhotoUrl);
        await cmd.RespondAsync(embed: embedBuilder.Build(), ephemeral: hide.Value);
    }
}