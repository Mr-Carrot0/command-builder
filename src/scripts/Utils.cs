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

    public static string Lettrify3to10(int i)
    {
        return i switch
        {
            3 => "three",
            4 => "four",
            5 => "five",
            6 => "six",
            7 => "seven",
            8 => "eight",
            9 => "nine",
            10 => "ten",
            _ => i.ToString(),
        };
    }
}