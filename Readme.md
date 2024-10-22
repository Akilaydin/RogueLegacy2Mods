#### Mods for the game Rogue Legacy 2

These mods are designed to improve gameplay by increasing rewards from chests and reducing grind by adjusting soul stone drops.

#### Links to the mods pages

- [Rich Chests mod CHANGE](https://www.nexusmods.com/roguelegacy2/mods/99999)
- [Soul Stones for All mod CHANGE](https://www.nexusmods.com/roguelegacy2/mods/99999)

#### Install

To install these mods, first install BepInEx for Rogue Legacy 2, then place the `.dll` files in the `BepInEx\plugins` folder.

You can use [this tool](https://www.nexusmods.com/site/mods/287) to install BepInEx.

#### Build

To build the mods, you need to set up BepInEx on your machine, and then run `dotnet build` in the mod folder.

You may need to modify the `GamePath` in `solution_private.targets` to match your local game directory to build the mods correctly.

Once built, the mods will be automatically copied to your game directory.

#### Information

These mods have been made possible using [Wobat's mods](https://github.com/wobatt/RogueLegacy2Mods) source code, and both mods use Wob_Common from that repository.