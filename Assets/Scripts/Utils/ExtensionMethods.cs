using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ExtensionMethods
{
    
    public static bool IsEmpty(this string value) => value == null || value == "" || value.Length <= 0;
    
    public static bool IsEmpty<T>(this T[] value) => value == null || value.Length <= 0;

    public static bool IsEmpty<T>(this IList<T> value) => value == null || value.Count <= 0;

    public static AsyncOperation LoadSceneAsync(this ScenesEnum value, LoadSceneMode mode = LoadSceneMode.Single) => SceneManager.LoadSceneAsync(value.ToString(), mode);

    public static void LoadScene(this ScenesEnum value, LoadSceneMode mode = LoadSceneMode.Single) => SceneManager.LoadScene(value.ToString(), mode);

    public static AsyncOperation UnloadSceneAsync(this ScenesEnum value) => SceneManager.UnloadSceneAsync(value.ToString());

    public static DirectionsEnum Vector2ToDirection(this Vector2 value)
    {
        if (value == Vector2.right) return DirectionsEnum.RIGHT;

        if (value == Vector2.down) return DirectionsEnum.DOWN;
        
        if (value == Vector2.left) return DirectionsEnum.LEFT;

        return DirectionsEnum.UP;
    }

}
