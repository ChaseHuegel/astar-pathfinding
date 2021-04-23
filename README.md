# astar-pathfinding
A* pathfinding in C# for my Swordfish library

An implementation of A* in C# that plugs right into Unity. Remove the calls to Unity transforms and Vectors to use in any other C# project. To get started, create a class inheriting from Actor and then use Actor.Goto() and watch it come to life! See VillagerActor.cs for an example, where it will pick a random point to path to unless you click somewhere to move.

I've largely optimized for memory usage, however it will still need some improvements for performance. Additionally pathfinding is done using lists which will be changed out to a more efficient structure, likely a heap.
