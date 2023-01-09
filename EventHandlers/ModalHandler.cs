using CompassApi;
using CompassDiscordBot.Commands;
using CompassDiscordBot.Data;
using Discord;
using Discord.WebSocket;

namespace CompassDiscordBot.EventHandlers; 

public static class ModalHandler {

    public static async Task Submit(SocketModal modal) {
        SocketMessageComponentData[] components = modal.Data.Components.ToArray();

        if (components == null!) {
            throw new Exception("No components found");
        }

        string school = components.GetValue("school");
        string username = components.GetValue("username");
        string password = components.GetValue("password");
        
        // Test Creds
        CompassLoginState loginState;
        try {
            CompassClient client = new(school);
            bool success = await client.Authenticate(username, password);
            if (!success) {
                throw new Exception("Invalid Credentials");
            }
            loginState = client.GetLoginState();
        }
        catch (Exception) {
            // Invalid
            ComponentBuilder builder = new ComponentBuilder().WithButton("Try Again", "try_login_again");
            await modal.RespondWithEmbedAsyncModal("Invalid Credentials", "The information you provided to us did not allow us to authenticate you.", ResponseType.Error, builder);
            return;
        }
        // Save info
        await StorageManager.SetUserLoginState(modal.User.Id.ToString(), loginState);
        await modal.RespondWithEmbedAsyncModal("Success", "You have been authenticated with Compass, you can now execute other commands.", ResponseType.Success);
    }
    
    public static string GetValue(this IEnumerable<SocketMessageComponentData> components, string key) {
        string? readOnlyCollection = components.First(x => x.CustomId == key).Value;
        if (readOnlyCollection == null) {
            throw new Exception("No values found");
        }
        return readOnlyCollection;
    }

    public static Modal GetLoginModal() {
        ModalBuilder builder = new ModalBuilder()
            .WithTitle("Compass Education Login")
            .AddTextInput("School Prefix", "school", TextInputStyle.Short,
                "If you access Compass at botsareepic-sc.compass.education then your prefix is botsareepic-sc", 1, 256)
            .AddTextInput("Username", "username", TextInputStyle.Short, "bot0036", 1, 256)
            .AddTextInput("Password", "password", TextInputStyle.Short, "verysecurepassword123", 1, 256)
            .WithCustomId("login");
        return builder.Build();
    }

}