using Godot;
using System;


public partial class ChestRandomizer : Label
{
    public override void _Ready()
    {
        GD.Randomize();
    }

    void Generate()
    {
        CardBuilder Card = new();
        Card.SetValue((int)(GD.Randi() % 14 + 1))
            .Bless((int)GD.Randfn(50, 20));

        if (GD.Randf() < 0.3f) Card.DevilContract();
        if (GD.Randf() < 0.4f) Card.Curse(4);

        (Text, LabelSettings.FontColor) = Card.BuildItem();
        // Text = text;
        // LabelSettings.FontColor = color;
    }

}
