using Godot;
using System;
class CardBuilder
{
    public CardBuilder(SuitEnum? suit = null)
    {
        SetSuit(suit ?? RadnomSuit());
    }
    public enum SuitEnum
    {
        Hearts,
        Spades,
        Diamonds,
        Clubs,
    }
    public static SuitEnum RadnomSuit()
    {
        return (SuitEnum)GD.RandRange(0, 3);
    }
    SuitEnum? Suit;
    int CardValue = 0;
    int Divinity = 0; // max=100
    int Demonic = 0; // max=100
    float Condition = 7; // max=10
    // int Quality;
    // const Array SuitValues = Enum.GetValues(typeof(SuitEnum));
    public CardBuilder SetSuit(SuitEnum? suit)
    {
        Suit = suit;
        CardValue = Suit switch
        {
            SuitEnum.Hearts => 10,
            SuitEnum.Diamonds => 5,
            SuitEnum.Spades => 5,
            SuitEnum.Clubs => 2,
            _ => -1,
        };
        return this;
    }
    public CardBuilder SetValue(int v)
    {
        CardValue = v;
        return this;
    }
    public CardBuilder Bless(int strength)
    {
        Divinity += strength;
        if (CardValue > 10) CardValue++;
        return this;
    }
    public CardBuilder Curse(int strength)
    {
        Divinity -= strength / 2;
        Demonic += (int)Mathf.Round(strength * 1.5f);
        CardValue -= strength / 20;
        return this;
    }

    public CardBuilder DevilContract()
    {
        Demonic += 20;
        CardValue -= Mathf.Min(CardValue + 1, 3);
        if (Demonic >= 50) CardValue *= 2;
        return this;
    }

    public (string text, Color color) BuildItem()
    {
        Color color = CardValue > 10 ? Colors.Goldenrod : Colors.White;
        if (CardValue == 1) color = Colors.SeaGreen;
        string str = "";

        if (Divinity > 50) { str += "Holy "; color = Colors.LightSkyBlue; }
        else if (Divinity < 0)
        {
            str += "Godless ";
            color = Colors.Gray;
        }
        if (Demonic > 50) { str += "Demon's "; color = new Color(101, 28, 50); }

        str += Condition switch
        {
            var _ when Condition < 1f => "Broken ",
            var _ when Condition < 5f => "Damaged ",
            var _ when Condition > 9.9f => "Pristine ",
            _ => "",
        };

        if (Demonic < 0) str += "Cleansed ";

        str += SuitToStr(CardValue);
        str += " of ";
        str += Enum.GetName(typeof(SuitEnum), Suit) ?? "the OtherWorld";
        if (CardValue < 0) str = str.TrimEnd('s');

        if (Divinity > 90 && Demonic > 90) { str += " (Artifact from Bard of Anarchy)"; color = new Color(0x36013f); }
        GD.Print($"CardValue={CardValue}, Divinity={Divinity}, Demonic={Demonic}, Condition={Condition}");
        return (str, color);
    }
    static string SuitToStr(int? Value)
    {
        if (Value is null) return "Corruptions";

        return Value switch
        {
            var expr when Value < 0 => $"Curse Lv{-expr}",
            0 => "Trash",
            1 => "Ace",
            2 => "Duce",
            11 => "Knight",
            12 => "Queen",
            13 => "King",
            var expr when Value > 13 => $"Ascended One Lv{expr - 13}",
            _ => Utils.Lettrify3to10((int)Value),
        };
        /**
        * 1: Ace
        * 2: Duce
        * 11: Knight
        * 12: Queen
        * 13: King
        * 3-10: return self
        */
    }
}