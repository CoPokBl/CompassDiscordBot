using Discord.WebSocket;

namespace CompassDiscordBot.EventHandlers; 

public static class ButtonHandler {

    public static async Task Execute(SocketMessageComponent button) {

        switch (button.Data.CustomId) {
            
            case "try_login_again":
                await button.RespondWithModalAsync(ModalHandler.GetLoginModal());
                break;
            
            default:
                throw new Exception("Button not found: " + button.Data.CustomId);
            
        }
        
    }
    
}