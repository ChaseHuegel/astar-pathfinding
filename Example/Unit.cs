using UnityEngine;
using Swordfish;
using Swordfish.Navigation;

public class Unit : Actor, IFactioned
{
    [Header("Faction")]
    public byte factionID = 0;
    private Faction faction;

    public Faction GetFaction() { return faction; }
    public void UpdateFaction() { faction = GameMaster.Factions.Find(x => x.index == factionID); }

    public RTSUnitType rtsUnitType;

    // Make this read only, we should only be able to change unit properties
    // through the database.
    public RTSUnitTypeData rtsUnitTypeData { get; }
    protected RTSUnitTypeData m_rtsUnitTypeData;
    
    public override void Initialize()
    {
        base.Initialize();
        
        // TODO: This could be removed at a later date and replaced with specific fetches
        // of the information needed in inheritors if we want to sacrifice memory
        // for performance
        m_rtsUnitTypeData = GameMaster.Instance.FindUnitData(rtsUnitType);

        UpdateFaction();
    }

    public bool IsCivilian()
    {
        return (int)rtsUnitType < (int)RTSUnitType.Scout;
    }
}
