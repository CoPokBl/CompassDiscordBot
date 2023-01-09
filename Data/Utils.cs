using Discord;

namespace CompassDiscordBot.Data; 

public static class Utils {
    
    public static TimestampTag ToDiscordTimestamp(this DateTime dateTime, TimestampTagStyles style = TimestampTagStyles.ShortDateTime) {
        return TimestampTag.FromDateTime(dateTime, style);
    }
    
}