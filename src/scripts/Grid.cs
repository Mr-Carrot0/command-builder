using Godot;
using System;
using Godot.Collections;

using Idxz = (int x, int z);

public partial class Grid : StaticBody3D
{
    [Export] Camera3D Cam;
    [Export] CollisionShape3D ColShape;

    // public override void _Process(double delta) { }
    public const int dimentions = 5;
    public readonly Vector3 Size = new(dimentions, 1, dimentions);
    // public bool[][] data = new bool[dimentions][];

    BasicBox[][] Boxes = new BasicBox[dimentions][];

    public BasicBox GetBox(Idxz id)
    {
        return Boxes[id.x][id.z];
    }

    readonly CommandManager CommandCtx = new();
    PackedScene BoxScene;
    [Export] Node3D BoxContainer;

    [Export(PropertyHint.Flags, "0,board,box,3")]
    public uint bitmask { get; set; } = 0;
    void UpdateBoardSize()
    {
        float d;
        BoardSize = new(d = ((BoxShape3D)ColShape.Shape).Size.Y, d);
    }
    [Export] RayCast3D RayCast;
    Vector3 RayHitPos;
    Vector2 BoardSize;

    public override void _Ready()
    {
        BoxScene = GD.Load<PackedScene>("res://src/prefabs/BasicBox.tscn");
        // GD.Print(CommandCtx);

        UpdateBoardSize();

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
        // UpdateBoardSize();
        if (@event is InputEventMouseButton mouseEvt && @event.IsPressed()
            && (mouseEvt.ButtonIndex == MouseButton.Right || mouseEvt.ButtonIndex == MouseButton.Left)
        )
        {
            // GD.Print(mouseEvt.ButtonIndex);
            // GD.Print(mouseEvt.Position);
            // float _o = Cam.GlobalPosition.DistanceTo(GlobalPosition + new Vector3(BoardSize.X, 0, BoardSize.Y));

            // Vector3 boardPos = Cam.ProjectPosition(mouseEvt.Position,
            //     Cam.GlobalPosition.DistanceTo(GlobalPosition + new Vector3(BoardSize.X, 0, BoardSize.Y))
            // );



            // Vector2I indexifiedPos = (Vector2I)(dimentions * (mouseEvt.Position / GetWindow().Size));
            // GD.Print(indexifiedPos);

            // var m = TestRayCast(mouseEvt);
            if (TestRayCast(mouseEvt) is RayHit rayHit)
            {
                BasicBox bestBox = null;

                if (rayHit.collider is BasicBox box)
                {
                    bestBox = box;
                }
                else
                {
                    RayHitPos = rayHit.position;
                    Idxz idx = ((int)Mathf.Round(RayHitPos.X - GlobalPosition.X - 0.5f),
                        (int)Mathf.Round(RayHitPos.Z - GlobalPosition.Z - 0.5f)
                    );

                    if (idx.x < dimentions && idx.z < dimentions
                        && idx.x >= 0 && idx.z >= 0)
                    {
                        bestBox = GetBox(idx);
                    }
                }
                GD.Print(bestBox);

                // collider.Visible ^= true;
                // GD.Print(RayHitPos);
                if (bestBox is not null)
                {
                    switch (mouseEvt.ButtonIndex)
                    {
                        case MouseButton.Left:
                            var pl = new PlaceTileViz(bestBox);

                            GD.Print(pl.Tile, pl.Tile.Visible);
                            CommandCtx.Add(pl);
                            break;
                        case MouseButton.Right:
                            CommandCtx.Add(new RemoveTileViz(bestBox));
                            break;
                    }
                }
            }
            PrintData();
            GD.Print(CommandCtx.UndoStack.Count);
        }
    }
    RayHit? TestRayCast(InputEventMouseButton mouseEvt)
    {
        PhysicsDirectSpaceState3D worldSpace = Cam.GetWorld3D().DirectSpaceState;

        Vector2 ScreenPos = mouseEvt.Position;
        // Vector3 from = Cam.ProjectRayOrigin(ScreenPos);
        // Vector3 to = Cam.ProjectPosition(ScreenPos, 10);

        Godot.Collections.Dictionary res = worldSpace.IntersectRay(PhysicsRayQueryParameters3D.Create(
            Cam.ProjectRayOrigin(ScreenPos), // from
            Cam.ProjectPosition(ScreenPos, 30), // to
            bitmask
            ));


        if (!res.ContainsKey("position")) return null;

        return new((Vector3)res["position"], (CollisionObject3D)(GodotObject)res["position"]);
    }
    public class PlaceTileViz(BasicBox Tile) : ICommand
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
    public class RemoveTileViz(BasicBox Tile) : ICommand
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


public struct RayHit(Vector3 position, CollisionObject3D collider)
{
    public Vector3 position = position;
    public CollisionObject3D collider = collider;
    // public readonly Rid rid;
    // public readonly Vector3 normal;
    // public readonly object collider_id;
    // public readonly int shape;
    // public readonly Variant metadata;
}