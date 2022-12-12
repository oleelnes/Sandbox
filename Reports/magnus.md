### Score weighting
|Description | my weight |
|----|----|
|Gameplay video | 10 |
|Code video | 5 |
|Good Code  | 20 |
|Bad Code | 15 |
|Development process | 30 |
|Reflection | 30 |


### Point Distribution: 
The reason for this distribution is I did not personally make the videos, but I had input in content and structure. I was not too happy with my code and felt my contributions to the development process were better.

### Development Process: 
I took the role of wolf for this project, ensuring that everyone got their work done. I feel I have completed this role well, as I have mostly been in charge of setting up meetings, planning out sprints, and dealing with group issues (lack of contribution from some members). I have experience working on bigger school projects like this, and tried to make everything be on schedule.

We used agile development for this project, with two week long sprints. Viktor was the dedicated SCRUM master, but the roles were more blurred as the project progressed. He knew about the SCRUM boards and had experience with it. I showed how to conduct code reviews and sprint reviews, which Viktor then led.

In addition I have taught the group good practice for using GiT version control. A few examples are not merging branches without another developer doing a quick code review, deleting branches when the feature is implemented, dealing with merge conflicts, and better naming conventions. Initially the other devs coded on 1 dedicated branch named after themselves and periodically merged with the main branch. I organized it with a trunk branch setup in mind, with one master branch that is continuously updated when a feature is complete. We also encountered a lot of issues when working on the same scene on different machines in Unity. I helped, as some of the group had never resolved a merge conflict before. After that I set up the system of copying a scene, and then manually moving it over to the main scene after a pull request was merged.

### Good Code
One contribution I was happy with was the health system. It is dynamic, where you can increase or decrease the amount of health the player has, as well as having half hearts. I made the sprite myself using pixel art, and had 3 versions. A full, half and empty heart. The numbers are stored in an enum that helps decide which heart needs to be drawn.

![](https://github.com/oleelnes/Sandbox/blob/master/Reports/images/magnus/heart_image_magnus.png?raw=true)

This function clears all the hearts and finds out the size of the health bar, and what hearts to draw depending on the current health of the player.

![](https://github.com/oleelnes/Sandbox/blob/master/Reports/images/magnus/draw_hearts_magnus.png?raw=true)

Another aspect I was happy with was how the menus worked out. Most of the logic was made in the Unity engine, where I used events to program what the different buttons do. Then there were some simple scripts for controlling the menu. We had problems with unlocking and locking the cursor when in a menu. Some of the inventory code overlapped, as the cursor is unlocked when the inventory is open. A quick fix was using the inventory boolean in the pause menu as well. This is not very advanced, but I am happy with how it turned out.

![](https://github.com/oleelnes/Sandbox/blob/master/Reports/images/magnus/pause_menu_magnus.png?raw=true)

### Bad Code
One of the features I worked on but did not make it into the main game was a simple loot system for the enemies. The reason I put this in bad code is it does not work, but the logic seems correct to me. This is a lootbag script attached to an enemy that gets called when the enemy dies. There is a list of loot items which has a drop chance. When the enemy dies a random number is generated, and if it is larger than the drop chance the item is added to another list. Then an item number from the new list is chosen and dropped from the dead enemy. The problem comes when I try to instantiate the object. I have created a 3D prefab of a gem, and it is this object I am trying to instantiate. The bad part is me not understanding the proper logic to spawn in an object.

This is a system I tried to adapt for our game based on someone else's implementation. This is following a trend I saw when working on the project. I find myself relying a lot on tutorials on how to program in Unity. I generally try to avoid this, as there is no guarantee that a programmer on youtube is following good coding conventions. I prefer going to the documentation and experimenting from there. I found this difficult to do when working with a game engine, as it is a bit different from other programming platforms. Not being able to adapt something to fit my own project shows a lack of understanding how the engine/code works.

![](https://github.com/oleelnes/Sandbox/blob/master/Reports/images/magnus/loot_magnus.png?raw=true)

### Reflection
Overall I am not too happy with the end project, but not too disappointed either. I feel my code contribution should have been larger, but also that I have had a positive contribution to the group. The biggest lesson I learned was how to coordinate a project with so many members, where most of the work was done online. This proved more challenging than anticipated.

Finding a time to work together with everyone having conflicting schedules was very difficult, and I am a person that thrives on teamwork. Sitting alone in my room coding without any input from my group was not a great experience. Usually I have worked with people I am somewhat familiar with, but everyone in this group was a stranger. This made it more difficult for me, as I found it harder to ask for help outside the scheduled coding sessions. This is an experience I will learn from, as it is likely working from home can be a requirement in a future development job. What I found helped alleviate this was the development methods I had learned during my degree. It is good to be able to lean on established processes.

Early on the other group members wanted to make a procedurally generated game, which was something I was against. I felt it was too complicated and not a great use of time. However, everyone else wanted it and I agreed. I chose to focus on aspects that had a more general use for different games such as: Health system, menus, loot system. In the end I feel we could have made a much more cohesive and polished product if we chose an easier project. The procedural generation took a lot of dev time and it being 3D also created a lot of complications. I think it was a poor choice for our first game. 

Several things I worked on did not end up in the game. This was mostly due to poor planning on my part and the lack of direction our game had. In the future I will set more defined goals that are easier to work towards. Some parts did not fit with the rest of the game, or I underestimated the time it would take to develop, and was not able to finish. I created the cave level we use in the game today. I also made another bigger level, with maze like architecture, which I intended to fill with small puzzles and traps, but only finished the cave layout. Then the deadline was approaching and other subjects demanded attention for exams.I did not like using a game engine, and do not think I will continue with game programming in the future.

In evaluating my fellow students I am generally very happy. Ole has really gone above what was expected and has been working hard throughout the entire project. If anyone deserves a better grade than the rest it is him. Viktor works hard but is bad at communicating. He suddenly shows up having done a lot without communicating, still what he produced was very impressive and I cannot complain about his work ethic or results.

