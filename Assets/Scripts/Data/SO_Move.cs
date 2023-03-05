using UnityEngine;


public class SO_Move : ScriptableObject
{
    public int id;
    public int accuracy;
    public new string name;
    public string description;
    public int power;
    public int pp;
    public int priority;
    public string damageClass;

    public SO_MoveTarget moveTarget;
    public SO_Type type;
}
