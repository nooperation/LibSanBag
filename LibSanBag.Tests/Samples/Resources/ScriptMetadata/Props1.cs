using Sansar.Simulation;
using System;
using System.Collections.Generic;
using Sansar.Script;
using Sansar;

// This is an example script used for research.
public class ExampleScript : SceneObjectScript
{
    [DefaultValue(42.42)]
    [Range(1.234, 40.42)]
    [DisplayName("Example Display Name")]
    public readonly double ExampleDoubleRangeAll;
            
    [DefaultValue(42.42)]
    [DisplayName("Example Display Name")]
    public readonly double ExampleDoubleDefault42Display;
    
    [DefaultValue(42.42)]
    public readonly double ExampleDoubleDefault42;
    
    [DisplayName("Example Display Name")]
    public readonly double ExampleDoubleDisplay;

    public readonly double ExampleDouble;

    [DefaultValue("<1.2,3.4,5.6>")]
    [DisplayName("Example Display Name")]
    public Sansar.Vector ExampleVectorDefaultDisplay;

    [DefaultValue("<1.2,3.4,5.6>")]
    public Sansar.Vector ExampleVectorDefault;

    [DisplayName("Example Display Name")] 
    public Sansar.Vector ExampleVectorDisplay;

    public Sansar.Vector ExampleVector;

    public override void Init()
    {
        Log.Write("Simple example script for research purposes.");
        float warning = (float)42;
    }
}
