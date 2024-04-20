# IsInCover

### Definition
Namespace: *`KieranCoppins.PostNavigation`* | Assembly: *kierancoppins.post-navigation.dll* | Source: [*IsInCover.cs*](../../../../Runtime/Rules/IsInCover.cs) | Implements: [*`IPostRule`*](../../../../Runtime/Rules/Interfaces/IPostRule.cs)

Checks if the post is in cover from the given target transform's position. This rule can be destructive and remove posts from consideration from next rules if the post is not in cover.

### Constructors
| Name | Description |
|------|-------------|
| `IsInCover(Transform target, float distance = 2f, float weight = 1f, bool destructive = true)` | Constructs the rule with a transform `target` to check if the post is in cover from, a `weight` on how much the rule should affect the score of the post, and `destructive` bool if the rule should remove posts from the dictionary if they are not in cover from the target transform's position. |