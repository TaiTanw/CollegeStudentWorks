using System;

[AttributeUsage(AttributeTargets.Field)]
public class FieldOrderAttribute : Attribute
{
    public int Order { get; private set; }

    public FieldOrderAttribute(int order)
    {
        Order = order;
    }
}