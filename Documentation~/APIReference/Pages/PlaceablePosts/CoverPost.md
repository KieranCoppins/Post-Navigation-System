# CoverPost

### Definition
Namespace: *`KieranCoppins.PostNavigation`* | Assembly: *kierancoppins.post-navigation.dll* | Source: [*CoverPost.cs*]() | Inherits: *`Monobehaviour`* | Implements: [*`ICoverPost`*]()

A game component that represents a cover post. The post manager will automatically read these posts at run time and incude them in its post data.

### Variables
| Exposed To Inspector? | Accessor | Type | Name | Description |
|-----------------------|----------|------|------|-------------|
| True | `{ public get; protected set; }` | `CoverType` | **CoverType** | Whether this cover is high or low cover. |
| True | `{ public get; protected set; }` | `bool` | **CanPeakLeft** | Whether an agent can peak left from this cover. |
| True | `{ public get; protected set; }` | `bool` | **CanPeakRight** | Whether an agent can peak right from this cover. |

### Remarks
- Cover posts have a property called `CoverDirection`. On these game component posts that allow game designers to place them wherever they like, the forward vector of the game object which this component is attached to will be cover direction. Likewise `Position` will be taken from the game object's `transform.position`.