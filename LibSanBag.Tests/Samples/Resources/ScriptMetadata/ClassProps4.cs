using Sansar.Simulation;
using System;
using System.Collections.Generic;
using Sansar.Script;
using Sansar;

[assembly: Tooltip("Assembly tooltip goes here")]

// This is an example script used for research.
public class ClasProps4 : SceneObjectScript
{
    public readonly string ExampleStringWithTooltip;

    public override void Init()
    {
        Log.Write("Simple example script for research purposes.2");
    }
}
