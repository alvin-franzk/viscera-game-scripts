# Viscera
#### Video Demo: [YouTube](https://youtu.be/JxEz8x5xKD0)
## Description:
Viscera is a survival action game where players must fend off hordes of enemies, pick up potions for buffs, and aim for the highest score. Inspired by the combat mechanics of Alien Shooter, Viscera challenges players with intense sword swinging, blocking, and dodging action.

## Features
* **Survival Mode**: Jump straight into the action with no main menu.
* **RPG Melee Combat**: Engage in strategic combat with sword swinging, blocking, and dodging.
* **Powerups**: Collect potions to gain temporary buffs and enhance your survival chances.
* **High Score System**: Compete for the highest score by surviving as long as possible.

## Gameplay
In Viscera, the player is immediately placed into a survival level upon launching the game. The objective is to survive waves of enemies for as long as possible while collecting potions that provide various buffs. There are no level-ups, skill abilities, or rogue-like features—just pure, intense survival combat.

## Installation
1. Download the release here: [MEGA](https://mega.nz/file/del3VSLJ#rdgKhYQbMJfIwTTlloBR4NF6ZfMpcuHUTGNudr9RkUA) v0.3
2. Extract the downloaded zip file.
3. Run the executable file **Viscera.exe** to start playing.

## Controls
* Movement: W, A, S, D keys
* Attack: Left Mouse Button
* Block: Space (Hold)
* Dodge: Space + Movement keys

## Key Components
* Enemy Spawning: Enemies spawn in intervals that also scale the longer you survive. (The spawner gets upgraded to reduce the interval between spawns and buffs the enemies.)
* Combat System: Mechanics for sword swinging, blocking, and dodging.
* Buff System: Implementation of powerups through potion pickups.
* Randomization: Each variable in every game object (including the player) has random values on each startup. For example, an enemy might spawn with low health but greater damage. The player's damage might deal within a random range. Powerups might give less or more than intended. And more.
* Score System: Score is based on how much damage was dealt and how many attacks it took to take down an enemy.

## Development
Viscera was developed using [Unity](https://unity.com/) and programmed in C#. The project was created as a final project for the CS50 course.
In developing Viscera, I focused on creating a robust and engaging gameplay experience by writing multiple scripts to handle various aspects of the game. Here are the scripts I used and their use. If you want to take a closer look at the scripts, here is the [repo](https://github.com/alvin-franzk/viscera-game-scripts) for it.
### General Scripts
* *BGMPlayer.cs*: Manages the background music, ensuring that a soundtrack plays during gameplay to enhance the atmosphere. So far it has only three soundtracks, all are nod/reference to Alien Shooter.
* *EnemySpawner.cs*: Responsible for procedural generation of enemies, controlling the spawning process and managing the increasing difficulty of enemy waves. Each upgrade increases the maximum amount of enemies to spawn and reduces the interval between spawns which are activated based on how much score the player has accumulated.
* *LootDuration.cs*: Manages the duration of the potions before disappearing (in order to not clog the memory as well as a balancing feature).
* *LootManager.cs*: A Unity Scriptable object template, contributes to the game's potion/buff system.
* *SoundManager.cs*: Centralizes the control of sound effects, ensuring that audio cues are played correctly for actions like attacking, blocking, and picking up items.
* *StatueRegen.cs*: Implements a regeneration feature through statues placed in the game, allowing players to restore health or stamina at specific points.
### Enemy Scripts
* *EnemyAttackCollider.cs*: Handles the detection of player collisions with enemy attacks, managing the damage and effects triggered by these collisions.
* *EnemyStatus.cs*: Manages the health, status effects, and behavior of enemies, providing crucial data for combat and AI decision-making.
* *HitboxCollider.cs*: Detects collisions between the player's attacks, determining hit registration and damage application.
* *LootBag.cs*: Manages the items dropped by enemies upon defeat, ensuring that players are rewarded with loot appropriately. It is chance-based that is set by the scriptable object mentioned earlier.
### Player Scripts
* *CameraFollow.cs*: Manages the camera's behavior, ensuring it smoothly follows the player character throughout the game environment.
* *PlayerLootCollider.cs*: Detects collisions between the player and loot items, managing the collection of potions and other buffs.
* *PlayerStatus.cs*: Manages the player's health, status effects, and buffs, crucial for maintaining gameplay balance and player feedback.
### Enemy State Machine
* *EnemyBaseState.cs*: Defines the base state for enemy behaviors, serving as a foundation for more specific enemy states.
* *EnemyMovementStateMachine.cs*: Manages the entire logic for enemies that is needed to transition between states such as pathfinding and chasing the player. By default, it always returns the patrolling state. The scripts also includes the components and variables needed for the enemy e.g. trigger flags, attack settings, and sound clips.
* *EnemyStateMachine.cs*: Oversees the transitions between different enemy states, ensuring that enemies react appropriately to the player's actions.
#### Enemy States
* *EnemyAttacking.cs*: Manages the behavior of enemies during their attack phase, including attack animations and damage dealing.
* *EnemyChasing.cs*: Handles the logic for enemies when they are chasing the player, ensuring they follow and engage the player effectively.
* *EnemyDying.cs*: Manages the behavior of enemies when they are defeated, including death animations and removal from the game.
* *EnemyHurt.cs*: Controls the reactions of enemies when they take damage, including playing hurt animations and updating health status.
* *EnemyPatrolling.cs*: Oversees the patrol behavior of enemies, managing their movements and actions while they are not engaged with the player.
### Player State Machine
* *PlayerBaseState.cs*: Defines the base state for player behaviors, serving as a foundation for more specific player states.
* *PlayerLookDirection.cs*: Manages the direction the player character is looking, ensuring accurate aiming and camera orientation.
* *PlayerMovementStateMachine.cs*: Manages the entire logic for the player that is needed to transition between states such as moving, dodging, attacking, etc. By default, it always returns the idle state. The scripts also includes the components and variables needed for the enemy e.g. movement settings, dodge settings, block settings, attack settings, and sound clips.
* *PlayerStateMachine.cs*: Oversees the transitions between different player states, ensuring that the player reacts appropriately to various in-game situations.
#### Player States
* *Attacking.cs*: Manages the player's attack actions, including attack animations and damage dealing.
* *Blocking.cs*: Controls the player's blocking behavior, including shield usage and damage mitigation.
* *Dodging.cs*: Handles the player's dodging actions, enabling quick evasion from enemy attacks.
* *Dying.cs*: Manages the player's death behavior, including death animations and game-over conditions.
* *Hurt.cs*: Controls the player's reactions to taking damage, including playing hurt animations and updating health status.
* *Idle.cs*: Manages the player's idle state, including animations and behaviors when the player is not performing any actions.
* *Moving.cs*: Oversees the player's movement actions and running animations.
* *Sprinting.cs*: Handles the player's sprinting behavior, including faster movement and corresponding animations.

## Credits
- Developer: Alvin Franz T. Jebone
- Course: CS50 by Harvard University
- Assets: Mixamo, SKULLVERTEX, xiaolianhuastudio, Sigma Team (Alien Shooter Soundtrack)
- I might have forgotten some so feel free to DM me if I missed you; thank you all for taking a bit off the load!
