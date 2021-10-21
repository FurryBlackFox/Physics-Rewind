using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Settings", menuName = "Scriptable objects/Settings", order = 1)]
public class Settings : ScriptableObject
{
    [Header("Aim")]
    [Min(0f)] public float maxAimRaycastDistance = 100f;
    [Min(0f)] public float aimPointOffset = 0.05f;
    public LayerMask aimLayerMask;
    public bool updateAimRotation = true;

    [Header("Cannon")]
    [Min(0f)] public float cannonShotForce = 100f;

    [Header("Rewindable Objects")]
    [Min(0f)] public float sleepThreshold = 0.1f;

    [Header("Debug")] 
    public Material sleepMaterial;
    public Material wakeMaterial;
}
