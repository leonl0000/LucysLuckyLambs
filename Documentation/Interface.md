# Interface

The <a href='https://github.com/StanfordCS194/EmuGames/tree/master/prototypes/Quick%20Prototype/Assets/Scripts/UI%20Scripts'>UI Scripts</a> folder contains scripts used to manage the user interface. This includes both the in-game HUD, as well as the out-of-game interface, including the pause menu and save/load interface.

The <a href='https://github.com/StanfordCS194/EmuGames/blob/master/prototypes/Quick%20Prototype/Assets/Scripts/UI%20Scripts/scoreScript.cs'>score script</a> is responsible for updating the in-game HUD, to display the player's health and mana, and the number of sheep that have been eaten or killed.

The <a href='https://github.com/StanfordCS194/EmuGames/blob/master/prototypes/Quick%20Prototype/Assets/Scripts/UI%20Scripts/PauseMenuScript.cs'>pause menu script</a> controls the pause menu, which is entered by pressing the escape key. When the game is paused, it displays the pause menu and activates the save/load buttons; when the game is unpaused, it removes all of these.

The <a href='https://github.com/StanfordCS194/EmuGames/blob/master/prototypes/Quick%20Prototype/Assets/Scripts/UI%20Scripts/MenuManager.cs'>menu manager script</a> manages the menus displayed when the game is started. It shows the initial welcome page, the first "letter from Lucifer", and loads the first level.

The <a href='https://github.com/StanfordCS194/EmuGames/blob/master/prototypes/Quick%20Prototype/Assets/MinimapScript.cs'>minimap script</a> ensures the texture render minimap at the top right of the UI follows the player's position and z-y orientation (their "look" direction" so the map is oriented as the player is).

TODO update this once we have mana and health bars
