using System.Collections.Generic;
using Swordfish;
using Swordfish.Navigation;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class Structure : Obstacle, IFactioned
{
    public byte factionID = 0;
    private Faction faction;

    // Set to grab data about this building from database.
    public RTSBuildingType rtsBuildingType;
    protected RTSBuildingTypeData rtsBuildingTypeData;

    private bool built = false;
    private Damageable damageable;
    public Damageable AttributeHandler { get { return damageable; } }

    public Faction GetFaction() { return faction; }
    public void UpdateFaction() { faction = GameMaster.Factions.Find(x => x.index == factionID); }

    public bool NeedsRepairs() { return damageable.GetAttributePercent(Attributes.HEALTH) < 1f; }
    public bool IsBuilt() { return built; }

    public override void Initialize()
    {
        base.Initialize();

        // Retrieve building data from database.
        rtsBuildingTypeData = GameMaster.Instance.FindBuildingData(rtsBuildingType);

        UpdateFaction();

        if (!(damageable = GetComponent<Damageable>()))
            Debug.Log("No damageable component on structure!");
    }

    public void TryRepair(int count, Actor repairer = null)
    {
        AttributeHandler.Heal(count, AttributeChangeCause.HEALED, repairer.AttributeHandler);
    }

    public bool CanDropOff(ResourceGatheringType type)
    {
        return rtsBuildingTypeData.dropoffTypes.HasFlag(type);
    }
}
