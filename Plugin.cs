using Godot;

namespace LoggingPlugin
{
#if TOOLS
    [Tool]
    public class Plugin : EditorPlugin
    {
        public override void _EnterTree()
        {
            AddCustomType("NodeTracer", "Node", (Script)GD.Load("res://addons/debug/nodetracer/NodeTracer.cs"), (Texture)GD.Load("res://addons/debug/assets/icons/signal.svg"));
        }

        public override void _ExitTree()
        {
            RemoveCustomType("NodeTracer");
        }
    }
#endif
}
