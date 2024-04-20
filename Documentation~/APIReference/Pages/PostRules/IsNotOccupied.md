# IsNotOccupied

### Definition
Namespace: *`KieranCoppins.PostNavigation`* | Assembly: *kierancoppins.post-navigation.dll* | Source: [*IsNotOccupied.cs*](../../../../Runtime/Rules/IsNotOccupied.cs) | Implements: [*`IPostRule`*](../../../../Runtime/Rules/Interfaces/IPostRule.cs)

Checks if the post is currently occupied by any agent that isnt the agent passed in the constructor. This rule is always destructive and will remove posts from consideration if they are occupied.

### Constructors
| Name | Description |
|------|-------------|
| `IsNotOccupied(IPostAgent self)` | Constructs the rule with an agent, this agent should be the agent that is running the rule. The rule will still keep the post that the agent passed is occupying. |