using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    
    public static bool IsEmpty(this string value) => value == null || value == "" || value.Length <= 0 || value.ToLower() == "null";
    
    public static bool IsEmpty<T>(this T[] value) => value == null || value.Length <= 0;

    public static bool IsEmpty<T>(this IList<T> value) => value == null || value.Count <= 0;
    
    public static bool IsEmpty<T, U>(this Dictionary<T, U> value) => value == null || value.Count <= 0;

    public static DirectionsEnum Vector2ToDirection(this Vector2 value)
    {
        if (value == Vector2.up) return DirectionsEnum.UP;

        if (value == Vector2.right) return DirectionsEnum.RIGHT;

        if (value == Vector2.down) return DirectionsEnum.DOWN;
        
        if (value == Vector2.left) return DirectionsEnum.LEFT;

        return DirectionsEnum.NONE;
    }

    public static Vector3 DirectionToVector3(this DirectionsEnum value)
    {
        return value switch
        {
            DirectionsEnum.UP => Vector2.up,
            DirectionsEnum.RIGHT => Vector2.right,
            DirectionsEnum.DOWN => Vector2.down,
            DirectionsEnum.LEFT => Vector2.left,
            _ => Vector2.zero,
        };
    }

}
