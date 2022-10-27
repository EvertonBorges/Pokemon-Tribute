using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class InspectorScene
{

    [SerializeField] private bool _visibleLabel = true;
    [SerializeField] private string _scene;

    public string Scene => _scene;

    private int BuildIndex => SceneUtility.GetBuildIndexByScenePath(Extensions.Scenes[_scene]);

    private bool ValidScene => !_scene.IsEmpty() && Extensions.Scenes.ContainsKey(_scene);

    public InspectorScene() { }

    public InspectorScene(string scene)
    {
        _scene = scene;
    }

    public AsyncOperation LoadSceneAsync(LoadSceneMode mode = LoadSceneMode.Single)
    {
        if (!ValidScene)
            return null;

        return SceneManager.LoadSceneAsync(BuildIndex, mode);
    }

    public void LoadScene(LoadSceneMode mode = LoadSceneMode.Single)
    {
        if (!ValidScene)
            return;

        SceneManager.LoadScene(BuildIndex, mode);
    }

    public AsyncOperation UnloadSceneAsync()
    {
        if (!ValidScene)
            return null;

        return SceneManager.UnloadSceneAsync(BuildIndex);
    }

    public override string ToString()
    {
        return $"{_scene}";
    }

    public static implicit operator InspectorScene(string value) => new(value);

    public class Extensions
    {

        public static Dictionary<string, string> Scenes
        {
            get
            {
                Dictionary<string, string> scenes = new();

                for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
                {
                    var path = SceneUtility.GetScenePathByBuildIndex(i);

                    if (path.IsEmpty())
                        continue;

                    scenes.Add(GetScenePath(path), path);
                }

                return scenes;
            }
        }

        private static string GetScenePath(string path)
        {
            for (int i = 0; i < 2; i++)
                path = path.Remove(0, path.IndexOf("/") + 1);

            return path.Replace(".unity", "");
        }

    }

}
