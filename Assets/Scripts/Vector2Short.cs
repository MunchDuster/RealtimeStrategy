using UnityEngine;
using Photon.Pun;

public class Vector2Short
{
    public static Vector2Short Zero = new Vector2Short(0, 0);
    public static Vector2Short Up = new Vector2Short(0, 1);
    public static Vector2Short Down = new Vector2Short(0, -1);
    public static Vector2Short Right = new Vector2Short(1, 0);
    public static Vector2Short Left = new Vector2Short(-1, 0);
    public static short Step = 200;

    public short x;
    public short y;

    public Vector2Short(short x, short y)
    {
        this.x = x;
        this.y = y;
    }

    public Vector2Short(Vector3 position)
    {
        x = (short)Mathf.RoundToInt(position.x * Step);
        y = (short)Mathf.RoundToInt(position.z * Step);
    }

    public Vector2Short()
    {
        x = 0;
        y = 0;
    }

    /// <summary>
    /// Converts between game-position to unity coordinates
    /// World boundary: 162m
    /// Unit distance: 0.005m (1/200)
    /// </summary>
    public static implicit operator Vector3(Vector2Short v) => new Vector3(v.x / Step, 0, v.y / Step);

    /// <summary>
    /// Because normal magnitude is not optimized enough
    /// </summary>
    public int sqrMagnitude => x * x + y * y;

    /// <summary>
    /// Serialize the position into a photon data stream
    /// </summary>
    /// <param name="stream"></param>
    public void Serialize(PhotonStream stream)
    {
        stream.Serialize(ref x);
        stream.Serialize(ref y);
    }

    public static Vector2Short operator +(Vector2Short a, Vector2Short b)
        => new Vector2Short((short)(a.x + b.x), (short)(a.y + b.y));

    public static Vector2Short operator *(Vector2Short a, short b)
        => new Vector2Short((short)(a.x * b), (short)(a.y * b));

    public override string ToString() => $"short({x},{y})";
}
