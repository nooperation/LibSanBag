using Sansar.Simulation;
using System;
using System.Collections.Generic;
using Sansar.Script;
using Sansar;

// This is an example script used for research.
public class Multi1_B : SceneObjectScript
{
    [Tooltip("SECOND Helpful text")]
    public readonly string SecondExampleStringWithTooltip;

    public override void Init()
    {
        Log.Write("Second Simple example script for research purposes.");
    }
}
