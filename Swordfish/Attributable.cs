using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swordfish
{

public class Attributable : MonoBehaviour
{
    [SerializeField] private List<Attribute> attributes = new List<Attribute>();

    public float GetAttributeValue(Attributes attribute)
    {
        for (int i = 0; i < attributes.Count; i++)
        {
            if (attributes[i].GetAttribute() == attribute) return attributes[i].GetValue();
        }

        return 0.0f;
    }

    public float GetAttributeMax(Attributes attribute)
    {
        for (int i = 0; i < attributes.Count; i++)
        {
            if (attributes[i].GetAttribute() == attribute) return attributes[i].GetMax();
        }

        return 0.0f;
    }

    public float GetAttributePercent(Attributes attribute)
    {
        for (int i = 0; i < attributes.Count; i++)
        {
            if (attributes[i].GetAttribute() == attribute) return attributes[i].GetValue() / attributes[i].GetMax();
        }

        return 0.0f;
    }

    public Attribute GetAttribute(Attributes attribute)
    {
        for (int i = 0; i < attributes.Count; i++)
        {
            if (attributes[i].GetAttribute() == attribute) return attributes[i];
        }

        return attribute.Create();
    }

    public bool HasAttribute(Attributes attribute)
    {
        for (int i = 0; i < attributes.Count; i++)
        {
            if (attributes[i].GetAttribute() == attribute) return true;
        }

        return false;
    }

    public bool AddAttribute(Attributes attribute)
    {
        if (HasAttribute(attribute)) return false;    //  The attribute exists, failed to add it

        attributes.Add( attribute.CreateDefault() );
        return true;    //  We were able to add the attribute
    }

    public bool AddAttribute(Attributes attribute, float value, float max = 0.0f)
    {
        if (HasAttribute(attribute)) return false;    //  The attribute exists, failed to add it

        attributes.Add( attribute.Create(value, max) );
        return true;    //  We were able to add the attribute
    }

    public bool RemoveAttribute(Attributes attribute)
    {
        for (int i = 0; i < attributes.Count; i++)
        {
            if (attributes[i].GetAttribute() == attribute) attributes.RemoveAt(i); return true;
        }

        return false;
    }
}

}