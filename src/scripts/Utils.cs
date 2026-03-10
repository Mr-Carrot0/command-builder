using Godot;
using System;

public static class Utils
{
    public static Vector3 FlattenVecZ(Vector2 vec, float z = 0)
    {
        return new(vec.X, vec.Y, 0);
    }
    public static Vector3 FlattenVecY(Vector2 vec, float y = 0)
    {
        return new(vec.X, y, vec.Y);
    }
    public static Vector3 PrependVec2(Vector2 vec, float x = 0)
    {
        return new(x, vec.X, vec.Y);
    }
    // public static void IterVec3(Vector3 vec, Action<float, int, Vector3> fn)
    // {
    //     for (int i = 0; i < 3; i++)
    //     {
    //         fn(vec[i], i, vec);
    //     }
    // }
}