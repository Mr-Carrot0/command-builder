using Godot;
using System;

public partial class CursorRayCast : RayCast3D
{
    [Export] Camera3D Cam;
    public RayHit? Test(InputEventMouseButton mouseEvt)
    {
        Vector2 ScreenPos = mouseEvt.Position;
        GlobalPosition = Cam.ProjectRayOrigin(ScreenPos); //      from
        TargetPosition = Cam.ProjectRayNormal(ScreenPos) * 30; // to

        ForceRaycastUpdate();

        if (IsColliding())
        {
            return new(GetCollisionPoint(), (CollisionObject3D)GetCollider());
        }
        else return null;
    }
}

public struct RayHit(Vector3 position, CollisionObject3D collider)
{
    public Vector3 position = position;
    public CollisionObject3D collider = collider;
}
