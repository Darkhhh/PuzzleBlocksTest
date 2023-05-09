using UnityEngine;

public static class Extensions
{
    public static Vector3Int GetIntVector(this Vector3 t)
    {
        return new Vector3Int(Mathf.RoundToInt(t.x), Mathf.RoundToInt(t.y), Mathf.RoundToInt(t.z));
    }
}