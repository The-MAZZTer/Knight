# Knight

This tool is intended to help manage mods and patches for the Dark Forces / Jedi Knight series of games.

The code is licensed under the MIT license. Feel free to take the code and learn from it and use it in your own projects.

Don't take too much though, a lot of this code is old and bad. :)

## Dependencies

Uses Google Material Symbols, MIT licensed.

## Quick Start

Run the app, your games should be detected automatically if you have them installed on Steam.

If not, use the drop down arrows to the right of each game name to find the Locate button to browse for the game.

Mods should automatically populate. See sections for each game below for details.

You can use the drop down to access other options such as the Options dialog for each game where you can configure how it is run.

Some games have checkboxes next to the mods, you can select multiple mods to run this way.

Otherwise select a mod by clicking it.

Click the Play button in the game header when you want to run your selection.

Some of these games are quite old and it's recommended to reference [PC Gaming Wiki](https://www.pcgamingwiki.com/) to get these games working properly on modern systems.

## Dark Forces

Dark Forces can be run using the original release, Steam rerelease, or remaster.

Note that the 3/18/2024 build or newer of the remaster is required for many mods to work properly. As of this writing that version is in private beta. The public build is hit-or-miss with mods.

Dark Forces supports mods in folders or ZIPs.

It is recommended you create a Levels folder in Dark Forces. Each subfolder or ZIP is expected to be its own mod. The name of each must match the name of the GOB file for the mod.

Knight does not use the BAT files that come with many mods so you will need to organize the files yourself to use them with Knight. :(

Mods tend to have their own DFBRIEF.LFD or FTEXTCRA.LFD files renamed. Knight will handle these files properly. Other files will need to be manually renamed to their intended name to work with Knight. The BAT file included with the mod can show how these files need to be renamed.

For example Assassinate Dark Vader (KILVADER) mod contains the following files:

* KILVADER.GOB
* KILVADER.LFD
* KILVADER.BAT
* KILVADER.TXT
* VADCUT.LFD
* VADCUT.LST
* VADCUT.TXT
* VADCUT1.GMD
* VADCUT2.GMD
* VADCUT3.GMD
* VADCUT4.GMD
* VADCUT5.GMD

To use this mod with Knight, you must rename the extra files according to the BAT file. Striked out files are not required (but do not have to be removed either).

* Levels\KILVADER\KILVADER.GOB
* Levels\KILVADER\KILVADER.LFD
* ~~Levels\KILVADER\KILVADER.BAT~~
* ~~Levels\KILVADER\KILVADER.TXT~~
* Levels\KILVADER\VADCUT.LFD
* Levels\KILVADER\CUTSCENE.LST
* ~~Levels\KILVADER\VADCUT.TXT~~
* Levels\KILVADER\GROMAS1.GMD
* Levels\KILVADER\GROMAS2.GMD
* Levels\KILVADER\ROBOT1.GMD
* Levels\KILVADER\ROBOT2.GMD
* Levels\KILVADER\CLOSCRED.GMD

Knight already has support for this mod so it will work as-is. If it did not, you would see KILVADER show up in the mod list, and you would right click it and use Rename and Properties options to configure it.

Under Properties you would select KILVADER.LFD as the DEBRIEF.LFD file. the other LFD, the LST, and the GMD files should all be selected under "Other" as they are a part of the mod as well.

Now this mod will work with Knight.

## Jedi Knight / Mysteries of the Sith

Jedi Knight / Mysteries of the Sith support mirrors the structure of Jedi Knight Patch Commander. Simply put mods in the JKPatch folder and they will show up in Knight.

You can also put patched EXEs in the patches folder. You can then swap them out using Knight.

My jkversions tool also available on GitHub will generate a few patches for you.

JK and MotS seem to no longer run on modern Windows systems even when using -windowgui option. I tried to impllement JkGfxMod and OpenJKDF2 support, but I could not get the former to work properly, and the latter does not seem to work properly with the -path command line argument. So for the time being support for those has not been enabled.

## Jedi Outcast / Jedi Academy

Mod support is provided by the games themselves. Knight simply acts as a launcher. Install your mods as normal.
