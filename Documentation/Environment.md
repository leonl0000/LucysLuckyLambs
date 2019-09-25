# Environment

The <a href='https://github.com/leonl0000/LucysLuckyLambs/tree/master/prototypes/Quick%20Prototype/Assets/Scripts/Environmental%20Scripts'>Environmental Scripts folder</a> contains scripts that manage the environment of the game. This includes the scene manager, as well as inanimate objects in the scene, such as the spawn gate and the hell gate.

## Scene managers

The <a href='https://github.com/leonl0000/LucysLuckyLambs/blob/master/prototypes/Quick%20Prototype/Assets/Scripts/Environmental%20Scripts/hellSceneManager.cs'>scene manager script</a> is the biggest script in the game, responsible for managing the scene. Its responsibilities include:

* Tracking the player's current mana level and health level. These are displayed on the interface by the <a href='https://github.com/leonl0000/LucysLuckyLambs/blob/master/prototypes/Quick%20Prototype/Assets/Scripts/UI%20Scripts/scoreScript.cs'>score script</a>, and are modified by various actions such as casting spells or taking damage.
* Maintaining dictionaries of all sheep, all lures, and all angels in the current scene. These are used by sheep to compute their movement, and by the AI for wolves and angels when they want to chase sheep. Angels also need to find nearby angels as part of their attack state, necessitating a dictionary of angels. Objects are added to the dictionaries when they are created, by calling a registration function (`registerSheep` in the case of sheep) or by the `Start` functions of angels, or by the lure spell in the case of lures.
* Casting spells when the number keys are pressed. This is done by invoking the appropriate function from the  <a href='https://github.com/leonl0000/LucysLuckyLambs/blob/master/prototypes/Quick%20Prototype/Assets/Scripts/Player%20Scripts/Abilities.cs'>player abilities script</a>.
* Removing sheep that fall off the map, collide with the hell gate, or are eaten by wolves, as well as spawning sheep at the spawn gate.
* Computing sheeps' movement goals, as described in <a href="https://github.com/leonl0000/LucysLuckyLambs/blob/master/Documentation/NPCs.md">the documentation on NPCs</a>.
* Loading the next level once the current level is won.
* Loading saved games, as described in <a href="https://github.com/leonl0000/LucysLuckyLambs/blob/master/Documentation/System.md">the documentation on system scripts</a>.


## Inanimate objects

There are three environmental scripts used to control various inanimate parts of the environment.

The <a href='https://github.com/leonl0000/LucysLuckyLambs/blob/master/prototypes/Quick%20Prototype/Assets/Scripts/Environmental%20Scripts/CollisionManager_Ground.cs'>ground collision manager script</a> handles collisions between the player and the ground, which is necessary to reset the number of jumps the player is allowed to make.

The <a href='https://github.com/leonl0000/LucysLuckyLambs/blob/master/prototypes/Quick%20Prototype/Assets/Scripts/Environmental%20Scripts/hellgateScript.cs'>hell gate script</a> controls the hell gate, interfacing with the scene manager to remove sheep that collide with the hell gate and update the number of sheep sacrificed.

The <a href='https://github.com/leonl0000/LucysLuckyLambs/blob/master/prototypes/Quick%20Prototype/Assets/Scripts/Environmental%20Scripts/spawnGateScript.cs'>spawn gate script</a> controls the spawn gate, which originally periodically spawned sheep but no longer does this because we now spawn sheep using spells.
