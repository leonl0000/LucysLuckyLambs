# Story and dialogue

The <a href='https://github.com/StanfordCS194/EmuGames/tree/master/prototypes/Quick%20Prototype/Assets/Scripts/DialogueScripts'>Dialogue Scripts</a> folder contains three scripts used to manage the dialogue system, which we have used to create our tutorial and storyline.

The <a href='https://github.com/StanfordCS194/EmuGames/blob/master/prototypes/Quick%20Prototype/Assets/Scripts/DialogueScripts/DialogueBoxScript.cs'>dialogue box script</a> manages boxes containing dialogue, allowing them to display a line of dialogue or story as well as a list of several responses, with a scrollbar. The player can choose between the different responses listed, which will cause the dialogue box to update to display the next line of dialogue, chosen according to the player's response.

The <a href='https://github.com/StanfordCS194/EmuGames/blob/master/prototypes/Quick%20Prototype/Assets/Scripts/DialogueScripts/Page.cs'>page script</a> defines a serializable `Page` class which represents one page of dialogue, including its dialogue text and the available responses. The dialogue box displays one page at a time, switching between pages according to responses.

The <a href='https://github.com/StanfordCS194/EmuGames/blob/master/prototypes/Quick%20Prototype/Assets/Scripts/DialogueScripts/TutorialConstructor.cs'>tutorial constructor script</a> is used to initiate a sequence of dialogue. It creates a dialogue box, and handles clicks on the response buttons.
