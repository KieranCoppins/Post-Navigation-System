# Post Navigation System API Reference Docs

### Interfaces
| Class | Description |
|-----------|-------------|
| [`CoverPost`][CoverPost] | A monobehaviour implementation of [`ICoverPost`][ICoverPost]. Allows game designers to place open posts in the level. |
| [`DistanceToTarget`][DistanceToTarget] | Weights each post depending on the distance from a given point. |
| [`HasLineOfSight`][HasLineOfSight] | Weights each post depending if the post has line of sight to a given point. This rule can be destructive.|
| [`ICoverPost`][ICoverPost] | A post that describes a point of cover in the level for an agent. |
| [`IOpenPost`][IOpenPost] | A post that describes an open point in the level for an agent. |
| [`IPost`][IPost] | The base interface for any post for the Post Navigation System. |
| [`IPostAgent`][IPostAgent]  | Any agent that aims to use posts or zones. |
| [`IPostRule`][IPostRule] | The interface for any rule that a post selector should run on all the posts it has. |
| [`IsInCover`][IsInCover] | Weights each post depending if the post is of a cover type and is in cover from a given point. This rule can be destructive. |
| [`IsNotOccupied`][IsNotOccupied] | A destructive rule that removes any posts from consideration if they are currently being occupied. |
| [`IsOfPostType`][IsOfPostType] | A destructive rule that removes any post from consideration if they are not the provided post type. |
| [`NavMeshPostGenenerationConfig`][NavMeshPostGenenerationConfig] | Contains data used for post generation on the NavMesh. |
| [`OpenPost`][OpenPost] | A monobehaviour implementation of [`IOpenPost`][IOpenPost]. Allows game designers to place open posts in the level. |
| [`PostGenerators`][PostGenerators] | A static class that contains various methods for post generation at runtime. |
| [`PostManager`][PostManager] | A singleton manager that handles storing post data and agents occupation of posts. |
| [`Zone`][Zone] | An area that agents can be assigned to. Each zone has a reference to all posts within its zone. |
| [`ZoneManager`][ZoneManager] | A singleton monobehaviour in your scene that is in charge of managing the zones that have been placed. It periodically checks the state of each zone and ensures that the minimum and maximum agents in the zone are always being abided by. Also keeps track of agents that are assigned to zones and handles zone requests made by agents. |


[ICoverPost]: <./Pages/Interfaces/ICoverPost.md> (ICoverPost)
[IOpenPost]: <./Pages/Interfaces/IOpenPost.md> (IOpenPost)
[IPost]: <./Pages/Interfaces/IPost.md> (IPost)
[IPostAgent]: <./Pages/Interfaces/IPostAgent.md> (IPostAgent)
[IPostRule]: <./Pages/Interfaces/IPostRule.md> (IPostRule)

[OpenPost]: <./Pages/PlaceablePosts/OpenPost.md> (OpenPost)
[CoverPost]: <./Pages/Interfaces/IPostRule.md> (CoverPost)

[PostManager]: <./Pages/PostManager.md> (PostManager)
[ZoneManager]: <./Pages/ZoneManager.md> (ZoneManager)

[DistanceToTarget]: <./Pages/PostRules/DistanceToTarget.md> (DistanceToTarget)
[HasLineOfSight]: <./Pages/PostRules/HasLineOfSight.md> (HasLineOfSight)
[IsInCover]: <./Pages/PostRules/IsInCover.md> (IsInCover)
[IsNotOccupied]: <./Pages/PostRules/IsNotOccupied.md> (IsNotOccupied)
[IsOfPostType]: <./Pages/PostRules/IsOfPostType.md> (IsOfPostType)

[PostGenerators]: <./Pages/PostGenerators.md> (PostGenerators)

[NavMeshPostGenenerationConfig]: <./Pages/NavMeshPostGenerationConfig.md> (NavMeshPostGenenerationConfig)

[Zone]: <./Pages/Zone.md> (Zone)

