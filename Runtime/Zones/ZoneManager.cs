using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KieranCoppins.PostNavigation
{

    public class ZoneManager : MonoBehaviour
    {
        public static ZoneManager Instance { get; private set; }

        public List<Zone> Zones { get => zones; private set => zones = value; }
        [SerializeField] List<Zone> zones;
        [SerializeField] Vector3 combatVector = Vector3.forward;

        private List<IZoneableAgent> agents = new List<IZoneableAgent>();

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

            // Get all agents in the scene
            foreach (var rootObject in gameObjects)
            {
                agents.AddRange(rootObject.GetComponentsInChildren<IZoneableAgent>());
            }

            StartCoroutine(CheckZoneStates());
        }

        /// <summary>
        /// Gets the closest zone to the agent that isnt the zone they are currently in
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public Zone GetClosestZoneToAgent(IZoneableAgent agent)
        {
            float dst = float.MaxValue;
            Zone closestZone = null;
            foreach (var zone in Zones)
            {
                if (agent.AssignedZone == zone) continue;

                float newDst = Vector3.Distance(agent.Position, zone.transform.position);
                if (newDst < dst)
                {
                    dst = newDst;
                    closestZone = zone;
                }
            }
            return closestZone;
        }

        public void RequestZone(Zone zone, IZoneableAgent agent)
        {
            if (zone == null) return;

            if (zone.CurrentAgents < zone.MinAgents)
            {
                // Assign the agent to the zone
                zone.AssignAgent(agent);
                return;
            }

            if (!AllZonesHaveMinimumAgents() || zone.CurrentAgents >= zone.MaxAgents)
            {
                ShuffleAgent(zone);

                // Assign the agent to the zone
                zone.AssignAgent(agent);
                return;
            }

            if (zone.CurrentAgents < zone.MaxAgents)
            {
                // Assign the agent to the zone
                zone.AssignAgent(agent);
                return;
            }
        }

        private void ShuffleAgent(Zone zone)
        {
            // Get the next zone in the list
            int currentZoneIndex = Zones.IndexOf(zone);

            if (currentZoneIndex != Zones.Count - 1)
            {
                // Move an agent in the zone being requested to the next zone
                Zone nextZone = Zones[currentZoneIndex + 1];
                IZoneableAgent agentInZone = zone.GetAgentInZone();
                RequestZone(nextZone, agentInZone);
                return;
            }

            // If we are at the end of the zones log a warning
            Debug.LogWarning("No zones available to shuffle agents to, all zones are at maximum capacity.");
        }

        private bool AllZonesHaveMinimumAgents()
        {
            return Zones.All(zone => zone.CurrentAgents >= zone.MinAgents);
        }

        private IEnumerator CheckZoneStates()
        {
            while (true)
            {

                foreach (var zone in Zones)
                {
                    if (zone.CurrentAgents < zone.MinAgents)
                    {
                        // Shuffle agents backwards to fill the zone
                        int currentZoneIndex = Zones.IndexOf(zone);

                        if (currentZoneIndex < Zones.Count - 1)
                        {
                            // Move an agent in the next zone to this zone
                            Zone nextZone = Zones[currentZoneIndex + 1];
                            IZoneableAgent agentInZone = nextZone.GetAgentInZone();
                            if (agentInZone == null) break;
                            RequestZone(zone, agentInZone);
                            break;
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
        /// 1. Get the first zone in the list that has more than 1 the minimum agents, if one is found then send that agent back
        /// 2. If we get to the last zone, then shuffle an agent from the last zone back
        /// 3. If we get to a zone with 0 agents, then shuffle an agent from the previous zone back

        /// The system should work kind of like a glass where if Zone B doesn't have the minimum agents, then Zone C should have 0 agents.
        /// That allows for some assumptions during our checks above
    }
}
