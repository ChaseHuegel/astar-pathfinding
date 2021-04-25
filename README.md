# astar-pathfinding
A* pathfinding in C# for my Swordfish library with simple but clever pathfinding AI

An implementation of A* in C# that plugs right into Unity. Remove the calls to Unity transforms and Vectors to use in any other C# project. To get started, create a class inheriting from Actor and then use Actor.Goto() and watch it come to life it avoids other actors and obstacles! The pathfinding grid can be updated on the fly for dynamic environments, which allows for actors to be considered obstacles and gives way for something like an RTS where a player can put down a building. The Obstacle class is a static actor which will block a specified rectangle area from pathing. Use the obstacle class to define trees, rocks, buildings, etc. See VillagerActor.cs for an example of how to use Actor.Goto

If an Actor's path becomes blocked, they will wait for a time to see if their path clears. On failure, they attempt finding a new path a number of times. If they can't find a new path, they give up and stay where they are.

### V3

![4e168d9dbef054a3975eb69394b0f074](https://user-images.githubusercontent.com/14932139/115980384-506ea400-a55a-11eb-9445-d8edb6f16a0d.gif)

### V2

![3c8a2b50e2177905800ff1842c3e5565](https://user-images.githubusercontent.com/14932139/115919589-0b0d8200-a447-11eb-9afd-b978a383f9d3.gif)

### V1

![e5d677538083619ace97d95fea7edfc7](https://user-images.githubusercontent.com/14932139/115832569-0910d800-a3e1-11eb-89e1-af1e45a9da3a.gif)
