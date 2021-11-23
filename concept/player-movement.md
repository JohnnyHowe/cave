Player movement based around 3 main mechanics: Jumping, sliding, and dashing.
Both movements will cause an increase in mass to make pushing obstacles easier. 
# Jump
Done by swiping up.

Character will instantly be given an upwards velocity.

**Restrictions**
Player will not jump regardless of input if
* Player is directly under a solid tile (e.g. sliding in a small gap)
* Not touching ground (or has already used double jump)

# Slide
Done by swiping down.

Character will be given a small velocity increase but will lost speed faster than when running.
The players collider will become shorter to allow the player through narrower gaps. 

**Restrictions**
Player will not slide regardless to input if
* In the air - this however will cause the player to Drop

## Drop
When the player tries to slide but is in the air, they will drop.
Dropping causes the player to fall faster than usual and slide upon landing.
# Dash