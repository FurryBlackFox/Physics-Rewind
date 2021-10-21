using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager S;

    public Settings settings;
    private void Awake()
    {
        if (!S)
            S = this;
    }
}
