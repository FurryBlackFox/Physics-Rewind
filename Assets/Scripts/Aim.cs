using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Aim : MonoBehaviour
{
    public static event Action<Vector3> OnFire;
    public static event Action<Vector3> OnAimPositionUpdated;
    
    private Settings settings;
    private MeshRenderer meshRenderer;
    private Camera currentCamera;
    private void Awake()
    {
        settings = GameManager.S.settings;
        meshRenderer = GetComponent<MeshRenderer>();
        currentCamera = Camera.main;
    }

    private void OnEnable()
    {
        InputEvents.OnAimButtonDown += OnAimStarted;
        InputEvents.OnAimButtonStays += UpdateAimPosition;
        InputEvents.OnAimButtonUp += OnAimFinished;
    }

    private void OnDisable()
    {
        InputEvents.OnAimButtonDown -= OnAimStarted;
        InputEvents.OnAimButtonStays -= UpdateAimPosition;
        InputEvents.OnAimButtonUp -= OnAimFinished;

    }
    
    private void OnAimStarted()
    {
        meshRenderer.enabled = true;
    }

    private void OnAimFinished()
    {
        meshRenderer.enabled = false;
        OnFire?.Invoke(transform.position);
    }

    private void UpdateAimPosition()
    {
        var ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hitInfo, settings.maxAimRaycastDistance, settings.aimLayerMask))
        {
            transform.position = hitInfo.point + hitInfo.normal * settings.aimPointOffset;
            OnAimPositionUpdated?.Invoke(transform.position);
            if(settings.updateAimRotation)
                transform.up = (Cannon.Position - transform.position).normalized;
        }
    }
}
