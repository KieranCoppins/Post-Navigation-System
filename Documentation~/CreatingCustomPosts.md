# Creating Custom Posts
![InteractionPostDemenstration](./Resources/InteractionPost.gif)

In this guide, we will look at creating our own custom posts using the `IPost` interface; and more specifically, creating an interaction post. Interaction posts are a post in the level that have some animation data associated with them. An example of this might be a post against a wall where an agent can go and lean on, another might be a workbench where the agent can go and play a tinkering animation. The idea is that the post is very generic and can be used by any agent to play any animation.

## Creating the Interaction Post class
First we want to create a new script in Unity called `InteractionPost` and implement the [`IPost`](./APIReference/Pages/Interfaces/IPost.md) interface. IPost only has one property and that is its position. We can just return the transform.position.

Next we want to add a public property of an Animation Clip. This is the clip that will be played by the agent when they arrive at the post.
```csharp
using KieranCoppins.PostNavigation;
using UnityEngine;

/// <summary>
/// A custom post implementation for playing an animation when an agent arrives at the post
/// </summary>
public class InteractionPost : MonoBehaviour, IPost
{
    Vector3 IPost.Position { get => transform.position; set => transform.position = value; }

    /// <summary>
    /// The animation that will play when an agent interacts with this post
    /// </summary>
    public AnimationClip InteractionAnimation;
}
```

## Placing the interaction post
To place the interaction post, all we have to do is create an empty game object in the scene and add the InteractionPost component. Place the game object where you'd like the animation to play and thats it! The post manager will get a reference to the created post at runtime!

## Making our agent move to the interaction post
Next we want go to our `IPostAgent` implementation. If you have an agent already, just implement the `IPostAgent` interface or alternatively you can go to the [Getting Started](GettingStarted.md) guide which will setup the IPostAgent implementation.

Inside the agent, we want to run a post selector to go to an interaction post, here is a simple post selector that will read from all the posts in the world and get the closest interaction post:
```csharp
postSelector = new PostSelector(
    new List<IPostRule>{
        new IsOfPostType(typeof(InteractionPost)),
        new DistanceToTarget(transform, 1, true)
    },
    (origin) => PostManager.Instance.Posts
);
```

We can then run the post selector like in the [Getting Started](GettingStarted.md) guide and save a reference to the post and move to it:
```csharp
IPost bestPost = postSelector.Run(transform.position).GetBestPost();
if (bestPost is InteractionPost interactionPost)
{
    postToGoTo = interactionPost;
    PostManager.Instance.OccupyPost(bestPost, this);
    agent.SetDestination(bestPost.Position);
}
```

**Note** here that I check that the post is an interaction post again, because I want to save an instance of the InteractionPost type so that we can access the animation clip easier. I cast now so we don't have to later!

## Playing the animation clip on arrival
Next all for us to do is to check that we are close enough to the interaction post to play the animation clip. This is a very crude implementation using the update loop:
```csharp
void Update()
{
    // Crude distance check for demonstration purposes
    if (postToGoTo != null && Vector3.Distance(transform.position, ((IPost)postToGoTo).Position) < 1.2f)
    {
        // Play the animation
        animComponent.Play(postToGoTo.InteractionAnimation.name);

        // Clear our post so we dont play the animation on repeat
        postToGoTo = null;
    }
}
```

To make the agent then move once the animation is finished we can add an event to the end of the animation that triggers the agent to run the post selector again. However, that is not for this guide.

And thats it, its very simple to create whatever custom post you may want!

## Final Implementations
InteractionPost implementation:
```csharp
using KieranCoppins.PostNavigation;
using UnityEngine;

/// <summary>
/// A custom post implementation for playing an animation when an agent arrives at the post
/// </summary>
public class InteractionPost : MonoBehaviour, IPost
{
    Vector3 IPost.Position { get => transform.position; set => transform.position = value; }

    /// <summary>
    /// The animation that will play when an agent interacts with this post
    /// </summary>
    public AnimationClip InteractionAnimation;
}
```
Agent implementation:
```csharp
using System;
using System.Collections.Generic;
using KieranCoppins.PostNavigation;
using UnityEngine;
using UnityEngine.AI;

public class GoToInteractionPost : MonoBehaviour, IPostAgent
{
    public Vector3 Position => transform.position;
    public Action<Zone> OnAssignedZone { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    private NavMeshAgent agent;
    private Animator animComponent;
    private PostSelector postSelector;
    private InteractionPost postToGoTo;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animComponent = GetComponent<Animator>();

        postSelector = new PostSelector(
            new List<IPostRule>{
                new IsOfPostType(typeof(InteractionPost)),
                new DistanceToTarget(transform, -1, true)
            },
            (origin) => PostManager.Instance.Posts
        );
    }

    // Start is called before the first frame update
    void Start()
    {
        GoToPost();
    }

    // Update is called once per frame
    void Update()
    {
        // Crude distance check for demonstration purposes
        if (postToGoTo != null && Vector3.Distance(transform.position, ((IPost)postToGoTo).Position) < 1.2f)
        {
            animComponent.Play(postToGoTo.InteractionAnimation.name);
            postToGoTo = null;
        }
    }

    public void GoToPost()
    {
        IPost bestPost = postSelector.Run(transform.position).GetBestPost();
        if (bestPost is InteractionPost interactionPost)
        {
            postToGoTo = interactionPost;
            PostManager.Instance.OccupyPost(bestPost, this);
            agent.SetDestination(bestPost.Position);
        }

    }
}
```