# Terrace

## Concept

Terrace is a  point & click narrative game about loss and coping with it. It is set within the main character's terrace, which is rundown. As a player, you experience the story as the terrace is put back together. There is a focus on gardening and the way to experience the story is based on the seeds that are planted.

## Gameplay loop
The game is set entirely within the Terrace, and the player can cycle between day and night and experience different weathers each time they choose to go back inside.
It's also possible to interact with objects by clicking on them, and also collect objects like seeds and gardening tools. 

## Structure
As my first big Unity project, I relied heavily on the Singleton pattern, applying it to several connected managers, such as:

- Click Manager, to manage movement and interaction with objects
- State Manager, to track game states
- Story Manager, to track story progression
- Time Manager, to track time progression and weather
- Inventory Manager, to track objects, such as seeds, harvested plants and tools

Each object that could be interacted had an associated class inherited from InteractableObject. Information about collectable objects was stored using the ObjDescription (scriptable object) class.

Movement was done using a NavMesh integrated with the [A* Pathfinding](https://arongranberg.com/astar/) project to account for obstacles.

Dialogue was handled using [Yarn Spinner](https://yarnspinner.dev/docs/unity/).

## Links
[Itch.io Page](https://terrace.itch.io/terrace)
[My website](cesardaher.com)
