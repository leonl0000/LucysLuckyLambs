# NPCs 

The scripts controlling the behavior of NPCs and related objects are in the <a href='https://github.com/leonl0000/LucysLuckyLambs/tree/master/prototypes/Quick%20Prototype/Assets/Scripts/NPC%20Scripts'>NPC Scripts</a> folder. There are three main NPCs, described below, as well as some auxiliary helper scripts.

## Helper scripts
The <a href='https://github.com/leonl0000/LucysLuckyLambs/blob/master/prototypes/Quick%20Prototype/Assets/Scripts/NPC%20Scripts/HealthScript.cs'>health script</a> is responsible for giving NPCs health, which can be reduced by damaging-causing objects such as fireballs or lightning. This script is attached to all of the NPCs described below, and is generally initiated from each NPC's main controlling script using the AddHealthScript function. This function can be given a WoundAction argument and a DeathFunction argument, which allow NPCs to have customized behavior when they die or are wounded, such as blood splatter particle effects or the conversion of angels into dead angels. The script also instantiates health bars for NPCs that are wounded, displaying the remaining health of the NPC.

The second helper script is the <a href='https://github.com/leonl0000/LucysLuckyLambs/blob/master/prototypes/Quick%20Prototype/Assets/Scripts/NPC%20Scripts/despawnTimer.cs'>despawn timer</a> script. This is a simple script which causes an object to be deleted after a given period of time. The script is attached to fireballs and angel bolts, as well as particle effects such as blood splatters and explosions, to prevent them from cluttering the game tree.

## Sheep

Sheep are controlled primarily by the <a href='https://github.com/leonl0000/LucysLuckyLambs/blob/master/prototypes/Quick%20Prototype/Assets/Scripts/NPC%20Scripts/sheepScript.cs'>sheep script</a>. This script has several different aspects. On creation, sheep are registered in a global `sheepDict` stored in the game manager, and given a health script as described above.

Sheep movement is somewhat complicated, following boid-like rules to create swarming behavior. Sheep move in a series of hops. After each hop, a timer is randomly set up, and when the timer finishes the sheep will perform its next hop. Sheep will hop in the direction of their `goal`, which is a destination that the sheep tries to move towards. The goal is calculated primarily not in the sheep script but in the <a href='https://github.com/leonl0000/LucysLuckyLambs/blob/master/prototypes/Quick%20Prototype/Assets/Scripts/Environmental%20Scripts/hellSceneManager.cs'>game manager script</a>, in the `getSheepGoal` function. This is because the game manager has access to the dictionaries containing all sheep and all lures, as well as the player's position, which are used together to determine how sheep move, as will be explained below.

The `getSheepGoal` function computes a sheep's goal as a weighted average of several factors. First, sheep tend to move towards other nearby sheep, according to the boid coherence rule. Second, sheep tend to move away from sheep that are too close to them, according to the boid separation rule. Third, sheep try to face in the same direction as the sheep close to them, according to the boid alignment rule.

These three rules are the classical boid rules, to which we have added some other forces for better gameplay. Sheep tend to move towards the player, and towards nearby lures. A random factor is also added to make their movements less robotic. Currently, the sheep do not avoid hostile NPCs such as wolves, because sheep are stupid.

Sheeps' goals are recomputed after every hop. This is better than computing them in every frame, because the `getSheepGoal` computations are somewhat costly. Sheep will turn to face the direction of their goal, giving the player some cues about where their sheep will head next. Sheeps' true goal is actually a weighted average of their previous goal and the output of a call to `getSheepGoal`, so essentially their goals are a geometrically-weighted moving average of recent outputs of `getSheepGoal`; this is to prevent sheep from turning rapidly and moving erratically.

## Wolf

