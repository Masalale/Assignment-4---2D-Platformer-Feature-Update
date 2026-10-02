# 2D Platformer Feature Update

> Came into this project broken. The player couldn't move, the camera wouldn't follow, half the scripts were stubs. Took it apart, fixed what was wrong, finished the game.

## What was broken

The brief said to find the gaps, so that's what this is. Twelve of them.

| # | What was wrong | Fix |
| :-- | :--- | :--- |
| 1 | `myBody` was never assigned, so nothing could move | Assigned it in `Awake` |
| 2 | `Input.GetAxis("0")` and there is no axis called 0 | Set it to `"Horizontal"`, switched the project to Both input |
| 3 | `Update()` was empty | Called the grounded check and the jump |
| 4 | Jump was blocked by a bool that could never be true | Used `Input.GetButtonDown("Jump")` |
| 5 | Ground check sat inside the floor so the player was never seen as grounded | Moved it up and made the ray longer |
| 6 | Coins counted as ground, so you could jump in mid air | Filtered triggers out of the raycast |
| 7 | Camera had no target so it just sat there | Finds the player by tag |
| 8 | Snail and Beetle were checking a layer the player wasn't on | Added the player to their masks |
| 9 | Snail turned based on where it spawned, not where it was | Walls and side hits use body contact now |
| 10 | Dead spider and beetle still hurt you on the way down | Damage off once they die |
| 11 | Two scripts tried to load a scene called "Gameplay" that doesn't exist | Pointed them at the real one |
| 12 | The game scene wasn't in Build Settings, so builds launched nothing | Registered all three |

## How the game works

| System | Rule |
| :--- | :--- |
| Lives | 3. Water or an enemy costs one |
| Water | Touching the water under the surface puts you back on the bank near where you fell in, with a moment where you can't move. Skimming the top doesn't count |
| Snail | Stomp it to stun it, then touch the shell to kick it away. Or shoot it twice |
| Beetle | One shot. Once it's dead it can't hurt you |
| Spider | Drops down when shot. Harmless while it falls |
| Bird | Hangs above and drops eggs, shoot it out of the way |
| Boss | Three hits to put down |
| Bonus block | Hit it from below for a coin, then it's spent |
| Finish line | Get to the end of the level |
| Timer | Counts up from the start, stops when you pause |
| Pause | Esc for resume, restart, or back to the menu |

## Controls

| Key | Action |
| :--- | :--- |
| `A` / `D` or arrow keys | Move |
| `Space` | Jump |
| `J` | Shoot |
| `Esc` | Pause |

## Requirements

- Unity `6000.3.23f1`
- Unity Hub

## Getting started

```bash
git clone https://github.com/Masalale/Assignment-4---2D-Platformer-Feature-Update.git
```

Add it through Unity Hub and open `Assets/Scenes/StartScene.unity`. That's the one you want, the game starts there.

## Credits

| Asset | Source |
| :--- | :--- |
| Art, enemies, tiles | From the asset pack linked in the assignment brief |
| Sounds | `coin.ogg`, came with the pack |
| Music | Mountain by Nebulite, [freetouse.com/music](https://freetouse.com/music) (copyright free) |
| UI | Unity TextMeshPro |