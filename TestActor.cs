using UnityEngine;
using Swordfish.Navigation;

public class TestActor : Actor
{
    public void Update()
    {
        //  Left click goes to where you click
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 target = Camera.main.ScreenToWorldPoint( new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0) );

            target = World.ToWorldSpace(target);

            GotoForced( (int)target.x, (int)target.z );
        }

        //  Right click picks a random path
        if (Input.GetMouseButtonDown(1))
        {
            GotoForced( Random.Range(0, World.Grid.GetSize()), Random.Range(0, World.Grid.GetSize()) );
        }

        //  Space will freeze/unfreeeze AI
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleFreeze();
        }
    }
}
