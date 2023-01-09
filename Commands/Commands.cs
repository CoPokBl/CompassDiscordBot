using CompassDiscordBot.Commands.CommandExecutors;
using Discord;

namespace CompassDiscordBot.Commands; 

public static class Commands {

    public static readonly SlashCommand[] SlashCommands = {
        
        new (
            "info", 
            "Get information about the bot", 
            Array.Empty<SlashCommandArgument>(),
            new InfoCommand(),
            null,
            false),
        
        new (
            "login", 
            "Login to Compass Education", 
            Array.Empty<SlashCommandArgument>(),
            new LoginCommand(),
            null,
            false),
        
        new (
            "get-profile", 
            "Get your Compass user profile", 
            new [] {
                new SlashCommandArgument(
                    "hide", 
                    "If set to true, only you will be able to see the profile. Defaults to false.", 
                    false, 
                    ApplicationCommandOptionType.Boolean)
            },
            new GetProfileCommand(),
            null,
            false),
        
        new (
            "get-schedule", 
            "Get your classes between the start and end dates. Max 20 classes.", 
            new [] {
                new SlashCommandArgument(
                    "hide", 
                    "If set to true, only you will be able to see the profile. Defaults to false.", 
                    false, 
                    ApplicationCommandOptionType.Boolean),
                new SlashCommandArgument(
                    "date", 
                    "The date to get classes at. Defaults to today. Format: yyyy-MM-dd", 
                    false, 
                    ApplicationCommandOptionType.String),
                new SlashCommandArgument(
                    "simple", 
                    "Whether or not to get less information for more speed. Defaults to false.", 
                    false, 
                    ApplicationCommandOptionType.Boolean)
            },
            new GetScheduleCommand(),
            null,
            false),
        
    };

}