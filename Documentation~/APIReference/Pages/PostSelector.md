# PostSelector

### Definition
Namespace: *`KieranCoppins.PostNavigation`* | Assembly: *kierancoppins.post-navigation.dll* | Source: [*PostSelector.cs*]()

A post selector takes a list of rules and an array of posts to run the rules on, it will iterate through each rule and apply the weighting the rule adds and returns the final dictionary of posts to weights.

### Constructors
| Name | Description |
|------|-------------|
| `PostSelector(List<IPostRule> rules, Func<Vector3, IPost[]> getPosts)` | The constructor to use, it initialises the post selector with a list of rules and a function that is ran to get all the posts for the post selector to run the post rules on. |

### Public Functions
| Return Type | Name | Description |
|-------------|------|-------------|
| `Dictionary<IPost, float>` | `Run(Vector3 origin)` | Runs the post selector, accepts an origin parameter that is passed into the getPosts function for procedural post generators. For example, if you want to generate posts in a grid around an origin position. Returns a dictionary of posts as keys and their final weighting as values. |

### Extensions
As a part of the post selector, an extension method has been created for `Dictionary<IPost, float>`:
```
public static IPost GetBestPost(this Dictionary<IPost, float> scores)
```

This returns the post with the highest weighting in the dictionary.
