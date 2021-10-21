using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CashedState
{
    public Vector3 position;
    public Quaternion rotation;
}

[RequireComponent(typeof(Rigidbody), typeof(MeshRenderer))]
public class RewindableObject : MonoBehaviour
{
    public static event Action OnRewindingFinished;

    private static List<RewindableObject> rewindableObjects;
    private static bool isAllObjectsAsleep = true;

    private Rigidbody cashedRigidbody;
    protected MeshRenderer meshRenderer;

    protected Settings settings;
    protected Stack<CashedState> cashedStates;

    protected bool isWriting = false;

    #region UnityEvents

    private void Awake()
    {
        rewindableObjects ??= new List<RewindableObject>();
        rewindableObjects.Add(this);
        
        settings = GameManager.S.settings;

        cashedRigidbody = GetComponent<Rigidbody>();
        cashedRigidbody.sleepThreshold = settings.sleepThreshold;
        cashedRigidbody.Sleep();
        
        meshRenderer = GetComponent<MeshRenderer>();
        
        cashedStates = new Stack<CashedState>();
    }

    private void OnEnable()
    {
        Cannon.OnShotPerformed += StartWritingStates;
        InputEvents.OnRewindButtonDown += CheckForRewinding;
    }

    private void OnDisable()
    {
        Cannon.OnShotPerformed -= StartWritingStates;
        InputEvents.OnRewindButtonDown -= CheckForRewinding;
    }

    private void FixedUpdate()
    {
        meshRenderer.sharedMaterial = cashedRigidbody.IsSleeping() ? settings.sleepMaterial : settings.wakeMaterial;
    }
    
    
    private void LateUpdate()
    {
        isAllObjectsAsleep = true;
        foreach (var rewindableObject in rewindableObjects)
        {
            if (rewindableObject.cashedRigidbody.IsSleeping())
                continue;
            
            isAllObjectsAsleep = false;
            break;
        }
    }

    #endregion
    

    #region Writing
    
    private void StartWritingStates()
    {
        StartCoroutine(StatesWritingCoroutine());
    }
    
    private IEnumerator StatesWritingCoroutine()
    {
        isWriting = true;
        do
        {
            WriteDownState();
            yield return new WaitForFixedUpdate();
        } while (!isAllObjectsAsleep);
        isWriting = false;
    }
    
    protected void WriteDownState()
    {
        var currentState = new CashedState
        {
            position = transform.position,
            rotation = transform.rotation
        };
        
        cashedStates.Push(currentState);
    }
    
    #endregion


    #region Rewinding

    private void CheckForRewinding()
    {
        if(!isWriting)
            StartCoroutine(RewindObjectCoroutine());
    }

    private IEnumerator RewindObjectCoroutine()
    {
        cashedRigidbody.isKinematic = true;
        while (cashedStates.Count > 0)
        {
            RewindState();
            yield return new WaitForFixedUpdate();
        }
        cashedRigidbody.isKinematic = false;
        OnRewindingFinished?.Invoke();
    }
    
    protected void RewindState()
    {
        var currentState = cashedStates.Pop();
        transform.position = currentState.position;
        transform.rotation = currentState.rotation;
    }
    
    

    #endregion
    

}
