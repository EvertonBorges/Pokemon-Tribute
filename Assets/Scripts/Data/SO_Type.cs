using System.Collections.Generic;
using UnityEngine;

public class SO_Type : ScriptableObject
{

    public int id = 0;    
    public new string name = "";
    public List<SO_Type> double_damage_from = new();
    public List<SO_Type> double_damage_to = new();
    public List<SO_Type> half_damage_from = new();
    public List<SO_Type> half_damage_to = new();
    public List<SO_Type> no_damage_from = new();
    public List<SO_Type> no_damage_to = new();

    public static SO_Type ToSoType(
        PokeAPI_Type typeApi)
    {
        SO_Type type;

        if (Manager_Resources.Pokemon_Resourcers.Types.ContainsKey(typeApi.name))
            type = Manager_Resources.Pokemon_Resourcers.Types[typeApi.name];
        else
        {
            type = CreateInstance<SO_Type>();
            
            type.name = typeApi.name;
        }


        return type;
    }

}
