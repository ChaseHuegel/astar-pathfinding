using UnityEngine;
using Swordfish;
using Swordfish.Navigation;

public class Resource : Obstacle
{
    public ResourceGatheringType type = ResourceGatheringType.None;
    public float amount = 1000;

    public float GetRemoveAmount(float count)
    {
        float value = amount - count;
        float overflow = value < 0 ? Mathf.Abs(value) : 0;

        return count - overflow;
    }

    //  Removes count and returns how much was removed
    public float TryRemove(float count)
    {
        amount -= count;
 
        float overflow = amount < 0 ? Mathf.Abs(amount) : 0;

        if (amount <= 0)
        {
            UnbakeFromGrid();
            Destroy(this.gameObject);
        }

        return count - overflow;
    }
}
