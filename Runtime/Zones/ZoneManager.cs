using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KieranCoppins.PostNavigation
{
    /// <summary>
    /// A singleton monobehaviour that manages all the zones in the scene. It assigns agents to zones and keeps track of the state of each zone
    /// ensuring that the minimum agents are always met and the maximum agents are not exceeded.
    /// </summary>
    public class ZoneManager : MonoBehaviour
    {
        /// <summary>
        /// The singleton instance of the zone manager
        /// </summary>
        public static ZoneManager Instance { get; private set; }

        /// <summary>
        /// All the zones in the scene
        /// </summary>
        public List<Zone> Zones { get; private set; }

        /// <summary>
        /// The vector that determines the combat direction, this is used to determine the priority of zones where
        /// zones at the back of the combat vector have a higher "priority". Note that this does not override zone's
        /// minimum agent requirements.
        /// </summary>
        [SerializeField, Tooltip("The vector that determines the combat direction, this is used to determine the priority of zones")] Vector3 combatVector = Vector3.forward;

        /// <summary>
        /// A dictionary that maps agents to the zones they are assigned to
        /// </summary>
        private Dictionary<IPostAgent, Zone> assignedZones = new();

        /// <summary>
        /// Assigns an agent to a zone
        /// </summary>
        /// <param name="agent">The agent to assign</param>
        /// <param name="zone">The zone to assign to</param>
        public void AssignAgentToZone(IPostAgent agent, Zone zone)
        {
            if (!assignedZones.ContainsKey(agent) && agent is MonoBehaviour monoBehaviour)
            {
                // If we are a monobehaviour then linked to the ondestroy event
                monoBehaviour.destroyCancellationToken.Register(() => { assignedZones.Remove(agent); });
            }
            assignedZones[agent] = zone;
            agent.OnAssignedZone?.Invoke(zone);
        }

        /// <summary>
        /// Gets all the agents within a zone
        /// </summary>
        /// <param name="zone">The zone to get all agents from</param>
        /// <returns>An array of IPostAgents that are assigned to the given zone</returns>
        public IPostAgent[] GetAgentsInZone(Zone zone)
        {
            // This is o(n) where n is agents in the scene but there wont be a lot of agents
            return assignedZones.Where(kvp => kvp.Value == zone).Select(kvp => kvp.Key).ToArray();
        }

        /// <summary>
        /// Gets posts for an agent from their assigned zone. If they are not assigned to a zone
        /// returns an empty array
        /// </summary>
        /// <param name="agent">The agent to get posts for</param>
        /// <returns>An array of all the posts within the zone the agent is assigned to</returns>
        public IPost[] GetPostsForAgent(IPostAgent agent)
        {
            if (assignedZones.ContainsKey(agent))
            {
                return assignedZones[agent].Posts.ToArray();
            }

            return new IPost[0];
        }

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("More than one ZoneManager in the scene. Deleting the new one.");
                Destroy(this.gameObject);
                return;
            }
            else
            {
                Instance = this;
            }

            // Get all zones in the scene
            GameObject[] gameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            List<Zone> zonesInScene = new List<Zone>();
            foreach (var rootObject in gameObjects)
            {
                zonesInScene.AddRange(rootObject.GetComponentsInChildren<Zone>());
            }

            Vector3 zoneCenterOfMass = zonesInScene.Aggregate(Vector3.zero, (acc, zone) => acc + zone.transform.position) / zonesInScene.Count;

            /// Add zones to the list in order of priority, the priority is determined by the combat vector
            /// The zones at the back of the vector are the highest priority
            zonesInScene.Sort((p1, p2) =>
            {
                Vector3 p1Vector = p1.transform.position - zoneCenterOfMass;
                Vector3 p2Vector = p2.transform.position - zoneCenterOfMass;

                return Vector3.Dot(p1Vector, combatVector).CompareTo(Vector3.Dot(p2Vector, combatVector));
            });

            Zones = new List<Zone>(zonesInScene);

            // Assign posts to each zone
            foreach (var zone in Zones)
            {
                foreach (var post in PostManager.Instance.Posts)
                {
                    if (zone.IsPointInZone(post.Position))
                    {
                        zone.Posts.Add(post);
                    }
                }
            }

            StartCoroutine(CheckZoneStates());
        }

        /// <summary>
        /// Gets the closest zone to the agent that isnt the zone they are currently in
        /// </summary>
        /// <param name="agent">The agent to get the zone for</param>
        /// <returns>The zone closest to the agent that isn't the zone they are assigned to</returns>
        public Zone GetClosestZoneToAgent(IPostAgent agent)
        {
            float dst = float.MaxValue;
            Zone closestZone = null;
            foreach (var zone in Zones)
            {
                if (assignedZones.ContainsKey(agent) && assignedZones[agent] == zone) continue;

                float newDst = Vector3.Distance(agent.Position, zone.transform.position);
                if (newDst < dst)
                {
                    dst = newDst;
                    closestZone = zone;
                }
            }
            return closestZone;
        }

        /// <summary>
        /// Requests the zone for the given agent
        /// </summary>
        /// <param name="zone">The zone that is being requested</param>
        /// <param name="agent">The agent requesting the zone</param>
        /// <param name="reverse">Controls if agents should "shuffle" up or down the combat vector</param>
        public void RequestZone(Zone zone, IPostAgent agent, bool reverse = false)
        {
            if (zone == null) return;

            int currentAgents = GetAgentsInZone(zone).Length;

            if (currentAgents < zone.MinAgents)
            {
                // Assign the agent to the zone
                AssignAgentToZone(agent, zone);
                return;
            }

            if (!AllZonesHaveMinimumAgents() || currentAgents >= zone.MaxAgents)
            {
                // Try to shuffle the agent
                if (ShuffleAgent(zone, reverse))
                {
                    // If we succeeded we can go to this zone and an agent that was in that zone will move to one in need
                    AssignAgentToZone(agent, zone);
                }
                else
                {
                    // Otherwise we need to go to the one in need
                    RequestZone(Zones.First(z => currentAgents < z.MinAgents), agent);
                }
                return;
            }

            if (currentAgents < zone.MaxAgents)
            {
                // Assign the agent to the zone
                AssignAgentToZone(agent, zone);
                return;
            }
        }

        /// <summary>
        /// Shuffles an agent from the given zone to the next zone in the list
        /// </summary>
        /// <param name="zone">The zone to shuffle an agent from</param>
        /// <param name="reverse">Controls if agents should "shuffle" up or down the combat vector</param>
        /// <returns>If shuffling the agent was successful. It can fail if theres no agents to shuffle</returns>
        private bool ShuffleAgent(Zone zone, bool reverse)
        {
            // Get the next zone in the list
            int currentZoneIndex = Zones.IndexOf(zone);

            Zone nextZone = null;
            if (reverse)
            {
                if (currentZoneIndex > 0)
                {
                    nextZone = Zones[currentZoneIndex - 1];
                }
            }
            else
            {
                if (currentZoneIndex < Zones.Count - 1)
                {
                    nextZone = Zones[currentZoneIndex + 1];
                }
            }

            if (zone != null)
            {
                // Move an agent in the zone being requested to the next zone
                IPostAgent agentInZone = GetAgentsInZone(zone).FirstOrDefault();
                if (agentInZone != null)
                {
                    RequestZone(nextZone, agentInZone, reverse);
                    return true;
                }
                return false;
            }

            // If we are at the end of the zones log a warning
            Debug.LogWarning("No zones available to shuffle agents to, all zones are at maximum capacity.");
            return false;
        }

        /// <summary>
        /// Checks if all the zones in the manager have the minimum agents
        /// </summary>
        /// <returns>True if all agents have the minimum zones</returns>
        private bool AllZonesHaveMinimumAgents()
        {
            return Zones.All(zone =>
            {
                var currentAgents = GetAgentsInZone(zone).Length;
                return currentAgents >= zone.MinAgents;
            });
        }

        /// <summary>
        /// A coroutine that checks the state of each zone in the scene and ensures that the
        /// minimum and maximum agents are met
        /// </summary>
        private IEnumerator CheckZoneStates()
        {
            while (true)
            {

                foreach (var zone in Zones)
                {
                    int currentAgents = GetAgentsInZone(zone).Length;
                    bool abort = false;
                    if (currentAgents < zone.MinAgents)
                    {
                        // Shuffle agents backwards to fill the zone
                        int currentZoneIndex = Zones.IndexOf(zone);

                        // Check if there is a zone that is not at minimum capacity, start at the lowest priority zone
                        for (int i = Zones.Count - 1; i >= 0; i--)
                        {
                            Zone z = Zones[i];
                            int zCurrentAgents = GetAgentsInZone(z).Length;
                            if (zCurrentAgents > z.MinAgents)
                            {
                                // Shuffle back to bubble back to the zone that needs agents if the zone that has available
                                // agents is ahead of the zone that is in crisis
                                ShuffleAgent(z, i > currentZoneIndex);
                                abort = true;
                                break;
                            }
                        }

                        if (abort) continue;

                        // Everyone is at min capacity so pull from the lowest priority zone
                        if (currentZoneIndex < Zones.Count - 1)
                        {
                            // Loop through the rest of the zones until we find an agent we can send back
                            for (int i = currentZoneIndex + 1; i < Zones.Count; i++)
                            {
                                Zone nextZone = Zones[i];
                                IPostAgent agentInZone = GetAgentsInZone(nextZone).FirstOrDefault();
                                if (agentInZone != null)
                                {
                                    RequestZone(zone, agentInZone);
                                    break;
                                }
                            }
                        }
                    }
                }

                // No need to do this every frame
                yield return new WaitForSeconds(.4f);
            }
        }


        /// An agent should be able to request a particular zone, most likely the one closest to it
        /// We should then check the following, in the following order
        /// 1. That Zone A is below the minimum agents, if it is then assign to this zone
        /// 2. If Zone A has the minimum agents, check that all zones have the minimum agents, if they don't then shuffle 
        ///     an agent from Zone A to Zone B, this should cause a dominoe effect that causes the end zone to get filled
        /// 3. If Zone A is at maximum capacity, then shuffle an agent that is in Zone A to Zone B
        /// 
        /// Note: "Shuffle"ing an agent up means that the agent being shuffled should request the next zone, that should cause
        /// this whole system to check again starting from Zone B

        /// The manager should also track the state of each zone and ensure that the minimum agents is always met
        /// If a zone at the start of the list (high priority) is below the minimum agents, then the manager should shuffle
        /// agents backwards to fill the zone, this should be the criteria for picking the starting agent
        /// 1. Get the first zone in the list that has less than the minimum agents, if one is found then send an agent in the zone forward back,
        ///    if there are no agents in the next zone, then continue the loop until we find an agent to send back (or forward depending on which
        ///    zone has ones to spare)

        /// The system should work kind of like a glass where if Zone B doesn't have the minimum agents, then Zone C should have 0 agents.
        /// That allows for some assumptions during our checks above
    }
}
