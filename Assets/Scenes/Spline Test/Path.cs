using PathCreation.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace Path2D
{
    public class Path
    {
        List<Vector2> points;

        public int NumberSegments => (points.Count - 4) / 3 + 1;
        public int NumberPoints => points.Count;

        public Vector2 this[int i] => points[i];

        public Path(Vector2 center)
        {
            points = new List<Vector2>
            {
                center + Vector2.left * 3f,
                center + (Vector2.left + Vector2.up) * 1.5f,
                center + (Vector2.right + Vector2.down) * 1.5f,
                center + Vector2.right * 3f
            };
        }

        public void AddSegment(Vector2 anchorPos)
        {
            points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
            points.Add((points[points.Count - 1] + anchorPos) * 0.5f);
            points.Add(anchorPos);
        }

        /// <summary>
        /// Get the 4 points defining the cubic Bezier curve at segment i
        /// </summary>
        /// <param name="i"> the segment from which the curve needs to be derived</param>
        /// <returns> </returns>
        public Vector2[] GetPointsInSegment(int i)
        {
            return new Vector2[]
            {
                points[i*3],
                points[i*3+1],
                points[i*3+2],
                points[i*3+3]
            };
        }

        public void MovePoint(int i , Vector2 newPos)
        {
            points[i] = newPos;
        }

    }

    [CustomEditor(typeof(PathCreator))]
    public class PathEditor : Editor
    {
        PathCreator creator;
        Path path;

        GlobalDisplaySettings visualSettings;
        private void OnEnable()
        {
            creator = (PathCreator)target;
            if (path == null)
            {
                creator.CreatePath();

                visualSettings = creator.globalDisplaySettings;
            }
            path = creator.path;
        }

        private void OnSceneGUI()
        {
            Input();
            Draw();
        }
        private void Input()
        {
            Event getEvent = Event.current;
            Vector2 mousePos = HandleUtility.GUIPointToWorldRay(getEvent.mousePosition).origin;

            if (getEvent.type == EventType.MouseDown && getEvent.button == 0 && getEvent.shift)
            {
                Undo.RecordObject(creator, "AddSegment");
                path.AddSegment(mousePos);
            }
        }
        void Draw()
        {
            for (int i = 0; i < path.NumberSegments; i++)
            {
                Vector2[] p = path.GetPointsInSegment(i);
                Handles.color = visualSettings.controlLine;
                Handles.DrawLine(p[0], p[1], visualSettings.controlLineWidth);
                Handles.DrawLine(p[2], p[3], visualSettings.controlLineWidth);
                DrawBezierInterpolated(p[0], p[3], p[1], p[2], visualSettings.bezierPath, null, visualSettings.bezierLineWidth);
            }
            Handles.color = visualSettings.anchor;
            for (int i = 0; i < path.NumberPoints; i++)
            {
                Vector2 newPos;

                if (i % 3 == 0)
                {
                    newPos = Handles.FreeMoveHandle(path[i], Quaternion.identity, 0.1f, Vector3.zero, Handles.CylinderHandleCap);
                }
                else
                {
                    newPos = Handles.FreeMoveHandle(path[i], Quaternion.identity, 0.1f, Vector3.zero, Handles.RectangleHandleCap);
                }
                if (path[i] != newPos)
                {
                    Undo.RecordObject(creator, "Move Point");
                    path.MovePoint(i, newPos);
                }
            }
            


    }
        /// <summary>
        /// Draws a cubic Bezier curve using CubicBezierUtility.EvaluateCurve.
        /// Works like Handles.DrawBezier.
        /// </summary>
        public static void DrawBezierInterpolated(
            Vector3 startPosition,
            Vector3 endPosition,
            Vector3 startTangent,
            Vector3 endTangent,
            Color color,
            Texture2D texture,
            float width)
        {
            Handles.color = color;

            int segments = 40; // Smoothness
            Vector3 prevPoint = startPosition;

            for (int i = 1; i <= segments; i++)
            {
                float t = i / (float)segments;

                // Use CubicBezierUtility to get curve point
                Vector3 pointOnCurve = CubicBezierUtility.EvaluateCurve(
                    startPosition, startTangent, endTangent, endPosition, t);

                Handles.DrawAAPolyLine(width, prevPoint, pointOnCurve);
                prevPoint = pointOnCurve;
            }
        }
    }
}
