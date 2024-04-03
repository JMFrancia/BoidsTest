# Zombie Legion Prototype


## Setup

Followed this tutorial: https://www.youtube.com/watch?v=6dJlhv3hfQ0


## Implementation Notes

* Agents spawned as part of flocks
* Flocks have 2 factions: zombie or human
* Flock
	* Iterates over all members...
		* Gathers their contexts (IE objects within 3 ranges: line of sight, neighborhood, and immediate)
		* Iterates through all attached behaviors passing in flock, agent, and contexts to calculate final vector for that agent at that step, passes to agent for movement
* Flock behaviors
	* Set up as scriptable objects
	* Can have filters (used to filter contexts for same flock, specific faction, etc.)
	* Can be composite (combining or lerping between multiple behaviors)
* Gameplay
	* Zombie flock has "follow the leader" behavior following player-controlled zombie leader
	* Zombie agents convert human agents to zombie flock upon collision
	* Human civilians flee zombies
	* Human soldiers try to maintain distance and shoot zombies (no bullets, just insta-kill)
	* Game ends with either all zombies are dead or all humans are converted to zombies

## Roadmap

### Core game demo

- [x] Start / win / lose UI
- [ ] Frenzy meter + button
- [ ] Rigidbody experiment
- [ ] Character graphics  
- [ ] Level graphics
- [ ] SFX
- [ ] Background SFX
- [ ] Narrative + graphic for leader mechanic
- [ ] Juice
- [ ] Agent health + UI
- [ ] VFX + shaders (try cartoon?)

### Expanded content

- [ ] Hazards (mines, barbed wire)
- [ ] New enemies (machine gunner, shot gunner)
- [ ] New zombie types (scout (kamikazi?), slinger, goliath)
- [ ] Powerups (health, speed, defense, attack)
- [ ] Zombie group rescue
- [ ] More complex maps


### Core game MVP

- [ ] Tutorial (lab escape)
- [ ] Power up choice
- [ ] Level up screen
- [ ] Agent stats

### Meta game

- [ ] Large map roguelike vs small map roguelike?
- [ ] Map conquest metagame (a la Rise of Nations) vs dynamic path metagame (a la FTL)?


### Optimization

- [ ] Quadtree
- [ ] Computer shader?
- [ ] Slow down (or stop) flock calculations outside player view
- [ ] Variable frame count for all behaviors (IE calc. every N frames) + load balancing (IE dividing behaviors evenly amongst frames to avoid CPU usage spikes)

### Misc

- [ ] Cats??
- [ ] Teleportation vs pathfind vs rescue for zombies left behind (leader teleports to zombie group if local flock dies?)
