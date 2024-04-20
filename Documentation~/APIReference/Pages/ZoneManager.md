# ZoneManager

### Definition
Namespace: *`KieranCoppins.PostNavigation`* | Assembly: *kierancoppins.post-navigation.dll* | Source: [*ZoneManager.cs*](../../../Runtime/Zones/ZoneManager.cs) | Inherits: *`Monobehaviour`*

A singleton monobehaviour that manages all the zones in the scene. It assigns agents to zones and keeps track of the state of each zone ensuring that the minimum agents are always met and the maximum agents are not exceeded.

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