# PostGenerators

### Definition
Namespace: *`KieranCoppins.PostNavigation`* | Assembly: *kierancoppins.post-navigation.dll* | Source: [*PostGenerators.cs*](../../../Runtime/PostGenerators.cs)

A static class containing a series of functions related to generating or reading posts.

### Public Static Functions
| Return Type | Name | Description |
|-------------|------|-------------|
| `IPost[]` | `GenerateGrid(Vector3 origin, float widthCount, float heightCount, float spacing)` | Generates a grid of generic posts around the provided origin. The grid with ve `widthCount` by `heightCount` where each post is equally spaced by `spacing`. |
| `IPost[]` | `GenerateCirle(Vector3 origin, float radius, float postSpacing)` | Generates a grid of points in a radius around the given origin. Each post is equally spaced by `postSpacing`. |
| `IPost[]` | `GetPostFromSceneData()` | Loads the posts saved in the scene data produced by the nav mesh post generation. This is not recommended to use and instead you should look to use the `PostManager`. |
