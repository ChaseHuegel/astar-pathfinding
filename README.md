# astar-pathfinding
A* pathfinding in C# for my Swordfish library

An implementation of A* in C# that plugs right into Unity. Remove the calls to Unity transforms and Vectors to use in any other C# project. To get started, create a class inheriting from Actor and then use Actor.Goto() and watch it come to life it avoids other actors and obstacles! The pathfinding grid can be updated on the fly for dynamic environments, which allows for actors to be considered obstacles and gives way for something like an RTS where a player can put down a building. The Obstacle class is a static actor which will block a specified rectangle area from pathing. Use the obstacle class to define trees, rocks, buildings, etc.

See VillagerActor.cs for an example, where it will pick a random point to path to unless you click somewhere to move.

I've largely optimized for memory usage, however it will still need some improvements for performance. Additionally pathfinding is done using lists which will be changed out to a more efficient structure, likely a heap.

![e5d677538083619ace97d95fea7edfc7](https://user-images.githubusercontent.com/14932139/115832569-0910d800-a3e1-11eb-89e1-af1e45a9da3a.gif)
