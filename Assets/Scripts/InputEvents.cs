using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputEvents : MonoBehaviour
{
    public static event Action OnAimButtonDown;
    public static event Action OnAimButtonStays;
    public static event Action OnAimButtonUp;

    public static event Action OnRewindButtonDown;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnAimButtonDown?.Invoke();
        }
        
        if (Input.GetMouseButton(0))
        {
            OnAimButtonStays?.Invoke();
        }
        
        else if (Input.GetMouseButtonUp(0))
        {
            OnAimButtonUp?.Invoke();
        }

        if (Input.GetMouseButtonDown(1))
        {
            OnRewindButtonDown?.Invoke();
        }
    }
}
