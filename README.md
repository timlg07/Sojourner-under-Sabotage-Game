# Sojourner under Sabotage - Game

This repository contains the Unity part of the serious game _Sojourner under Sabotage_. 
The Unity export is designed to be deployed as a part of the web application developed in the Sojourner-under-Sabotage-Server repository.

## Licensed Assets from the Asset Store

The game makes use of several packages and assets to improve the development workflow. Two of these assets are purchased from the Unity Asset Store:  

 - OneJS by DragonGround LLC (https://assetstore.unity.com/packages/tools/gui/onejs-221317)
 - RPG Map Editor by Creative Spore (https://assetstore.unity.com/packages/tools/game-toolkits/rpg-map-editor-25657)
 
These assets are not included in the public repository due to their licensing. 
To extend the game further and build the Unity export yourself, you need to purchase a proper license for the two packages and place them in the `Assets`-directory.

## OneJS

To edit the Preact/TypeScript part of the game, the OneJS asset is used.  

 1. Open the `/OneJS` directory in Visual Studio Code. _Important:_ not the `/Assets/OneJS` folder, but the one in the repository root.
 2. Use `Ctrl + Shift + B` to start up the watch tasks (`tsc: watch`).
 3. Now all changed files of the OneJS part of the game are automatically recompiled and reloaded into Unity.

You can learn more about OneJS here: https://onejs.com/docs/