using UnityEngine;

public class Dice
{
    public int Sides { get; set; }

    public int Roll()
    {
        return Random.Range(1, Sides + 1);
    }
}