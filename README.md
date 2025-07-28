# Crystal Quest

Crystal Quest is a simple 2D top‑down action game designed for an AR/VR course assignment.  Your goal as the player is to explore the map, collect a number of crystals scattered around the level and survive against a growing horde of enemies.  You win by collecting all crystals before your health reaches zero.

## Features

* **Player movement:** The player can move in four directions (up, down, left, right) using the **Horizontal** and **Vertical** input axes (WASD or arrow keys).  Movement is implemented with a `Rigidbody2D` so collisions are handled by Unity’s physics system.  Input is sampled in `Update` and applied to velocity in `FixedUpdate`【266688935483207†L61-L88】.
* **Projectile system:** Clicking or pressing the **Fire1** button spawns a projectile prefab that travels forward.  A cooldown prevents spamming.
* **Enemies:** Enemies spawn at regular intervals around a central spawner using a coroutine.  Random positions are chosen within a radius around the spawner【340535937812746†L46-L70】, and timing is controlled with `WaitForSeconds`【340535937812746†L82-L123】.  Enemies seek the player and deal damage on contact.  They can be destroyed by projectiles.
* **Crystals:** Collectible crystals increase your score and count toward the win condition.  Collecting all crystals triggers the win state.
* **Health and score:** The `GameManager` tracks the player’s health, score and number of collected crystals.  TextMeshPro labels display this information on screen.  When health reaches zero the game ends with a “Game Over” message; collecting all crystals displays “You Win!”.

## Getting started

These instructions assume you have the Unity Editor installed (any recent 2020‑2023 LTS version will work).  You can either create a new project and copy the files into the `Assets` folder, or clone the repository and open it directly from Unity Hub.

1. **Create a new project:** Launch Unity Hub and create a new **2D** project named `CrystalQuest`.  Once Unity finishes loading, close the initial scene.
2. **Import the assets:** Copy the `Assets` folder from this repository into your Unity project directory, overwriting the existing `Assets`.  The folder contains `Scripts` and `Sprites` used by the game.
3. **Create the main scene:** In Unity, right‑click inside the *Assets* window and choose **Create → Scene**.  Name it `MainScene` and double‑click it to open.
4. **Player setup:**
   - Drag `Assets/Sprites/player.png` into the **Hierarchy** to create a new GameObject.  Rename it `Player`.
   - With `Player` selected, click **Add Component** and add **Rigidbody 2D**.  Set *Body Type* to **Dynamic** and *Gravity Scale* to **0** (so the player doesn’t fall).
   - Add a **Box Collider 2D** or **Circle Collider 2D** to handle collisions.
   - Add the `PlayerController` script from `Assets/Scripts`.
   - Right‑click the `Player` in the Hierarchy and choose **Create Empty** to add a child.  Name this child `ProjectileSpawnPoint` and move it slightly in front of the player sprite (e.g. `x = 0.5`).
   - Drag and drop the `ProjectileSpawnPoint` into the **Projectile Spawn Point** field of `PlayerController` in the Inspector.
   - Create a **Projectile** prefab: drag `Assets/Sprites/crystal.png` into the scene and remove its collider, then add a **Circle Collider 2D** set to *Is Trigger* and add the `Projectile` script.  Adjust the `speed`, `damage` and `lifetime` in the Inspector.  Drag this GameObject back into the `Assets` window to make it a prefab and then delete the instance in the scene.  Assign this prefab to the `Projectile Prefab` field of `PlayerController`.
5. **Enemy setup:**
   - Drag `Assets/Sprites/enemy.png` into the scene and rename the GameObject `Enemy`.  Add a **Rigidbody 2D** (Dynamic, gravity scale 0), a **Circle Collider 2D**, and the `Enemy` script.  Configure health and speed as desired.
   - Drag the `Enemy` GameObject into the *Assets* window to create a prefab, then delete it from the scene.
   - Create an empty GameObject named `Spawner` in the scene.  Add the `EnemySpawner` script and assign your enemy prefab to the **Enemy Prefab** field.  Adjust the *Spawn Interval* (e.g. `10` seconds) and *Spawn Radius*.
6. **Crystals:** Drag `Assets/Sprites/crystal.png` into the scene multiple times to place crystals around your map.  Add a **Circle Collider 2D** set to *Is Trigger* and attach the `Crystal` script.  In the `GameManager` (see next step), set `Total Crystals` to the number of crystals you’ve placed.
7. **Game manager and UI:**
   - Create an empty GameObject named `GameManager`.  Add the `GameManager` script.
   - Add a **Canvas** (right‑click the Hierarchy → **UI → Canvas**).  Set *Render Mode* to **Screen Space Overlay**.  Add a **TextMeshPro – Text** object for the score, another for health, another for the timer, and a fourth for the end message.  Position these at the top of the screen.  Drag these text objects into the corresponding fields of the `GameManager` script.
   - Set `Total Crystals` in `GameManager` equal to the number of crystals you placed.
8. **Play the game:** Press **Play**.  Use WASD or arrow keys to move the player and click the left mouse button to fire projectiles.  Collect all crystals to win; avoid enemies to stay alive.

## References

* The movement code reads the **Horizontal** and **Vertical** input axes and sets the Rigidbody2D’s velocity accordingly, as recommended in Unity tutorials for top‑down movement【266688935483207†L61-L88】.
* The spawner uses `Random.insideUnitCircle` to choose random positions within a radius and repeatedly spawns enemies using a coroutine and `WaitForSeconds`【340535937812746†L46-L70】【340535937812746†L82-L123】.

Feel free to expand upon this template by adding more enemy types, power‑ups, particle effects, or more advanced AI behaviours.  Good luck with your project!