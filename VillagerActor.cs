using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Swordfish.Navigation;

public class VillagerActor : Actor
{
    public void Update()
    {
        //  Left click goes to where you click
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 target = Camera.main.ScreenToWorldPoint( new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0) );
            target -= World.GetPositionOffset();

            Goto( (int)target.x, (int)target.z );
        }

        //  Right click picks a random path
        if (Input.GetMouseButtonDown(1))
        {
            Goto( Random.Range(0, World.GetGridSize()), Random.Range(0, World.GetGridSize()) );
        }
    }
}
