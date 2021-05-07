# astar-pathfinding
Multithreaded A* pathfinding in C# for my Swordfish library with simple but clever pathfinding AI

An implementation of A* in C# that plugs right into Unity. To get started, create a class inheriting from Actor and then use Actor.Goto() and watch it come to life it avoids other actors and obstacles! The pathfinding grid can be updated on the fly for dynamic environments, which allows for actors to be considered obstacles and gives way for something like an RTS where a player can put down a building. The Obstacle class is a static actor which will block a specified rectangle area from pathing. Use the obstacle class to define trees, rocks, buildings, etc. See VillagerActor.cs for an example of how to use Actor.Goto

This project will continue to be updated to make a robust and fast A* solution thats easily extensible and highly configurable.

### V4
5/6/2021 - Actor behavior greatly improved and revised the system for AI pathing to be driven by abstract goals wrapped into a simple state machine. Using events, the user defines conditions for a goal to become a target, what happens when it is found, and what happens when it is interacted with. If an Actor has no goals, they will idle. By default, an Actor will roam aimlessly until it discovers a goal in its vicinity after which it will path to it. Below is the system in use for a group of lumberjacks collecting and dropping off wood. This behavior hasn't been committed to this branch as of yet as its still getting some refinement, but you can check out the project its in here: https://github.com/ChaseHuegel/vr-rts

![43f57adbb9db67225174e72f0bfce673](https://user-images.githubusercontent.com/14932139/117390717-adece400-aebc-11eb-97ed-f37011cf553a.gif)

### V3
If an Actor's path becomes blocked, they will wait for a time to see if their path clears. On failure, they attempt finding a new path a number of times. If they can't find a new path, they give up and stay where they are.

![4e168d9dbef054a3975eb69394b0f074](https://user-images.githubusercontent.com/14932139/115980384-506ea400-a55a-11eb-9445-d8edb6f16a0d.gif)

### V2

![3c8a2b50e2177905800ff1842c3e5565](https://user-images.githubusercontent.com/14932139/115919589-0b0d8200-a447-11eb-9afd-b978a383f9d3.gif)

### V1

![e5d677538083619ace97d95fea7edfc7](https://user-images.githubusercontent.com/14932139/115832569-0910d800-a3e1-11eb-89e1-af1e45a9da3a.gif)
