

https://user-images.githubusercontent.com/14932139/122598096-610f3800-d03a-11eb-944b-41615e9392a1.mp4

# astar-pathfinding
Multithreaded A* pathfinding in C# for my Swordfish library with simple but clever pathfinding AI

An implementation of A* (and associated AI) in C# that plugs right into Unity. To get started, you need a World component to define your grid and a class inheriting from Actor. Call Actor.Goto and it will navigate the grid avoiding Obstacles and Actors. Use the Obstacle component to define impassable areas. The system is designed for dynamic changes to the grid, so baking is not used and runtime modifications are fast.

See TestActor.cs for a simple example of how to make use of the A* pathfinding. To utilize the more hands-off AI behavior, call Actor.GotoNearestGoal or Actor.GotoNearestGoalWithPriority. This requires implementing Goals.

This project will continue to be updated to make a robust and fast solution thats easily extensible and highly configurable.

### V6
This update has some performance improvements and bug fixes, but the bulk of the changes are related to providing more control and tools to the user, smarter and more dynamic AI, and consistency in Actor behavior. There has also been some minor refactoring and cleaning up of the code, and additional comments on complex pieces. However, readability and documentation across the board is on the TODO list and not a focus at the moment. There have also been some minor access changes (privating Actor methods that can cause unwanted behavior) but the whole codebase needs a pass over (... and documentation) to make sure there is little room for user error.

Most importantly goals are now dynamically managed by Actor similarly to the re-pathing behavior. An Actor will change its goal priorities if it struggles or is unable to reach a goal. For behavior consistency, Actors have a short term memory of the goals they've previously interacted with. The Actor will prioritize memorized goals over searching the area around them. In addition, Actors now have the flag doMicroSearching which allows them to perform small goal searches along their path and potentially re-path. The default behavior prevents goal searching while the Actor is pathing so that the user has exact control over their movements, by enabling the micro search behavior you lose a degree of control for dynamics such as an Actor reacting to other Actors (i.e. chasing). Last another important change is that Actors will now target Bodies as goals wherever they are available. This allows actors to keep track of moving goals (i.e. other actors..)

#### Changes
- Dynamic goals
- More events to access pathing and goals
- More pathing controls: locking, pathable vs walkable cells (Cell.canPassThru), etc.
- AI memory functionality
- Performance (Goal caching, smarter goal searches, search optimizations)
- Toggleable micro-searching during pathing
- Minor refactoring to prevent user error
- Actors can now target Bodies where available
- Thread-safe fixes
- AI idling fix
- ...several other minor fixes

### V5
Large overhauls to the system with large performance improvements. AI has refined decision making logic that is managed via the goal system. Pathing and the actor logic behind it has seen small adjustments that have a large impact on intelligence and avoidance. There is a lot to explain with the huge changes, it is best demonstrated! See the example folder for an idea of the current usage. This is a very stripped down version of what is currently used in my VR-RTS collab https://github.com/ChaseHuegel/vr-rts

![916056447ac65d11c55da70350e6605e](https://user-images.githubusercontent.com/14932139/117926077-322cd600-b2c6-11eb-898f-3d4ac70948a1.gif)

### V4
Actor behavior greatly improved and revised the system for AI pathing to be driven by abstract goals wrapped into a simple state machine. Using events, the user defines conditions for a goal to become a target, what happens when it is found, and what happens when it is interacted with. If an Actor has no goals, they will idle. By default, an Actor will roam aimlessly until it discovers a goal in its vicinity after which it will path to it. Below is the system in use for a group of villagers that can build, repair, mine, cut wood, and deliver resources. This behavior hasn't been committed to this branch as of yet as its still getting some refinement.

### V3
If an Actor's path becomes blocked, they will wait for a time to see if their path clears. On failure, they attempt finding a new path a number of times. If they can't find a new path, they give up and stay where they are.

![4e168d9dbef054a3975eb69394b0f074](https://user-images.githubusercontent.com/14932139/115980384-506ea400-a55a-11eb-9445-d8edb6f16a0d.gif)

### V2

![3c8a2b50e2177905800ff1842c3e5565](https://user-images.githubusercontent.com/14932139/115919589-0b0d8200-a447-11eb-9afd-b978a383f9d3.gif)

### V1

![e5d677538083619ace97d95fea7edfc7](https://user-images.githubusercontent.com/14932139/115832569-0910d800-a3e1-11eb-89e1-af1e45a9da3a.gif)
