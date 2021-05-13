using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swordfish
{

public enum Attributes
{
    HEALTH
}

public static class AttributesExtensions
{
    public static Attribute Create(this Attributes attribute, float value = 1.0f, float max = 0.0f)
    {
        return new Attribute(attribute, value, max);
    }

    public static Attribute CreateDefault(this Attributes attribute)
    {
        switch (attribute)
        {
            case Attributes.HEALTH: return new Attribute(attribute, 1, 100);

            default: return new Attribute(attribute, 1.0f, 0.0f);
        }
    }
}

[Serializable]
public class Attribute
{
    [SerializeField] protected Attributes attribute;
    [SerializeField] protected float value = 1.0f;
    [SerializeField] protected float max = 1.0f;

    public Attribute(Attributes attribute, float value = 1.0f, float max = 0.0f)
    {
        this.attribute = attribute;
        this.value = value;
        if (max <= 0) max = int.MaxValue;   //  NOTE: max of 0 or less is considered uncapped
        this.max = max;
    }

    public Attributes   GetAttribute()  { return attribute; }
    public float        GetValue()      { return value; }
    public float        GetMax()        { return max; }
    public float        GetPercent()    { return value / max; }

    public Attribute SetValue(float value)          { this.value = Mathf.Clamp(value, 0, max); return this; }
    public Attribute SetValueUnsafe(float value)    { this.value = value; return this; }

    public Attribute Modify(float amount)           { this.value = Mathf.Clamp(this.value + amount, 0, max); return this; }
    public Attribute ModifyUnsafe(float amount)     { this.value += amount; return this; }

    public Attribute Max() { value = max; return this; }
    public Attribute Zero() { value = 0; return this; }

    public Attribute SetMax(float max)  { this.max = max; return this; }
    public Attribute SetMaxScaled(float max)
    {
        this.value *= max / this.max;
        SetMax(max);
        return this;
    }

    //  Operations
    public override bool Equals(System.Object obj)
    {
        Attribute atr = obj as Attribute;

        if (atr == null)
        {
            return false;
        }

        return this.attribute.Equals(atr.attribute);
    }

    public override int GetHashCode()
    {
        return value.GetHashCode() ^ max.GetHashCode() ^ attribute.GetHashCode();
    }
}

}