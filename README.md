# tetrisk
Tetris game with shenanigans made in Unity

### Adding powerups

Create unique tag for powerup

Add prefab (current scale: x=`2`, y=`2`, z=`1`)

+ attach tag
+ Network Identity
  + local player authority: `true`
+ Network Transform
  + transform sync mode: `sync transform`
+ CircleCollider2d
  + isTrigger: `true`
  + current radius: `0.09` 
+ PowerUpController script

Add to registed spawnables in Network Manager

Add to PowerUpSpawner script in PlayerScreen (increase size by 1)

Add powerup effect to player controller

```
if (powerUpControl != null) {
        if (powerUpControl.getCollected()){
          if (powerUp.CompareTag("power1")) {
            // do amazing powerup stuff here //
          }
          powerUpPresent = false;
          powerUpControl.DestoryPowerUp();
        }
      }
```