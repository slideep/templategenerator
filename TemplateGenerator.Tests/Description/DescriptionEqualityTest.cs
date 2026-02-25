using System;
using System.Collections.Generic;
using TemplateGenerator.Description;
using Xunit;

namespace TemplateGenerator.Tests.Description;

public class DescriptionEqualityTest
{
    [Fact]
    public void PropertyDescriptionEqualityUsesName()
    {
        var left = new PropertyDescription("Id", "Left", "string");
        var right = new PropertyDescription("Id", "Right", "int");

        Assert.True(left.Equals((IDescription?)right));
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
        Assert.Equal(0, left.CompareTo(right));
    }

    [Fact]
    public void PropertyDescriptionCompareToOrdersByNameAndTreatsNullAsLessThanSelf()
    {
        var alpha = new PropertyDescription("Alpha", "A", "string");
        var beta = new PropertyDescription("Beta", "B", "string");

        Assert.True(alpha.CompareTo(beta) < 0);
        Assert.True(beta.CompareTo(alpha) > 0);
        Assert.True(alpha.CompareTo(null) > 0);
    }

    [Fact]
    public void ClassDescriptionEqualityUsesName()
    {
        var left = new ClassDescription("Entity", "Left", Array.Empty<IPropertyDescription>(), Array.Empty<OperationDescription>());
        var right = new ClassDescription("Entity", "Right", Array.Empty<IPropertyDescription>(), Array.Empty<OperationDescription>());

        Assert.True(left.Equals((IDescription?)right));
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
        Assert.Equal(0, left.CompareTo(right));
    }

    [Fact]
    public void XmlDescriptionEqualityUsesName()
    {
        IReadOnlyList<IPropertyDescription> properties = Array.Empty<IPropertyDescription>();
        var left = new XmlDescription("Feed", "One", properties);
        var right = new XmlDescription("Feed", "Two", properties);
        var different = new XmlDescription("Other", "Two", properties);

        Assert.True(left.Equals((IDescription?)right));
        Assert.False(left.Equals((IDescription?)different));
        Assert.True(left.CompareTo(different) > 0 || left.CompareTo(different) < 0);
    }
}
