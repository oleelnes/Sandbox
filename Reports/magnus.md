### Point Distribution: 
The reason for this distribution is I did not personally make the videos, but I had input in content and structure. I was not too happy with my code and felt my contributions to the development process were better.

### Development Process: 
I took the role of wolf for this project, ensuring that everyone got their work done. I feel I have completed this role well, as I have mostly been in charge of setting up meetings, planning out sprints, and dealing with group issues (lack of contribution from some members). I have experience working on bigger school projects like this, and tried to make everything be on schedule.

We used agile development for this project, with two week long sprints. Viktor was the dedicated SCRUM master, but the roles were more blurred as the project progressed. He knew about the SCRUM boards and had experience with it. I showed how to conduct code reviews and sprint reviews, which Viktor then led.

In addition I have taught the group good practice for using GiT version control. A few examples are not merging branches without another developer doing a quick code review, deleting branches when the feature is implemented, dealing with merge conflicts, and better naming conventions. Initially the other devs coded on 1 dedicated branch named after themselves and periodically merged with the main branch. I organized it with a trunk branch setup in mind, with one master branch that is continuously updated when a feature is complete. We also encountered a lot of issues when working on the same scene on different machines in Unity. I helped, as some of the group had never resolved a merge conflict before. After that I set up the system of copying a scene, and then manually moving it over to the main scene after a pull request was merged.

### Good Code
One contribution I was happy with was the health system. It is dynamic, where you can increase or decrease the amount of health the player has, as well as having half hearts. I made the sprite myself using pixel art, and had 3 versions. A full, half and empty heart. The numbers are stored in an enum that helps decide which heart needs to be drawn.

![](https://github.com/oleelnes/Sandbox/blob/master/Reports/images/magnus/draw_hearts_magnus.png=raw=true)
