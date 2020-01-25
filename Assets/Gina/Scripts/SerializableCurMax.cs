using System;

[Serializable]
public class SerializableCurMax
{
    public float cur;
    public float max;
    public float multiplier;

    public SerializableCurMax(float cur, float max, float multiplier = 0)
    {
        this.cur = cur;
        this.max = max;
        this.multiplier = multiplier;
    }
    public SerializableCurMax(SerializableCurMax value)
    {
        this.cur = value.cur;
        this.max = value.max;
        this.multiplier = value.multiplier;
    }

    public float GetMax() => max * multiplier;
    public float percentage() => cur / GetMax();
    public float normalize() => cur / max;
    public override string ToString() => $"{cur},{max}*{multiplier}";

}