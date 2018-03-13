using Sansar.Simulation;
using System;
using System.Collections.Generic;
using Sansar.Script;
using Sansar;

// This is an example script used for research.
public class ExampleScript : SceneObjectScript
{
    [DefaultValue(1)]
    [Range(-50, 50)]
    public SByte Prop1;

    [DefaultValue(1)]
    [Range(0, 100)]
    public Byte Prop2;

    [DefaultValue(1)]
    [Range(-50, 50)]
    public Int16 Prop3;
    
    [DefaultValue(1)]
    [Range(0, 100)]
    public UInt16 Prop4;
    
    [DefaultValue(1)]
    [Range(-50, 50)]
    public Int32 Prop5;
    
    [DefaultValue(1)]
    [Range(0, 100)]
    public UInt32 Prop6;
    
    [DefaultValue(1)]
    [Range(-50, 50)]
    public Int64 Prop7;
    
    [DefaultValue(1)]
    [Range(0, 100)]
    public UInt64 Prop8;
    
    [DefaultValue(1.0f)]
    [Range(-50, 50)]
    public Single Prop9;
    
    [DefaultValue(1.0)]
    [Range(-50.0, 50)]
    public Double Prop10;
    
    public override void Init()
    {
        Log.Write("Simple example script for research purposes.");
        float warning = (float)42;
    }
}
