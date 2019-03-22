# System

The <a href='https://github.com/StanfordCS194/EmuGames/tree/master/prototypes/Quick%20Prototype/Assets/Scripts/System%20Scripts'>system scripts</a> control the saving and loading of games.

The <a href='https://github.com/StanfordCS194/EmuGames/blob/master/prototypes/Quick%20Prototype/Assets/Scripts/System%20Scripts/SaveData.cs'>Save Data</a> script is responsible for serializing the current state of the game. It defines a serializable class `SaveData` which has members containing the stored data representing the scene, as well as a `SaveData` constructor which initializes these data members with the required data from the game.

The <a href='https://github.com/StanfordCS194/EmuGames/blob/master/prototypes/Quick%20Prototype/Assets/Scripts/System%20Scripts/SaveSystem.cs'>Save System</a> script manages the system used for saving and loading games. It defines `SaveGame` and `LoadGame` functions, which write or read the save files. The game uses a system of save slots, allowing for a certain number of slots in which games can be saved or from which they are loaded, using the `SaveData` class described above.

Additional code for saving and loading is in the <a href='https://github.com/StanfordCS194/EmuGames/blob/master/prototypes/Quick%20Prototype/Assets/Scripts/Environmental%20Scripts/hellSceneManager.cs'>scene manager script</a>. This script defines a `save` function which simply invokes the code from the Save System script. It also defines the `load` function, which sets the current scene's values using the saved serial data when a game is loaded. We chose to place this code in the scene manager script, because the scene manager script is in charge of tracking various variables global to the game, such as the player's current mana and health levels.
