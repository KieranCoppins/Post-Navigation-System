# IPostRule

### Definition
Namespace: *`KieranCoppins.PostNavigation`* | Assembly: *kierancoppins.post-navigation.dll* | Source: [*IPostRule.cs*]()

The post rule interface. Every post selector rule must implement this interface.

### Methods
| Accessor | Return Type | Name | Description |
|----------|-------------|------|-------------|
| `public` | `Dictionary<IPost, float>` | `Run(Dictionary<IPost, float> scores)` | Is called by the post selector, passes in the current dictionary of post to scores and should return the new dictionary of posts to scores. |
