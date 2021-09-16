using Godot;
using Godot.Collections;

public class NodeTracerSystem : MarginContainer
{
    private GridContainer _Grid;
    private Dictionary<NodePath, NodeTracer> _Tracers = new Dictionary<NodePath, NodeTracer>();
    private Dictionary<NodePath, NodeTracerUI> _TracersUI = new Dictionary<NodePath, NodeTracerUI>();
    private Logging _Logger;

    public NodeTracerSystem() {
        _Logger = Logging.GetLogger("NodeTracerSystem");
    }

    public override void _Ready()
    {
        _Grid = GetNode<GridContainer>("Container/MarginContainer2/Grid");
    }

    public override void _Process(float delta)
    {
        foreach (NodeTracer node in GetTree().GetNodesInGroup("NodeTracer")) {
            var nodePath = node.GetPath();
            if (!_Tracers.ContainsKey(nodePath)) {
                _Logger.DebugM("_Process", $"Registering NodeTracer '{node.Title}'.");
                _Tracers.Add(nodePath, node);
                _CreateTracerUI(nodePath, node);
            } else {
                _UpdateTracerUI(node, _TracersUI[nodePath]);
            }
        }
    }

    private void _CreateTracerUI(NodePath path, NodeTracer tracer) {
        var ui = NodeTracerUI.CreateInstance();
        _TracersUI.Add(path, ui);
        _Grid.AddChild(ui);

        _UpdateTracerUI(tracer, ui);
    }

    private void _UpdateTracerUI(NodeTracer tracer, NodeTracerUI ui) {
        ui.UpdateUsingTracer(tracer);
    }
}
