using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swordfish.Navigation
{

public class PathManager : Singleton<PathManager>
{
    [System.Serializable]
    public class PathRequest
    {
        public Actor actor;
        public Coord2D target;

        public PathRequest(Actor actor, int x, int y)
        {
            this.actor = actor;
            this.target = new Coord2D(x, y);
        }
    }

    [SerializeField] protected int requestsPerUpdate = 1;
    protected Queue<PathRequest> requests;

    private PathRequest currentRequest;

    public void Start()
    {
        requests = new Queue<PathRequest>();
    }

    public void Update()
    {
        for (int n = 0; n < requestsPerUpdate; n++)
        {
            if (requests.Count > 0)
            {
                currentRequest = requests.Dequeue();
                currentRequest.actor.currentPath = Path.Find( currentRequest.actor.GetCellAtGrid(), World.at(currentRequest.target.x, currentRequest.target.y) );
            }
        }
    }

    public static void RequestPath(Actor actor, int targetX, int targetY)
    {
        //  Try to find an existing request for this actor
        PathRequest request = null; //Instance.requests.Find(x => x.actor == actor);

        foreach (PathRequest r in Instance.requests)
        {
            if (r.actor == actor)
            {
                request = r;
                break;
            }
        }

        //  Create a request if there isn't one, otherwise update it
        if (request == null)
            Instance.requests.Enqueue( new PathRequest(actor, targetX, targetY) );
        else
            request.target = new Coord2D(targetX, targetY);
    }
}

}