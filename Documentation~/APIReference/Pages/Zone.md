# Zone

### Definition
Namespace: *`KieranCoppins.PostNavigation`* | Assembly: *kierancoppins.post-navigation.dll* | Source: [*Zone.cs*]() | Inherits: *`Monobehaviour`*

Zones are a polygonal area that a game designer can draw. Any posts within the zone's area will be part of the zone. Agents can be assigned to zones and therefore have their post selection limited to posts within the zone. They have a minimum and maximum agent count that a game designer can tweak to make the AI behave how they want. A `ZoneManager` is required when using zones in your level.

### Variables
| Exposed To Inspector? | Accessor | Type | Name | Description |
|-----------------------|----------|------|------|-------------|
| True | ` { public get; public set; } `  | `List<Vector3>` | **ZonePoints** | A list of points that make up the zone's boundary. They should describe a closed loop of points for the zone's polygon. These points should not contain any height data, it should escribe a 2D polygon on the floor of the zone. Use `height` to define the height of the zone. |
| True | `private` | `float` | **height** | The height of the zone, any posts that are above or below the zones height are not included in the zone. The points of the zone are extruded by the height in the positive Y to produce the full 3D polygon. |
| True | ` {public get; private set; } ` | `int` | **MinAgents** | The minimum number of agents that should be assigned to this zone. The `ZoneManager` will aim to keep this zone at its minimum if there are any agents spare. |
| True | ` {public get; private set; } ` | `int` | **MaxAgents** | The maximum number of agents that should be assigned to this zone. The `ZoneManager` will never assign agents past the zone's maximum. |

### Public Functions
| Return Type | Name | Description |
|-------------|------|-------------|
| `bool` | `IsPointInZone(Vector3 point)` | Checks if the given point is inside the Zone's polygonal area and returns `true` if it is. |