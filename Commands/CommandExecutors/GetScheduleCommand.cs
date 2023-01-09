using CompassApi;
using CompassDiscordBot.Data;
using Discord;
using Discord.WebSocket;
using GeneralPurposeLib;

namespace CompassDiscordBot.Commands.CommandExecutors; 

public class GetScheduleCommand : ICommandExecutionHandler {
    public async Task Execute(SocketSlashCommand cmd, DiscordSocketClient client) {
        if (!cmd.User.IsUserLoggedIn(out CompassLoginState? state)) {
            await cmd.RespondWithEmbedAsync("Error", "You are not logged in. Please use /login to log in.", ResponseType.Error);
            return;
        }
        
        bool? hide = cmd.GetArgument<bool>("hide");
        bool? simple = cmd.GetArgument<bool>("simple");
        string? dateString = cmd.GetArgument<string>("date");  // yyyy-MM-dd
        hide ??= true;
        simple ??= false;
        
        DateTime date = dateString == null ? DateTime.Today : DateTime.Parse(dateString);

        CompassClient compass = new(state!);

        await cmd.DeferAsync(hide.Value);  // Let the bot think
        
        CompassClass[] classes = (await compass.GetClasses(!simple.Value, date.AddDays(-1), date, 20)).ToArray();
        Logger.Debug($"Got {classes.Length} classes");
        string desc = "";

        bool exempt = false;
        string week = "";
        foreach (CompassClass c in classes.Reverse()) {
            string name = c.Name == "Unknown" || string.IsNullOrWhiteSpace(c.Name) ? c.Id : c.Name;
            bool isModifier = false;
            switch (c.ActivityType) {
                
                case CompassClassType.Normal:
                    desc += $"{name} ({c.StartTime.ToDiscordTimestamp(TimestampTagStyles.LongTime)} - {c.EndTime.ToDiscordTimestamp(TimestampTagStyles.LongTime)})\n" +
                            $"Teacher: {c.Teacher}\n" +
                            $"Room: {c.Room}";
                    break;
                
                case CompassClassType.Exempt:
                    exempt = true;
                    isModifier = true;
                    break;
                
                case CompassClassType.WeekNumber:
                    isModifier = true;
                    week = name;
                    break;
                
                case CompassClassType.DueTask:

                case CompassClassType.Event:

                case CompassClassType.Unknown:
                default:
                    desc += $"{name} ({c.StartTime.ToDiscordTimestamp(TimestampTagStyles.LongTime)} - {c.EndTime.ToDiscordTimestamp(TimestampTagStyles.LongTime)})";
                    break;
                
            }
            desc += isModifier ? "" : "\n\n";
        }

        if (exempt) {
            desc = "You are exempt from classes today.\n\n" + desc;
        }

        if (classes.Length == 0) {
            desc = "No classes found.";
        }

        if (desc.Length > 4096) {
            desc = desc[..4093] + "...";
        }

        EmbedBuilder embedBuilder = new EmbedBuilder().WithTitle($"{date.ToLongDateString()} {(week == "" ? "" : $"({week})")}").WithDescription(desc)
            .WithColor(Color.Blue);
        await cmd.ModifyOriginalResponseAsync(props => {
            props.Embed = embedBuilder.Build();
        });
    }
}