using Godot;
using Godot.Collections;

public class NodeTracer: Node
{
    [Export]
    public string Title;

    public Dictionary<string, string> Parameters = new Dictionary<string, string>();

    public NodeTracer(): this("") {}
    public NodeTracer(string name) {
        Name = "NodeTracer";
        Title = name;
    }

    public override void _Ready()
    {
        // Use parent name if no name set
        if (Title == "") {
            Title = GetParent().GetPath().ToString();
        }

        AddToGroup("NodeTracer");
    }

    public void TraceParameter(string name, object value) {
        Parameters[name] = ConvertValue(value);
    }

    private string ConvertValue(object value) {
        // Special boolean case
        if (value is bool boolValue) {
            if (boolValue) {
                return "YES";
            } else {
                return "NO";
            }
        }

        if (value is Vector3 vec3) {
            return $"({vec3.x:F2}, {vec3.y:F2}, {vec3.z:F2})";
        }

        else {
            return value.ToString();
        }
    }
}
