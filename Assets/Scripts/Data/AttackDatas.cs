public class AttackDatas
{

    public SO_Slot_Pokemon attacker;
    public SO_Slot_Pokemon defenser;
    public SO_Move move;

    public AttackDatas() { }

    public AttackDatas(SO_Slot_Pokemon attacker, SO_Slot_Pokemon defenser, SO_Move move)
    {
        this.attacker = attacker;
        this.defenser = defenser;
        this.move = move;
    }

}
