using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Swordfish.Navigation;

public class VillagerActor : Actor
{
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 target = Camera.main.ScreenToWorldPoint( new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0) );

            currentPath = Path.Find( GetCell(), World.GetGrid().at( (int)target.x, (int)target.z) );
        }

        if (Input.GetMouseButtonDown(1))
        {
            currentPath = null;
        }
    }

    public override void Tick()
    {
        //  Choose a random path if we don't have one
        if (HasValidPath() == false)
        {
            currentPath = Path.Find(
                GetCell(),
                World.GetGrid().at(
                    Random.Range(0, World.GetGridSize() ),
                    Random.Range(0, World.GetGridSize() ))
            );
        }
    }
}
