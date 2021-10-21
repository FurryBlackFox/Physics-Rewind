using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private Rigidbody ball;

    public static event Action OnShotPerformed;
    public static Vector3 Position { get; private set; }
   
    
    private Settings settings;
    private bool canShoot = true;
    
    private void Awake()
    {
        Position = transform.position;
        settings = GameManager.S.settings;
    }

    private void OnEnable()
    {
        Aim.OnFire += MakeShot;
        Aim.OnAimPositionUpdated += UpdateRotation;
        RewindableObject.OnRewindingFinished += RechargeCannon;
    }

    private void OnDisable()
    {
        Aim.OnFire -= MakeShot;
        Aim.OnAimPositionUpdated -= UpdateRotation;
        RewindableObject.OnRewindingFinished -= RechargeCannon;
    }

    private void RechargeCannon()
    {
        canShoot = true;
    }

    private void UpdateRotation(Vector3 target)
    {
        if(!canShoot)
            return;
        
        transform.forward = (target - transform.position).normalized;
    }

    private void MakeShot(Vector3 target)
    {
        if(!canShoot)
            return;
        
        var force = transform.forward * settings.cannonShotForce;
        ball.AddForce(force, ForceMode.Impulse);
        canShoot = false;
        OnShotPerformed?.Invoke();
    }
}
