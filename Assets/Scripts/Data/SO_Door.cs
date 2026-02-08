using System.Collections;
using System.Linq;
using UnityEngine;

public class SO_Door : ScriptableObject
{
    
    [SerializeField] private InspectorDoor _from = new();
    public InspectorDoor From => _from;
    [SerializeField] private InspectorDoorLink _to = new();
    public InspectorDoorLink To => _to;

    public void SetPath(string value) => _from.SetPath(value);

    public override string ToString()
    {
        return $"{_from} -> {_to}";
    }

}
