using Sansar.Simulation;
using System;
using System.Collections.Generic;
using Sansar.Script;
using Sansar;

// This is an example script used for research.
public class ExampleScript : SceneObjectScript
{
    [DefaultValue("[1.2,3.4,5.6,7.8]")]
    [DisplayName("Example Display Name")]
    public Sansar.Quaternion ExampleQuatDefaultDisplay;

    [DefaultValue("[1.2,3.4,5.6,7.8]")]
    public Sansar.Quaternion ExampleQuatDefault;

    [DisplayName("Example Display Name")]
    public Sansar.Quaternion ExampleQuatDisplay;

    public Sansar.Quaternion ExampleQuat;

    [DefaultValue(true)]
    [DisplayName("Example Display Name")]
    public bool ExampleBoolDefaultDisplay;

    [DefaultValue(true)]
    public bool ExampleBoolDefault;

    [DisplayName("Example Display Name")]
    public bool ExampleBoolDisplay;

    public override void Init()
    {
        Log.Write("Simple example script for research purposes.");
        float warning = (float)42;
    }
}
