using Godot;
using System;
using Godot.Collections;

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

    Vector3 RayHitPos;

    public override void _Ready()
    {
        BoxScene = GD.Load<PackedScene>("res://src/prefabs/BasicBox.tscn");
        // GD.Print(CommandCtx);
        UndoButton.Pressed += CommandCtx.Undo;

        // init data (debug)
        for (int i = 0; i < dimentions; i++)
        {

            Boxes[i] = new BasicBox[dimentions];

            for (int j = 0; j < dimentions; j++)
            {
                BasicBox m = BoxScene.Instantiate<BasicBox>();
                m.Position = new(i, 0, j);
                // Boxes[i][j].Visible = false;
                Boxes[i][j] = m;
                BoxContainer.AddChild(m);
            }
        }


        // GD.Print(data);
        PrintData();
    }
    public override void _Process(double delta)
    {
        DebugDraw3D.DrawSphere(RayHitPos, 0.3f);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvt && @event.IsPressed()
            && (mouseEvt.ButtonIndex == MouseButton.Right || mouseEvt.ButtonIndex == MouseButton.Left)
        )
        {
            if (CursorCast.Test(mouseEvt) is RayHit rayHit)
            {
                BasicBox bestBox = null;

                if (rayHit.collider is BasicBox box)
                {
                    bestBox = box;
                    if (mouseEvt.ButtonIndex == MouseButton.Right)
                    {
                        CommandCtx.Add(new RemoveTile(box));

                    }
                    GD.Print(box);
                }
                else
                {
                    if (mouseEvt.ButtonIndex == MouseButton.Left && rayHit.collider is GameBoard Board)
                    {

                        RayHitPos = rayHit.position;
                        Idxz idx = ((int)Mathf.Round(RayHitPos.X - Board.GlobalPosition.X - 0.5f),
                            (int)Mathf.Round(RayHitPos.Z - Board.GlobalPosition.Z - 0.5f)
                        );

                        if (idx.x < dimentions && idx.z < dimentions
                            && idx.x >= 0 && idx.z >= 0)
                        {
                            // bestBox = GetBox(idx);
                            CommandCtx.Add(new AddTile(GetBox(idx)));

                        }
                    }
                }
            }
        }
    }
    public class AddTile(BasicBox Tile) : ICommand
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
    public class RemoveTile(BasicBox Tile) : ICommand
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

    void PrintData()
    {
        for (int col = 0; col < dimentions; col++)
        {
            string printOut = "";
            for (int row = 0; row < dimentions; row++)
            {
                printOut += Boxes[col][row] != null && Boxes[col][row].Visible
                    ? " 1" : " 0";
            }
            GD.Print(printOut);
        }
    }
    public static class Debug
    {
        public static int BtI(bool b)
        {
            return b ? 1 : 0;
        }
        public static string BtIS(bool b)
        {
            return b ? "1" : "0";
        }
        public static string BoolsToStr(bool[] bools)
        {
            string rt = "";
            for (int i = 0; i < bools.Length; i++)
            {
                rt += " " + BtIS(bools[i]);
            }
            return rt;
        }
    }

}


