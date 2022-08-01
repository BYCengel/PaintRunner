using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashTest : MonoBehaviour
{

    private Flash flash;

    private void Awake()
    {
        flash = GetComponent<Flash>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            flash.ColorFlash();
        }
    }
}
