using Godot;
using System;

using Idxz = (int x, int z);

public partial class GameBoard : StaticBody3D
{
    [Export] Camera3D Cam;
    [Export] CursorRayCast CursorCast;
    [Export] Button UndoButton;
    readonly CommandManager CommandCtx = new();
    [Export] CollisionShape3D ColShape;


    public const int dimentions = 5;
    public readonly Vector3 Size = new(dimentions, 1, dimentions);

    readonly BasicBox[][] Boxes = new BasicBox[dimentions][];

    public BasicBox GetBox(Idxz id)
    {
        return Boxes[id.x][id.z];
    }

    PackedScene BoxScene;
    [Export] Node3D BoxContainer;

    public override void _Ready()
    {
        BoxScene = GD.Load<PackedScene>("res://src/prefabs/BasicBox.tscn");
        UndoButton.Pressed += CommandCtx.Undo;

        for (int i = 0; i < dimentions; i++)
        {

            Boxes[i] = new BasicBox[dimentions];

            for (int j = 0; j < dimentions; j++)
            {
                BasicBox m = BoxScene.Instantiate<BasicBox>();
                m.Position = new(i, 0, j);
                Boxes[i][j] = m;
                BoxContainer.AddChild(m);
            }
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvt && @event.IsPressed()
            && (mouseEvt.ButtonIndex == MouseButton.Right || mouseEvt.ButtonIndex == MouseButton.Left)
        )
        {
            if (CursorCast.Test(mouseEvt) is RayHit rayHit)
            {
                switch (mouseEvt.ButtonIndex)
                {
                    case MouseButton.Left:
                        if (rayHit.collider is GameBoard Board)
                        {
                            Idxz idx = ((int)Mathf.Round(rayHit.position.X - Board.GlobalPosition.X - 0.5f),
                                (int)Mathf.Round(rayHit.position.Z - Board.GlobalPosition.Z - 0.5f)
                            );

                            if (idx.x < dimentions && idx.z < dimentions
                                && idx.x >= 0 && idx.z >= 0)
                            {
                                CommandCtx.Add(new AddTile(GetBox(idx)));
                            }
                        }
                        break;
                    case MouseButton.Right:
                        if (rayHit.collider is BasicBox box)
                        {
                            CommandCtx.Add(new RemoveTile(box));
                        }
                        break;
                }
            }
        }
    }
    class AddTile(BasicBox Tile) : ICommand
    {
        public readonly BasicBox Tile = Tile;
        public void Execute()
        {
            Tile.Enabled = true;
        }
        public void Undo()
        {
            Tile.Enabled = false;
        }
    }
    class RemoveTile(BasicBox Tile) : ICommand
    {
        public readonly BasicBox Tile = Tile;
        public void Execute()
        {
            Tile.Enabled = false;
        }
        public void Undo()
        {
            Tile.Visible = true;
        }
    }
}


