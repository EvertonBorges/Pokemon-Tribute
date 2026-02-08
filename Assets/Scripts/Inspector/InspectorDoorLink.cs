using UnityEngine;

[System.Serializable]
public class InspectorDoorLink
{

    [SerializeField] private bool _visibleLabel = true;
    [SerializeField] private string _path;
    public string Path => _path;

    public override string ToString()
    {
        return _path;
    }

}
