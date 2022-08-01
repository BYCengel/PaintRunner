using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private bool used = false;

    public void SetUsed(bool use)
    {
        used = use;
    }
    
    public bool GetUsed()
    {
        return used;
    }
}
