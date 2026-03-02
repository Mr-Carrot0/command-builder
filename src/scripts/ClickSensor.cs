using Godot;
using System;

public partial class ClickSensor : Node
{
    [Export] Camera3D Cam;

    // public override void _Process(double delta) { }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton && @event.IsPressed())
        {
            GD.Print(mouseButton.ButtonIndex);

        }
    }


}
