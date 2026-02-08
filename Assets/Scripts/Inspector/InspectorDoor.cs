using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class InspectorDoor
{

    [SerializeField] private bool _visibleLabel = true;
    [SerializeField] private string _path;
    [SerializeField] private InspectorScene _scene;

    public string Path => _path;
    public InspectorScene Scene => _scene;

    public void SetPath(string value) => _path = value;

    public override string ToString()
    {
        return _path;
    }
    
}
