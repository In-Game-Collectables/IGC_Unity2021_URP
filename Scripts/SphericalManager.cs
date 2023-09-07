using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalManager : MonoBehaviour
{
    public Vector3 GetSpiralLocation(float radius, float totalSegments, float currentSegment, float xSpeed, Vector3 center, Vector3 targetDirection,
                                    float HeightOffset = 0, float phiMin = 0, float phiMax = 1) // for camera
    {
        // Will return points around a spiral based around target
        float currentDistance = currentSegment / totalSegments;
        float theta = xSpeed * Mathf.PI * 2 * currentDistance - Mathf.Deg2Rad * Vector3.SignedAngle(targetDirection, Vector3.forward, Vector3.up); // signed angle to make it relative to the target's direction
        float phi = Mathf.PI * Mathf.Lerp(phiMin, phiMax, currentDistance);

        return center + new Vector3(0, HeightOffset, 0) + GetSphericalCoordinate(theta, phi, radius);
    }

    public Vector3 GetTopLocation(float radius, Vector3 center)
    {
        return GetSphericalCoordinate(0, 0, radius) + center;
    }

    public Vector3 GetCenterLocation(float angle, float radius, Vector3 center, Vector3 targetDirection)
    {
        float phi = Mathf.PI / 2;
        float theta = Mathf.PI * 2 * angle - Mathf.Deg2Rad * Vector3.SignedAngle(targetDirection, Vector3.forward, Vector3.up);
        Vector3 location = GetSphericalCoordinate(theta, phi, radius) + center;
        return location;
    }

    private Vector3 GetSphericalCoordinate(float theta, float phi, float radius)
    {
        theta = -(theta - Mathf.Deg2Rad * 90);
        float x = radius * Mathf.Sin(phi) * Mathf.Cos(theta);
        float z = radius * Mathf.Sin(phi) * Mathf.Sin(theta);
        float y = radius * Mathf.Cos(phi);

        return new Vector3(x, y, z);
    }
}
