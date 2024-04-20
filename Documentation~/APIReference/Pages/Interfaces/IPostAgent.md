# IPostAgent

### Definition
Namespace: *`KieranCoppins.PostNavigation`* | Assembly: *kierancoppins.post-navigation.dll* | Source: [*IPostAgent.cs*]()

An interface that every agent in the game that uses posts or zones should implement.

### Properties
| Accessor | Return Type | Name | Description |
|----------|-------------|------|-------------|
| `{ public get; }` | `Vector3` | **Position** | A public getter to get the current position of the agent. |
| `{ public get; public set; }` | `Action<Zone>` | **OnAssignedZone** | A public action delegate that is called by the zone manager when the agent is assigned to a zone. |
