# PostManager

### Definition
Namespace: *`KieranCoppins.PostNavigation`* | Assembly: *kierancoppins.post-navigation.dll* | Source: [*PostManager.cs*]()

A singleton class that manages all the posts in the scene, it is responsable for keeping a reference to all posts and which posts are occupied by IPostAgents.

### Variables
| Accessor | Type | Name | Description |
|----------|------|------|-------------|
| `{ public get; }` | `PostManager` | **Instance** | The instance of the singleton post manager. |
| `{ public get; private set; }` | `IPost[]` | **Posts** | All the posts loaded from the scene data, these are the posts generated from the nav mesh and serialized into the scene data. |



### Public Functions
| Return Type | Name | Description |
|-------------|------|-------------|
| `bool` | `IsPostOccupied(IPost post)` | Checks if the post provided is occupied by any agent. Returns true if it is. |
| `bool` | `IsPostOccupiedBy(IPost post, IPostAgent agent)` | Checks if the post provided is occupied by the provided agent. Returns true if it is. |
| `void` | `OccupyPost(IPost post, IPostAgent agent)` | Occupies the given post by the given agent. |