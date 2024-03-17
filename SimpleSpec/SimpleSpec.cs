using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Commands.Targeting;

namespace SimpleSpec;

public class SimpleSpec : BasePlugin
{
    public override string ModuleName => "SimpleSpec";
    public override string ModuleAuthor => "ji";
    public override string ModuleDescription =>
        "implements a simple css_spec command that allows players to quickly switch to a player they wish to spectate";
    public override string ModuleVersion => "build1";
    public override void Load(bool hotReload) {}
    
    
    private static TargetResult? GetTarget(CommandInfo command)
    {
        var matches = command.GetArgTargetResult(1);
        if (command.GetArg(1).StartsWith("@")) return null;
        if (!matches.Any())
        {
            command.ReplyToCommand($"Specified target {command.GetArg(1)} not found.");
        }
        if (matches.Count() == 1) return matches;
        command.ReplyToCommand(($"Multiple matches for specified target \"{command.GetArg(1)}\"."));
        return null;
    }

    [ConsoleCommand("css_spec", "css_spec <id or name>")]
    [CommandHelper(minArgs: 1, usage: "[name or ID]", whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void OnSpecCommand(CCSPlayerController? player, CommandInfo commandInfo)
    {
        var target = GetTarget(commandInfo);
        var playerTarget = target.Players.First();
        if (player.IsValid != true || player.PawnIsAlive != false) return;
        commandInfo.ReplyToCommand($"Now spectating: {playerTarget.PlayerName}.");
        player.ExecuteClientCommand($"spec_player #{playerTarget.UserId}");
    }
}