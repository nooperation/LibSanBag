using Sansar.Simulation;
using System;
using System.Collections.Generic;
using Sansar.Script;
using Sansar;

// This is an example script used for research.
public class ExampleScript : SceneObjectScript
{
    public String StringProp1;

    public override void Init()
    {
        Log.Write("Simple example script for research purposes.");
        float warning = (float)42;
    }
}
