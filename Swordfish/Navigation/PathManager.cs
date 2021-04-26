using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics;
using UnityEngine;
using Swordfish.Threading;

namespace Swordfish.Navigation
{

public class PathManager : Singleton<PathManager>
{
    [System.Serializable]
    public class PathRequest
    {
        public Actor actor;
        public Coord2D target;
        public bool ignoreActors;

        public PathRequest(Actor actor, int x, int y, bool ignoreActors = true)
        {
            this.actor = actor;
            this.target = new Coord2D(x, y);
            this.ignoreActors = ignoreActors;
        }
    }

    protected ConcurrentQueue<PathRequest> pathingQueue;    //  Requests ready to pathfind
    protected ConcurrentQueue<PathRequest> requests;        //  Requests needing verification

    private PathRequest currentRequest;
    private ThreadWorker pathingThread;
    private ThreadWorker requestThread;

    public static void RequestPath(Actor actor, int targetX, int targetY, bool ignoreActors = true)
    {
        Instance.requests.Enqueue( new PathRequest(actor, targetX, targetY, ignoreActors) );
    }

    public void Start()
    {
        pathingQueue = new ConcurrentQueue<PathRequest>();
        requests = new ConcurrentQueue<PathRequest>();

        pathingThread = new ThreadWorker(HandleRequest);
        requestThread = new ThreadWorker(PullRequest);

        pathingThread.Start();
        requestThread.Start();
    }

    public void OnDestroy()
    {
        pathingThread.Kill();
        requestThread.Kill();
    }

    public void HandleRequest()
    {
        pathingQueue.TryDequeue(out currentRequest);
        if (currentRequest == null) return;

        currentRequest.actor.currentPath = Path.Find( currentRequest.actor.GetCellAtGrid(), World.at(currentRequest.target.x, currentRequest.target.y) );
    }

    public void PullRequest()
    {
        PathRequest request;
        requests.TryDequeue(out request);
        if (request == null) return;

        //  Try to find an existing pathing attempt for this actor
        PathRequest pathfind = null;

        foreach (PathRequest r in Instance.pathingQueue)
        {
            if (r.actor == request.actor)
            {
                pathfind = r;
                break;
            }
        }

        //  Create a request if there isn't one, otherwise update it
        if (pathfind == null)
            Instance.pathingQueue.Enqueue(request);
        else
            request.target = new Coord2D(request.target.x, request.target.y);
    }
}

}