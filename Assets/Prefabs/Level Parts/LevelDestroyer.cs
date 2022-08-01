using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDestroyer : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject, 40f);
    }
}
