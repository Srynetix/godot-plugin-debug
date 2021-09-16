using Godot;

public class DebugMenu: CanvasLayer {
    private LogPanel _LogPanel;
    private NodeTracerSystem _NodeTracerSystem;
    private Logging _Logger;

    public DebugMenu() {
        _Logger = Logging.GetLogger("DebugMenu");
    }

    public override void _Ready()
    {
        _LogPanel = GD.Load<PackedScene>("res://addons/debug/logpanel/LogPanel.tscn").Instance<LogPanel>();
        AddChild(_LogPanel);
        _NodeTracerSystem = GD.Load<PackedScene>("res://addons/debug/nodetracer/NodeTracerSystem.tscn").Instance<NodeTracerSystem>();
        AddChild(_NodeTracerSystem);

        // Hide all
        _LogPanel.Visible = false;
        _NodeTracerSystem.Visible = false;
    }

    public override void _Input(InputEvent @event) {
        if (@event is InputEventKey eventKey) {
            if (eventKey.Pressed) {
                if (eventKey.Scancode == (int)KeyList.F2) {
                    _ToggleNodeTracer();
                }

                else if (eventKey.Scancode == (int)KeyList.F3) {
                    _ToggleLogPanel();
                }
            }
        }
    }

    private void _ToggleNodeTracer() {
        _LogPanel.Visible = false;
        _NodeTracerSystem.Visible = !_NodeTracerSystem.Visible;
    }

    private void _ToggleLogPanel() {
        _NodeTracerSystem.Visible = false;
        _LogPanel.Visible = !_LogPanel.Visible;
    }
}