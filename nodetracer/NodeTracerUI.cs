using Godot;
using Godot.Collections;

public class NodeTracerUI : ColorRect
{
    private Dictionary<string, TreeItem> _TreeItems = new Dictionary<string, TreeItem>();
    private Tree _Tree;

    public static NodeTracerUI CreateInstance() {
        var scene = (PackedScene)GD.Load("res://addons/debug/nodetracer/NodeTracerUI.tscn");
        return scene.Instance<NodeTracerUI>();
    }

    public override void _Ready()
    {
        _Tree = GetNode<Tree>("Margin/Tree");
        // Create root
        _Tree.CreateItem();
    }

    public void UpdateUsingTracer(NodeTracer tracer) {
        _Tree.GetRoot().SetText(0, tracer.Title);

        foreach (var kv in tracer.Parameters) {
            _TraceParameter(kv.Key, kv.Value);
        }
    }

    private void _CreateParameter(string name, string value) {
        var root = _Tree.GetRoot();
        var item = _Tree.CreateItem(root);
        _TreeItems[name] = item;

        item.SetText(0, name);
        item.SetText(1, value);
    }

    private void _TraceParameter(string name, string value) {
        if (_TreeItems.ContainsKey(name)) {
            _TreeItems[name].SetText(1, value);
        } else {
            _CreateParameter(name, value);
        }
    }
}
