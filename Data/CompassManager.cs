using CompassApi;
using Discord;

namespace CompassDiscordBot.Data; 

public static class CompassManager {

    public static bool IsUserLoggedIn(this IUser user, out CompassLoginState? state) {
        CompassLoginState? loginState = StorageManager.GetUserLoginState(user.Id.ToString()).Result;
        state = null;
        if (loginState == null) {
            return false;
        }
        // Test if the token is still valid
        CompassClient client = new(loginState);
        try {
            CompassUser _ = client.GetUserProfile().Result;
        }
        catch (Exception) {
            return false;
        }
        state = loginState;
        return true;
    }
    
}