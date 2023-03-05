using UnityEngine;

[System.Serializable]
public class DeterminantValues
{
    
    [Range(0, 15)] private int hp;
    public int HP => hp;
    [SerializeField] [Range(0, 15)] private int attack;
    public int Attack => attack;
    [SerializeField] [Range(0, 15)] private int defense;
    public int Defense => defense;
    [SerializeField] [Range(0, 15)] private int special;
    public int Special => special;
    [SerializeField] [Range(0, 15)] private int speed;
    public int Speed => speed;

    public void SetupValues(bool randomize)
    {
        if (randomize)
        {
            attack = Random.Range(0, 15);
            defense = Random.Range(0, 15);
            special = Random.Range(0, 15);
            speed = Random.Range(0, 15);
        }

        CalcHp();
    }

    public void CalcHp()
    {
        hp = Odd(attack) * 8 + Odd(defense) * 4 + Odd(special) * 2 + Odd(speed) * 1;
    }

    private int Odd(int value)
    {
        return value % 2;
    }

}
