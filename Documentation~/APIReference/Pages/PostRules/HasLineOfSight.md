# HasLineOfSight

### Definition
Namespace: *`KieranCoppins.PostNavigation`* | Assembly: *kierancoppins.post-navigation.dll* | Source: [*HasLineOfSight.cs*](../../../../Runtime/Rules/HasLineOfSight.cs) | Implements: [*`IPostRule`*](../../../../Runtime/Rules/Interfaces/IPostRule.cs)

Casts a raycast from each post in the post selector to the target transform's position. This rule can be destructive and remove posts from consideration from next rules if the post does not have line of sight.

### Constructors
| Name | Description |
|------|-------------|
| `HasLineOfSight(Transform target, float heightOffset, float weight = 1f, bool destructive = true)` | Constructs the rule with a transform `target` to check the line of sight to, a `weight` on how much the rule should affect the score of the post, and `destructive` bool if the rule should remove posts from the dictionary if they do not have line of sight to the target transform. |