The wolf attempts to chase and eat the sheep, and is controlled by the <a href='https://github.com/leonl0000/LucysLuckyLambs/blob/master/prototypes/Quick%20Prototype/Assets/Scripts/NPC%20Scripts/PredatorScript.cs'>predator script</a>. Wolf AI works using three main timers: a chase timer, a direction timer, and a terrain timer.

Initially, a wolf will try to select one nearby sheep as its prey, by calling the `getNewPrey` function. If there are no nearby sheep, the wolf will lie in wait until the player and their flock of sheep come near him. Once a prey is acquired, the chase timer and the direction timer will start counting down. The wolf will start moving towards the prey; every time the direction timer reaches zero, the wolf updates the direction of its approach and resets the direction timer.

When the chase timer reaches zero, the wolf will give up on its current prey and attempt to acquire a new sheep as its prey. This is essential because wolves do not pathfind but simply approach their target, and therefore will stay against walls if their target sheep is on the other side.

The terrain timer is used to prevent wolves from getting stuck on terrain. Whenever a wolf's position is almost identical between two updates, the terrain timer will count down; otherwise, if the wolf moved, the terrain timer will be reset. When the terrain timer has counted down for long enough, the wolf will conclude that it has gotten stuck and attempt to jump, so that it can resume chasing sheep.

## Angels

Angels are controlled by a finite state machine (FSM), implemented in the <a href='https://github.com/leonl0000/LucysLuckyLambs/blob/master/prototypes/Quick%20Prototype/Assets/Scripts/NPC%20Scripts/angelScript.cs'>angel script</a> and illustrated here:

<img src='https://github.com/leonl0000/LucysLuckyLambs/blob/master/Documentation/AngelFSM.png' />

As the diagram shows, there are four main states an angel can be in: chasing sheep, abducting sheep, attacking the player, or drifting randomly. Initially, the angel randomly chooses to chase, drift, or attack. If it chooses to chase sheep, but there are no nearby sheep, or if it decides to attack but the player is not nearby, it will default to drifting randomly.

When drifting randomly, the angel picks a random direction to float in, and biases this slightly towards its spawn point, stored in the angel's start function. This prevents the angels from wandering too far from their initial location, at least when drifting randomly. After a period of random drifting, the angel will move to another random state.

When chasing sheep, an angel will pick a random nearby sheep. They will then compute a point above the sheep, and drift towards it. Angels do not have any pathfinding abilities, but since they float above the ground this is not generally a problem. Nevertheless, we have added a random factor to make angel movements less robotic and prevent them from getting stuck trying to move through hills or trees. When an angel starts chasing, a chase timer will count down, and if it reaches zero the angel will abandon the chase and choose a random next state.

If, while chasing sheep, the angel's distance to its target sheep drops below some threshold, it will start the abduction process. When abducting sheep, a line will be drawn to the sheep to alert the player, and the sheep will move into the sky, towards the angel, along this line. When the sheep is close enough to the angel, it will be abducted, vanishing with a particle effect. This abduction process was inspired by an earlier idea we had to have UFOs as one of our NPCs, since UFOs supposedly abduct livestock. After more brainstorming, the game took on some religious themes, so we decided to make angels instead of UFOs.

If the player does any damage to an angel while it is drifting, chasing, or abducting, the angel and all angels close to it will instantly switch to attacking the player. When attacking, the angel picks a point above the player, plus a random factor to reduce predictability, and then move towards this point. The point is re-chosen periodically to create zig-zagging movement which makes it difficult to hit angels with fireballs. After attacking for a certain period, the angel will switch to a random state.

When an angel is attacking the player, once the player is close enough, the angel will periodically fire "angel bolts" at the player. These are equipped with an <a href='https://github.com/leonl0000/LucysLuckyLambs/blob/master/prototypes/Quick%20Prototype/Assets/Scripts/NPC%20Scripts/angelBoltScript.cs'>angel bolt script</a> which allows them to cause damage to the player and other NPCs, as well as a despawn timer script (described above) so that they will vanish after a certain period.
