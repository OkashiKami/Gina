using System;
using UnityEngine;

[Serializable]
public class SerializableVector
{
    public float x, y, z;

    public SerializableVector(float x = 0, float y = 0, float z = 0) { this.x = x; this.y = y; this.z = z; }
    public SerializableVector(SerializableVector value)
    {
        this.x = value.x;
        this.y = value.y;
        this.z = value.z;
    }

    public static implicit operator SerializableVector(Vector2 vector) => new SerializableVector(vector.x, vector.y);
    public static implicit operator Vector2(SerializableVector vector) => new Vector2(vector.x, vector.y);

    public static implicit operator SerializableVector(Vector3 vector) => new SerializableVector(vector.x, vector.y, vector.z);
    public static implicit operator Vector3(SerializableVector vector) => new Vector3(vector.x, vector.y, vector.z);

    public override string ToString() => $"{x},{y},{z}";
}