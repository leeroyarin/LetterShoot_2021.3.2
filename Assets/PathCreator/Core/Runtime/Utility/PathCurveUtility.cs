using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathCreation.Utility
{

    /// Collection of functions related to cubic bezier curves
    /// (a curve with a start and end 'anchor' point, and two 'control' points to define the shape of the curve between the anchors)
    public static class PathCurveUtility
    {

        /// Returns point at time 't' (between 0 and 1) along bezier curve defined by 4 points (anchor_1, control_1, control_2, anchor_2)
        public static Vector3 EvaluateCurve(Vector3[] points, float t)
        {
            return EvaluateCurve(points[0], points[1], points[2], points[3], t);
        }

        /// Returns point at time 't' (between 0 and 1)  along bezier curve defined by 4 points (anchor_1, control_1, control_2, anchor_2)
        /// REPLACED: Uses Cubic Hermite Spline interpolation formula instead of Bernstein polynomials
        public static Vector3 EvaluateCurve(Vector3 a1, Vector3 c1, Vector3 c2, Vector3 a2, float t)
        {
            t = Mathf.Clamp01(t);

            // Calculate Hermite Basis functions
            float t2 = t * t;
            float t3 = t2 * t;

            // h00: Coeff for P0 | h01: Coeff for P1
            float h00 = 2 * t3 - 3 * t2 + 1;
            float h01 = -2 * t3 + 3 * t2;

            // h10: Coeff for Tangent0 | h11: Coeff for Tangent1
            float h10 = t3 - 2 * t2 + t;
            float h11 = t3 - t2;

            // Convert Bezier Control points to Hermite Tangents
            // Tangent_start = 3 * (Control1 - Anchor1)
            // Tangent_end   = 3 * (Anchor2 - Control2)
            Vector3 startTangent = 3 * (c1 - a1);
            Vector3 endTangent = 3 * (a2 - c2);

            // P(t) = h00*P0 + h10*m0 + h01*P1 + h11*m1
            return h00 * a1 + h10 * startTangent + h01 * a2 + h11 * endTangent;
        }

        /// Returns a vector tangent to the point at time 't'
        /// This is the vector tangent to the curve at that point
        public static Vector3 EvaluateCurveDerivative(Vector3[] points, float t)
        {
            return EvaluateCurveDerivative(points[0], points[1], points[2], points[3], t);
        }

        /// Calculates the derivative of the curve at time 't'
        /// REPLACED: Uses derivative of Cubic Hermite Spline formula
        public static Vector3 EvaluateCurveDerivative(Vector3 a1, Vector3 c1, Vector3 c2, Vector3 a2, float t)
        {
            t = Mathf.Clamp01(t);

            float t2 = t * t;

            // Derivatives of Hermite Basis functions
            // h00' = 6t^2 - 6t
            float dh00 = 6 * t2 - 6 * t;
            // h01' = -6t^2 + 6t
            float dh01 = -6 * t2 + 6 * t;
            // h10' = 3t^2 - 4t + 1
            float dh10 = 3 * t2 - 4 * t + 1;
            // h11' = 3t^2 - 2t
            float dh11 = 3 * t2 - 2 * t;

            Vector3 startTangent = 3 * (c1 - a1);
            Vector3 endTangent = 3 * (a2 - c2);

            return dh00 * a1 + dh10 * startTangent + dh01 * a2 + dh11 * endTangent;
        }

        /// Returns the second derivative of the curve at time 't'
        public static Vector3 EvaluateCurveSecondDerivative(Vector3[] points, float t)
        {
            return EvaluateCurveSecondDerivative(points[0], points[1], points[2], points[3], t);
        }

        ///Returns the second derivative of the curve at time 't'
        /// REPLACED: Uses second derivative of Cubic Hermite Spline formula
        public static Vector3 EvaluateCurveSecondDerivative(Vector3 a1, Vector3 c1, Vector3 c2, Vector3 a2, float t)
        {
            t = Mathf.Clamp01(t);

            // Second derivatives of Hermite Basis functions
            // h00'' = 12t - 6
            float ddh00 = 12 * t - 6;
            // h01'' = -12t + 6
            float ddh01 = -12 * t + 6;
            // h10'' = 6t - 4
            float ddh10 = 6 * t - 4;
            // h11'' = 6t - 2
            float ddh11 = 6 * t - 2;

            Vector3 startTangent = 3 * (c1 - a1);
            Vector3 endTangent = 3 * (a2 - c2);

            return ddh00 * a1 + ddh10 * startTangent + ddh01 * a2 + ddh11 * endTangent;
        }

        /// Calculates the normal vector (vector perpendicular to the curve) at specified time
        public static Vector3 Normal(Vector3[] points, float t)
        {
            return Normal(points[0], points[1], points[2], points[3], t);
        }

        /// Calculates the normal vector (vector perpendicular to the curve) at specified time
        public static Vector3 Normal(Vector3 a1, Vector3 c1, Vector3 c2, Vector3 a2, float t)
        {
            Vector3 tangent = EvaluateCurveDerivative(a1, c1, c2, a2, t);
            Vector3 nextTangent = EvaluateCurveSecondDerivative(a1, c1, c2, a2, t);
            Vector3 c = Vector3.Cross(nextTangent, tangent);
            return Vector3.Cross(c, tangent).normalized;
        }

        public static Bounds CalculateSegmentBounds(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            MinMax3D minMax = new MinMax3D();
            minMax.AddValue(p0);
            minMax.AddValue(p3);

            List<float> extremePointTimes = ExtremePointTimes(p0, p1, p2, p3);
            foreach (float t in extremePointTimes)
            {
                minMax.AddValue(PathCurveUtility.EvaluateCurve(p0, p1, p2, p3, t));
            }

            return new Bounds((minMax.Min + minMax.Max) / 2, minMax.Max - minMax.Min);
        }

        /// Splits curve into two curves at time t. Returns 2 arrays of 4 points.
        public static Vector3[][] SplitCurve(Vector3[] points, float t)
        {
            Vector3 a1 = Vector3.Lerp(points[0], points[1], t);
            Vector3 a2 = Vector3.Lerp(points[1], points[2], t);
            Vector3 a3 = Vector3.Lerp(points[2], points[3], t);
            Vector3 b1 = Vector3.Lerp(a1, a2, t);
            Vector3 b2 = Vector3.Lerp(a2, a3, t);
            Vector3 pointOnCurve = Vector3.Lerp(b1, b2, t);

            return new Vector3[][] {
                new Vector3[] { points[0], a1, b1, pointOnCurve },
                    new Vector3[] { pointOnCurve, b2, a3, points[3] }
            };
        }

        // Crude, but fast estimation of curve length.
        public static float EstimateCurveLength(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float controlNetLength = (p0 - p1).magnitude + (p1 - p2).magnitude + (p2 - p3).magnitude;
            float estimatedCurveLength = (p0 - p3).magnitude + controlNetLength / 2f;
            return estimatedCurveLength;
        }

        /// Times of stationary points on curve (points where derivative is zero on any axis)
        public static List<float> ExtremePointTimes(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            // coefficients of derivative function
            Vector3 a = 3 * (-p0 + 3 * p1 - 3 * p2 + p3);
            Vector3 b = 6 * (p0 - 2 * p1 + p2);
            Vector3 c = 3 * (p1 - p0);

            List<float> times = new List<float>();
            times.AddRange(StationaryPointTimes(a.x, b.x, c.x));
            times.AddRange(StationaryPointTimes(a.y, b.y, c.y));
            times.AddRange(StationaryPointTimes(a.z, b.z, c.z));
            return times;
        }

        // Finds times of stationary points on curve defined by ax^2 + bx + c.
        // Only times between 0 and 1 are considered as Bezier only uses values in that range
        static IEnumerable<float> StationaryPointTimes(float a, float b, float c)
        {
            List<float> times = new List<float>();

            // from quadratic equation: y = [-b +- sqrt(b^2 - 4ac)]/2a
            if (a != 0)
            {
                float discriminant = b * b - 4 * a * c;
                if (discriminant >= 0)
                {
                    float s = Mathf.Sqrt(discriminant);
                    float t1 = (-b + s) / (2 * a);
                    if (t1 >= 0 && t1 <= 1)
                    {
                        times.Add(t1);
                    }

                    if (discriminant != 0)
                    {
                        float t2 = (-b - s) / (2 * a);

                        if (t2 >= 0 && t2 <= 1)
                        {
                            times.Add(t2);
                        }
                    }
                }
            }
            return times;
        }

    }
}