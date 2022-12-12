# Sandbox 

## Group members

* Mateusz
* Magnus
* Ole
* Berkay
* Viktor
* Julian

## Link to repository

## Gameplay video

## Code video
"code that is tightinly/tightly? integrated with the game engine that is difficult to see from the text of the programming."

## Group discussion
* Strengths and weaknesses of engine we used in our game
* How we controlled process and communication systems during development
* Use of version control systems, ticket tracking, branching, version control






## Individual discussions
* A link to, and discussion of, code you consider good
* A link to, and discussion of, code you consider bad
* A personal reflection about the key concepts you learnt during the course
* Code involvement


For code involvement the interaction rules are:
Anyone can claim to have “Touched” code. This would involve as little as single line debugging. You would only mention this if the input was significant for the project, but very small in actual change in code.
If you claim you did “All” of a part of code no one else can claim “Most”, “Half”, or “Some”, any group member can claim they touched the code.
If you claim you did “Most” then others cannot claim “All”, “Most”, or “Half” but up to half the group (N/2) can claim “Some” work.
If you claim “Half” then others cannot claim “All” or “Most”, but one (1) other can claim “Half” and any number of the group (N) can claim “Some” or “Touched” Others



Work distribution matrix

|                                         | Viktor | Ole     | Magnus | Mateusz | Julian | Berkay |
|-----------------------------------------|--------|---------|--------|---------|--------|--------|
| Procedural generation                   |        | All     |        |         |        |        | 
| Day-night cycle                         |        |         | All    |         |        |        | 
| Inventory                               |        | Touched |        | All     |        |        | 
| Wood cutting / object destruction logic |        | All     |        |         |        |        | 
| Chunk objects                           |        | All     |        |         |        |        | 
| Lakes                                   |        | All     |        |         |        |        | 
| Caves                                   |        | Half    | Half   |         |        |        |
| Animals                                 |        |         |        |         |        | All    |
| Enemies                                 |        |         |        |         |        |        |
| Input system                            |        |         |        |         | All    |        |
| Main menu                               |        |         | Some   |         | Most   |        | 
| Pause menu                              |        |         | All    |         |        |        | 
| health system                           |        |         | All    |         |        |        |




### Gameplay
The game is a  first person 3D exploration game, where the player explores a changing world, gathers resources, and fights enemies along the way. The world is procedurally generated with a forest biome, and other minor biomes that change the color of ground. There are lakes, vegetation and cave entrances to explore. The caves are static levels filled with enemies.

There are 3 types of enemies in the game and 1 boss encounter. Some enemies use borrowed assets while the Squidman is using a self made animation and model.

The player has 3 types of equipment, a sword, a knife and an axe. The sword and knife are used for combat purposes as the world is filled with different enemies. The axe is used for collecting lumber, storing it in the player inventory.

The inventory is used to store items, mainly the resources gathered from the world. There is also an equipment bar where you can drag items from your inventory. The functionality for using the items on the equipment bar is not yet implemented.

### Teamwork And Development process

For this project we used the Agile development method SCRUM. We organized the work with sprints lasting 2 weeks, with a sprint review at the end of each cycle. The different tasks were defined and distributed using a kanban/SCRUM board where each member assigns themselves a card with a task, and places it according to their current progress. An example of a sprint can be seen here: [SCRUM board](https://github.com/oleelnes/Sandbox/blob/master/Reports/images/terllo_board.png?raw=true) We planned one 1 meeting a week to discuss our progress, 1 online coding session from 10:00 to 13:00 on Wednesdays, and a sprint review on Monday every two weeks. The sprint reviews were physical if possible, so the team had the opportunity to meet each other. 

One of the benefits of working online is not everyone in the group could be physically present a majority of the time. We tried to have the non code related meetings happen in person and on campus, but as we closed in on exam season this proved more difficult. People's schedules were conflicting, as more time was dedicated to other subjects, and rooms at campus became more difficult to book. By moving the meetings online more people were able to participate. 

Another benefit was pair programming, as it is easy to share your screen to get input from another developer. This way you can collaborate and get another pair of eyes on your code. The drawback was the project became more disconnected by not having group members see each other for large periods of time. It is difficult to bounce ideas off each other in an online setting, and there is a different atmosphere when you can talk with other people during a break. Some of the interpersonal relationships are lost when only interacting through a screen. 

When initially pitching the game we had a very broad game design with a lot of different features. None of the developers had created a video game before and set too ambitious goals. Everyone had different aspects they wanted to work on, and with such a large scope we lost sight of what the game was supposed to be. The result of this was a demo with a small amount of gameplay, but aspects of several bigger features. A clear vision beyond a 3D procedurally generated world was not properly established during development. 

We did a good job delegating tasks and working individually, but a consequence of this was a lack of cohesion when it came to development. Different group members worked on different features for the game without cooperating. This caused problems further down the line, as the gameplay elements overlapped. A better approach would be more cooperation when developing core parts of the game, so everyone knew how key features worked. An example was Ole working on the procedural generation, and other members needed to place items in the world. To solve this he made a short tutorial of how this was done. This was a good solution to bring everyone up to speed, but could have been avoided if we collaborated more.

Our team has been working together on this project for several months now, and we have come a long way. We started out with just a rough idea and now we have a fully functional video game. It has not been an easy journey, but we have persevered and learned a lot along the way. Overall, our team has been a great success. We have learned a lot, we have created something we are proud of, and we have had fun along the way.












| | # claim | Others | All	| Most | Half | Some | Touched |
|------|------|--------|----|-----|-----|-----|-----|
| All	| 1 |        | 0      | 0 | 0 | 0 | N |
| Most | 1 |        | 0      | 0 | 0 | N | N |
| Half | 2 |        | 0      | 0 | 1 | N | N |
| Some | N |        | 0      | 1 | 1 | N | N |
| Touched | N |        | 1      | 1 | 2 | N | N |

An example of doing this individually:

| File | Claim |
|-----|-----|
|file.cs|All|
|prefab.prefab|Most|
|file2.cs|Most|
|file3.cs|Half|
|file3.cs|Touched|

Or we could have one table for everyone, like this:

| File | Ola Nordmann   | Jane Doe   | Kari Nordmann  |
|------|----------------|------------|----------------|
| file1.cs | Most           | Some       | Touched        |
| Prefab.prefab | Touched        | All        | -              |
| file2.cs | Half           | Half       | Touched        |
| file2.cs | -              | Most       | Some           |
