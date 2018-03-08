using Sansar.Simulation;
using System;
using Sansar.Script;
using Sansar;

// This is an example script used for research.
public class ExampleScript : SceneObjectScript
{
    [DefaultValue("Example Default Value")]
    [DisplayName("Example Display Name")]
    public readonly string ExampleStringPropertyDefaultDisplay;
    
    [DisplayName("Example Display Name")]
    public readonly string ExampleStringPropertyDisplay;
    
    [DefaultValue("Example Default Value")]
    public readonly string ExampleStringPropertyDefault;

    public readonly string ExampleStringProperty;

    [DefaultValue(42)]
    [DisplayName("Example Display Name")]
    public readonly int ExampleIntPropertyDefaultDisplay;
    
    [DefaultValue(42)]
    public readonly int ExampleIntPropertyDefault;
    
    [DisplayName("Example Display Name")]
    public readonly int ExampleIntPropertyDisplay;
    
    public readonly int ExampleIntProperty;
        
    [DefaultValue(42.0)]
    [DisplayName("Example Display Name")]
    public readonly double ExampleDoublePropertyDefault42Display;
    
    [DefaultValue(42.0)]
    public readonly double ExampleDoublePropertyDefault42;
    
    [DisplayName("Example Display Name")]
    public readonly double ExampleDoublePropertyDisplay;

    public readonly double ExampleDoubleProperty;
    
    public readonly int ExampleIntPropertyNoDefault;
    
    public override void Init()
    {
        Log.Write("Simple example script for research purposes.");
        float warning = (float)ExampleIntPropertyDisplay;
    }
}
