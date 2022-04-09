# DeleteAllSaves
[![CodeFactor grade](https://img.shields.io/codefactor/grade/github/asdfcyber/deleteallsaves)](https://www.codefactor.io/repository/github/asdfcyber/deleteallsaves) [![Latest release](https://img.shields.io/github/v/release/asdfcyber/deleteallsaves)](https://github.com/asdfcyber/deleteallsaves/releases/latest) ![Platforms](https://img.shields.io/badge/platform-windows%20%7C%20macos%20%7C%20linux-blue) <br/><br/>


## Description
A mod for [Rail Route](https://railroute.bitrich.info/) to delete all saves per level, or all but the most recent one.


## How to install
Download and unpack the zip in the right directory depending on your operating system:

**Windows:** `%USERPROFILE%\AppData\LocalLow\bitrich\Rail Route\mods`  
**Linux:** `$HOME/.config/unity3d/bitrich/Rail Route/mods`  
**macOS:** `~/Library/Application Support/Rail Route/mods`

If you did it right, you should have the following file structure:
```
mods
|
| - DeleteAllSaves
|   |
|   | - DeleteAllSaves.dll and other files
```

You can uninstall the mod by deleting the DeleteAllSaves folder and its contents. This won't break your game or corrupt remaining saves. If you want to temporarily disable DeleteAllSaves without deleting anything for whatever reason, you can change the file extension of `DeleteAllSaves.dll` or move it somewhere else.<br/><br/>


## How to use
In the main menu, two new buttons are added to every level's additional options panel (accessible by clicking the dots in the top right corner of each level). The red eraser will delete all savefiles, and the orange eraser will delete all savefiles except for the most recent one. After clicking one of these you will be asked to confirm your decision, only then will files actually be deleted.<br/><br/>



## Reporting issues
If you find a bug, please start a new GitHub issue or message me on the Rail Route discord. Don't forget to send your `Player.log` file! You can find it one folder up from the mod folder. If you think you found a bug in Rail Route and you're using DeleteAllSaves, first remove/disable the mod to make sure it isn't actually a mod issue.
