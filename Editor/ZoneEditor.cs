using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace KieranCoppins.PostNavigation
{
    [CustomEditor(typeof(Zone))]
    public class ZoneEditor : Editor
    {
        public void OnSceneGUI()
        {
            Zone zone = (Zone)target;
            Handles.color = Color.red;
            // Add the first point to the end so that we draw a complete loop
            List<Vector3> zonePointsToDraw = new List<Vector3>(zone.ZonePoints)
        {
            zone.ZonePoints[0]
        };

            // Handles.DrawPolyLine(zonePointsToDraw.Select(p => zone.transform.TransformPoint(p)).ToArray());

            List<Vector3> newZonePoints = new List<Vector3>();

            foreach (Vector3 point in zone.ZonePoints)
            {
                Vector3 globalPoint = zone.transform.TransformPoint(point);
                EditorGUI.BeginChangeCheck();
                Vector3 newGlobalPoint = Handles.PositionHandle(globalPoint, Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(zone, "Change Zone Point Position");
                    newZonePoints.Add(zone.transform.InverseTransformPoint(newGlobalPoint));
                }
                else
                {
                    newZonePoints.Add(point);
                }
            }

            zone.ZonePoints = newZonePoints;

            EditorGUI.BeginChangeCheck();
            Vector3 newZonePosition = Handles.PositionHandle(zone.transform.position, zone.transform.rotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(zone, "Change Zone Position");
                zone.transform.position = newZonePosition;
            }

            EditorUtility.SetDirty(zone);
        }
    }
}