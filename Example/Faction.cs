using Swordfish;
using UnityEngine;

[CreateAssetMenu(fileName = "New Faction", menuName = "RTS/Faction")]
public class Faction : ScriptableObject
{
    public byte index;
    public Color color = Color.blue;
    private BitMask mask;

    public void SetAlly(Faction faction)
    {
        Bit.Set(ref mask.bits, faction.index);
    }

    public void SetEnemy(Faction faction)
    {
        Bit.Clear(ref mask.bits, faction.index);
    }

    public bool IsAllied(Faction faction)
    {
        return Bit.Compare(mask, faction.mask, faction.index);
    }
}

public interface IFactioned
{
    Faction GetFaction();
}