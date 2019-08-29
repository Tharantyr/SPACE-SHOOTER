# Super Powerful Awesome Crazy Epic SHOOTER

Warning: high difficulty, probably a little unbalanced because of the short window of time to complete it

Controls:
- WASD to move the ship
- Mouse movement to rotate
- Left mouse button to fire bullets
- Right mouse button to fire rockets
- Press Escape to open the pause menu

Gameplay: Survive each wave of 1 minute and buy upgrades at the shop in between. Enemies drop orbs that are used as a currency.

Enemies:
- Normal enemy - is stupid and shoots at literally anyone in sight (closest object)
- Asteroid - Fly randomly into the level. Deals big damage and hard to destroy.
- Chaser enemies - annoying little guys that will chase you and blow up when they connect
- Charger enemies - Bigger chasers that charge at you and deal devastating damage
- Laser enemies - Hide in corners to shoot lasers at you. Luckily the lasers damage all other enemies too.

Most important scripts:

- GameSystem that manages the entire game and transitions between different states (MainMenu, Waves, Shop, Highscores, etc.)
- EnemySpawner generates each enemy wave. Wave generation is a mix of random position placement in the level combined with fixed times at which enemies spawn. However, the system has been designed to allow for the combination of waves to create new and more complex waves. The first 10 waves have been mostly manually created, after that it's a random combination of different waves.
- ObjectPool is used to create a pool of deactivated objects that can be activated when they are needed instead of calling Instantiate() and Destroy() constantly, which can become computationally expensive with a high number of objects in the scene. The pool dynamically expands and adds new objects whenever necessary.
- Shop allows the player to buy upgrades (increase rockets (max 10), restock rockets, restock lives (max 3), decrease shield recharge time (min 3), increase shield (max 100)). Shop prices double after each buy.
- Highscores allow the player to enter a name and record their high score on the high score list

All sound effects designed by me.
Music: https://www.youtube.com/watch?v=nFY5zzdjdQs

Sprites taken from Roche Fusion (game I worked on previously) and the Internet, then converted to pixel art using Pixelator.
