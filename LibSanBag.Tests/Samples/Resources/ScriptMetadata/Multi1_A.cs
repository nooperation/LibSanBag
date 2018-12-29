﻿using Sansar.Simulation;
using System;
using System.Collections.Generic;
using Sansar.Script;
using Sansar;

// This is an example script used for research.
public class Multi1_A : SceneObjectScript
{
    [Tooltip("Helpful text")]
    public readonly string ExampleStringWithTooltip;

    public override void Init()
    {
        Log.Write("Simple example script for research purposes.");
    }
}
