# NavMeshPostGenerationConfig

### Definition
Namespace: *`KieranCoppins.PostNavigation`* | Assembly: *kierancoppins.post-navigation.dll* | Source: [*PostGenerators.cs*]()

### Variables
| Accessor | Type | Name | Description |
|----------|------|------|-------------|
| `public` | `float` | **AgentHeight** | The height of the agent that will be using the generated posts. Used for determining if cover is low or high cover. |
| `public` | `float` | **CoverDistance** | Th distance from the point to check for cover. In other words, this is the length of raycast used to determine if the current point being checked is cover. This should be a little bit larger than the radius of your agent. |
| `public` | `float` | **CoverPeakDistance** | The distance from the point to the left or right to check if an agent can peak around the cover. This is for high covers so that cover points aren't generated in the center of a wall with no where to peak. |
| `public` | `float` | **CoverPostStepSize** | The distance between each cover post. This is used during edge walking around the NavMesh to find points to check if they are part of cover or not. |

### Constructors
| Name | Description |
|------|-------------|
| `NavMeshPostGenerationConfig()` | Default constructor. Populates each value with some default values that have been tested to produce a good result. |