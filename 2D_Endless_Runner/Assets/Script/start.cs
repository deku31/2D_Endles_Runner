﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class start : MonoBehaviour
{
    // start screen
    public script scr;
    private void Start()
    {
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            scr.start = false;
        }
    }
}
