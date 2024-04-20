# ICoverPost

### Definition
Namespace: *`KieranCoppins.PostNavigation`* | Assembly: *kierancoppins.post-navigation.dll* | Source: [*ICoverPost.cs*](../../../../Runtime/Posts/Interfaces/ICoverPost.cs) | Inherits: [*`IPost`*](../../../../Runtime/Posts/Interfaces/IPost.cs)

An interface that every cover post implements. Contains data relevant for cover posts.

### Properties
| Accessor | Return Type | Name | Description |
|----------|-------------|------|-------------|
| `{ public get; protected set; }` | `CoverType` | **CoverType** | The type of cover this post is. |
| `{ public get; protected set; }` | `Vector3` | **CoverDirection** | The direction that cover is in relation to this cover post. |
| `{ public get; protected set; }` | `bool` | **CanPeakLeft** | Whether the agent can peak left from this cover. |
| `{ public get; protected set; }` | `bool` | **CanPeakRight** | Whether the agent can peak right from this cover. |