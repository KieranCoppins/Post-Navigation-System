# ZoneManager

### Definition
Namespace: *`KieranCoppins.PostNavigation`* | Assembly: *kierancoppins.post-navigation.dll* | Source: [*ZoneManager.cs*](../../../Runtime/Zones/ZoneManager.cs) | Inherits: *`Monobehaviour`*

The zone manager is a singleton monobehaviour in your scene that is in charge of managing the zones that have been placed. It periodically checks the state of each zone and ensures that the minimum and maximum agents in the zone are always being abided by. Here is a simple list of rules that the zone manager will check for every zone:
-  Check that the zone has the minimum agents; if not, then we need to find a zone that has more than the minimum agents. Depending on where the zone is in order of priority, agents will be shuffled forward or backward to the zone in crisis.
- If there is no zone that has more than the minimum number of agents, then we will take it from the zone with the lowest priority and send it to the zone in crisis.

Agents can request a zone using the zone manager. Typically, an agent will request the zone that is nearest to them; this is ultimately up to the scripter to decide which zone to request. When a zone is requested by an agent, the zone manager will:
1. Check that Zone A (the zone being requested) is below the minimum number of agents; if it is, then assign the agent to that zone.
1. If Zone A has the minimum agents, check that all the zones have minimum agents; if they don't, then shuffle an agent from Zone A to Zone B (the next zone in priority). This will cause a domino effect that causes the end zones to be filled.
1. If Zone A is above maximum capacity, then do the same as before and shuffle an agent from Zone A to Zone B.
1. If Zone A is below the maximum number of agents, then just assign the agent to Zone A.

The **combat vector** is a primary component of the zone manager. It describes the overall flow of combat in the level. One can usually describe a level's flow using a directional vector pointing from the exit to the entrance of the level. I.e., from the player's goal to the player's position. This combat vector is what determines zone priority in the zone manager.

Zone priority does not override the minimum agent requirement of the zone; however, if all zones have their minimum agents, then the manager will begin to fill the zones based on priority.

Having the combat vector point from the exit to the entrance will inherently create a more defensive layout against the player. The agents will prioritise the zone nearest to the exit. Alternatively, a game designer could invert this vector and create a more aggressive combat scenario where agents will want to prioritise zones closer to the entrance and get closer to the player.

### Variables
| Exposed To Inspector? | Accessor | Type | Name | Description |
|-----------------------|----------|------|------|-------------|
| False | `{ public get; private set; }` | `ZoneManager` | **Instance** | The singleton instance of `ZoneManager`. |
| False | `{ public get; private set; }` | `List<Zone>` | **Zones** | A list of all the zones in the active scene. |
| True | `private` | `Vector3` | **combatVector** | The combat vector that the zone manager will use, this should be a normalised vector. |


### Public Functions
| Return Type | Name | Description |
|-------------|------|-------------|
| `void` | `AssignAgentToZone(IPostAgent agent, Zone zone)` | Assigns the given agent to the given zone. If the agent derives `Monobehaviour` it will automatically hook into the `destroyCancellationToken` to remove the agent from the zone. |
| `IPostAgent[]` | `GetAgentsInZone(Zone zone)` | Gets all the agents that are assigned to the given zone. |
| `Zone` | `GetClosestZoneToAgent(IPostAgent agent)` | Gets the closest zone to the agent that isn't the zone they are currently assigned to. |
| `IPost[]` | `GetPostsForAgent(IPostAgent agent)` | Gets all the posts for an agent from their assigned zone. If the agent is not assigned to a zone it will return an empty array. |
| `void` | `RequestZone(Zone zone, IPostAgent agent, bool reverse = false)` | Requests the given zone for the given agent. `reverse` controls if agents should "shuffle" up or down the combat vector when shuffling occurs. |

### Remarks
- Population of the zone manager's `Zones` happens in Unity's `Awake` function. This is also where the singleton instance is populated. If the manager is accessed before its awake is called, it will return null.