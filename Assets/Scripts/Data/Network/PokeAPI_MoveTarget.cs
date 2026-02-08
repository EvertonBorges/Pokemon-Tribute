using System;
using System.Collections.Generic;

[Serializable]
public class PokeAPI_MoveTarget
{

    public int id;    
    public string name;
    public List<PokeAPI_Descrition> descriptions;

    public class PokeAPI_Descrition
    {
        public string description;
        public PokeAPI_NameUrl language;
    }

}
