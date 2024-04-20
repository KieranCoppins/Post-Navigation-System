# DistanceToTarget

### Definition
Namespace: *`KieranCoppins.PostNavigation`* | Assembly: *kierancoppins.post-navigation.dll* | Source: [*DistanceToTarget.cs*]() | Implements: [*`IPostRule`*]()

Calculates the distance of each post from the target transform's position and weights each post accordingly. The weighting can be normalised so that extreme distances are not affected. The distance check is as the crow flies so path cost is not taken into account.

### Constructors
| Name | Description |
|------|-------------|
| `DistanceToTarget(Transform target, float weight, bool normaliseScore)` | Constructs the rule with a transform `target` to check the distance from, a `weight` on how much the rule should affect the score of the post, and `normaliseScore` so that the total distance doesnt affect the weighting. For example, if all the posts are far away, they will be normalized between 0 and 1 from the farthest to the closest. |