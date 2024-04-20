# OpenPost

### Definition
Namespace: *`KieranCoppins.PostNavigation`* | Assembly: *kierancoppins.post-navigation.dll* | Source: [*OpenPost.cs*](../../../../Runtime/Posts/OpenPost.cs) | Inherits: *`Monobehaviour`* | Implements: [*`IOpenPost`*](../../../../Runtime/Posts/Interfaces/IOpenPost.cs)

A game component that represents an open post. The post manager will automatically read these posts at run time and incude them in its post data.

### Remarks
- Open Posts are fairly basic but contain a position property. This is automatically set from the game object's `transform.position`.