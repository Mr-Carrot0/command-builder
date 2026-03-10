using Godot;
using System;

public partial class BasicBox : StaticBody3D
{
    [Export] MeshInstance3D MeshInstance;
    [Export] CollisionShape3D ColShape;
    public Vector3 Size
    {
        get { return Shape.Size; }
        set
        {
            Shape.Size = value;
            Mesh.Size = value;
        }
    }

    public bool Enabled
    {
        get
        {
            return Visible;
        }
        set
        {
            base.Visible = value;
            ColShape.Disabled = !value;
        }
    }

    private bool Visible
    {
        get
        {
            return base.IsVisible();
            
        }
        set
        {
            base.SetVisible(value);
        }
    }

    public BoxShape3D Shape { get { return (BoxShape3D)ColShape.Shape; } }
    public BoxMesh Mesh { get { return (BoxMesh)MeshInstance.Mesh; } }

    public override void _Ready()
    {

    }

}
