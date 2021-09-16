using Godot;
using Godot.Collections;
using System.Text;

public class LogPanel: Control {
    private RichTextLabel _Content;
    private Array<LogMessage> _Messages;

    public override void _Ready()
    {
        _Content = GetNode<RichTextLabel>("Container/ColorRect/Margin/Content");
    }

    public override void _Process(float delta)
    {
        _Content.BbcodeText = _GenerateContent(Logging.Messages);
    }

    public string _GenerateContent(Array<LogMessage> messages) {
        var sb = new StringBuilder();
        foreach (LogMessage message in messages) {
            sb.Append(
                $"[b][color=yellow][{message.Level.ToString().ToUpper()}][/color][/b] "
            );

            if (message.PeerId > 0) {
                sb.Append($"[b][color=blue]*{message.PeerId}*[/color][/b] ");
            }

            sb.Append(
                $"[b][color=green][{message.LoggerName}][/color][b] "
            );

            sb.AppendLine(message.Message);
        }
        return sb.ToString();
    }
}