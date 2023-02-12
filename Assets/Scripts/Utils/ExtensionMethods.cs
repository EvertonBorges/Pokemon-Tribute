using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public static class ExtensionMethods
{

    private const string ResourcesFolder = "Assets/Resources";

    public static bool IsEmpty(this string value) => value == null || value == "" || value.Length <= 0 || value.ToLower() == "null";

    public static bool IsEmpty<T>(this T[] value) => value == null || value.Length <= 0;

    public static bool IsEmpty<T>(this IList<T> value) => value == null || value.Count <= 0;

    public static bool IsEmpty<T, U>(this Dictionary<T, U> value) => value == null || value.Count <= 0;

    public static string ToSingleString(this string[] values) => values.ToList().ToSingleString();

    public static string ToSingleString(this List<string> values)
    {
        string result = "";

        foreach (var value in values)
            result += $"{value}{(value == values[^1] ? "." : ", ")}";

        return result;
    }

    public static UnityWebRequestAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOp)
    {
        return new UnityWebRequestAwaiter(asyncOp);
    }

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

    private static int RomanToDecimal(char r)
    {
        if (r == 'I' || r == 'i')
            return 1;
        if (r == 'V' || r == 'v')
            return 5;
        if (r == 'X' || r == 'x')
            return 10;
        if (r == 'L' || r == 'l')
            return 50;
        if (r == 'C' || r == 'c')
            return 100;
        if (r == 'D' || r == 'd')
            return 500;
        if (r == 'M' || r == 'm')
            return 1000;
        return -1;
    }

    public static int RomanToDecimal(this string str)
    {
        int res = 0;

        for (int i = 0; i < str.Length; i++)
        {
            int s1 = RomanToDecimal(str[i]);

            if (i + 1 < str.Length)
            {

                int s2 = RomanToDecimal(str[i + 1]);

                if (s1 >= s2)
                    res += s1;
                else
                {
                    res = res + s2 - s1;
                    i++;
                }
            }
            else
            {
                res += s1;
                i++;
            }
        }

        return res;
    }

    public static void CreateResourcesFolder(this string fullpath)
    {
        if (!fullpath.StartsWith(ResourcesFolder))
            return;

        if (!fullpath.EndsWith(".asset"))
            return;

        var folder = fullpath[..fullpath.LastIndexOf("/")];

        var paths = folder.Split("/");

        var path = "";

        for (int i = 0; i < paths.Length; i++)
        {
            if (i == 0)
                path += paths[i];
            else
                path += $"/{paths[i]}";

            if (!AssetDatabase.IsValidFolder(path))
            {
                var parentDir = path[..path.LastIndexOf("/")];

                var folderDir = path[(path.LastIndexOf("/") + 1)..];

                AssetDatabase.CreateFolder(parentDir, folderDir);
            }
        }
    }

}
