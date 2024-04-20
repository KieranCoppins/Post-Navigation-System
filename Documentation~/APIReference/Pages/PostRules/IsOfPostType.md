# IsOfPostType

### Definition
Namespace: *`KieranCoppins.PostNavigation`* | Assembly: *kierancoppins.post-navigation.dll* | Source: [*IsOfPostType.cs*](../../../../Runtime/Rules/IsOfPostType.cs) | Implements: [*`IPostRule`*](../../../../Runtime/Rules/Interfaces/IPostRule.cs)

Checks if the post is derived from the given type. This rule is always destructive and will remove posts from consideration if they do not derive the given type.

### Constructors
| Name | Description |
|------|-------------|
| `IsOfPostType(Type postType)` | Constructs the rule with a type. Any posts that are not the given type are removed from consideration. |