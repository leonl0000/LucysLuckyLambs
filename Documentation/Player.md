# The Player

The scripts used for controlling the player and their abilities are stored in the <a href='https://github.com/leonl0000/LucysLuckyLambs/tree/master/prototypes/Quick%20Prototype/Assets/Scripts/Player%20Scripts'>Player Scripts folder</a>. There are five player scripts, described below.

## Movement
The <a href='https://github.com/leonl0000/LucysLuckyLambs/blob/master/prototypes/Quick%20Prototype/Assets/Scripts/Player%20Scripts/PlayerMovement.cs'>movement script</a> handles all incoming keypresses. These include the arrow keys for movement, the panning keys Q and E, the space bar for jumping, as well as the number keys for the various spells. The spell keys cause the invocation of functions from the abilities script, described below.

## Abilities
The <a href='https://github.com/leonl0000/LucysLuckyLambs/blob/master/prototypes/Quick%20Prototype/Assets/Scripts/Player%20Scripts/Abilities.cs'>abilities script</a> contains functions for each of the spells. These include lures, fireballs, wall creation, and lightning.

## Camera-following
The <a href='https://github.com/leonl0000/LucysLuckyLambs/blob/master/prototypes/Quick%20Prototype/Assets/Scripts/Player%20Scripts/FollowPlayer.cs'>player-following script FollowPlayer</a> is responsible for ensuring that the camera follows the player's movements. The camera is placed in a position above and behind the player, looking at a point some distance in front of the player.

## Lures
The <a href='https://github.com/leonl0000/LucysLuckyLambs/blob/master/prototypes/Quick%20Prototype/Assets/Scripts/Player%20Scripts/LureScript.cs'>lure script</a> controls lure objects after they have been created; their creation is controlled by the abilities script, described above. This script destroys lures after their lifetime is over, and updates their transparency to indicate their remaining life.

## Fireballs
The <a href='https://github.com/leonl0000/LucysLuckyLambs/blob/master/prototypes/Quick%20Prototype/Assets/Scripts/Player%20Scripts/FireballScript.cs'>fireball script</a> controls the movements of fireballs after they have been created; their creation is controlled by the abilities script described above. This script causes fireballs to generate explosions and cause damage upon collision, scaled with the fireball's size. It also ensures fireballs disappear after some time, so that fireballs shot into the sky will not accumulate and clutter up the object tree.

## Walls
Contained within the <a href='https://github.com/leonl0000/LucysLuckyLambs/blob/master/prototypes/Quick%20Prototype/Assets/Scripts/Player%20Scripts/Abilities.cs'>abilities script</a> and in conjunction with an additional <a href='https://github.com/leonl0000/LucysLuckyLambs/blob/master/prototypes/Quick%20Prototype/Assets/player_control.cs'>player control script</a>, a wall is spawned in front of the player. While the wall key is held down, the player can control where to place the wall using mouse movement. The wall prevents all ground entities from passing.
