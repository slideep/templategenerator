using System;
using System.IO;
using TemplateGenerator.Builder;
using TemplateGenerator.Description;
using TemplateGenerator.Template;
using Xunit;

namespace TemplateGenerator.Tests.Builder;

public class TemplateBuilderBehaviorTest
{
    [Fact]
    public void XmlDescriptionBuilderThrowsStructuredErrorWhenRequiredPropertyElementIsMissing()
    {
        var tempDirectory = CreateTempDirectory();

        try
        {
            var templatePath = Path.Combine(tempDirectory, "MissingDescription.xml");
            File.WriteAllText(templatePath, BuildXmlTemplate(includeDescriptionProperty: false, includeDescriptionValue: true));

            var builder = new XmlDescriptionBuilder(tempDirectory);

            var exception = Assert.Throws<TemplateParseException>(() => _ = builder.BuiltTemplates);

            Assert.Equal(TemplateParseErrorCode.MissingPropertyElement, exception.ErrorCode);
            Assert.Equal(MetadataParameters.Description, exception.PropertyName);
            Assert.Equal(templatePath, exception.FileFullPath);
        }
        finally
        {
            Directory.Delete(tempDirectory, true);
        }
    }

    [Fact]
    public void XmlDescriptionBuilderThrowsStructuredErrorWhenRequiredPropertyValueAttributeIsMissing()
    {
        var tempDirectory = CreateTempDirectory();

        try
        {
            var templatePath = Path.Combine(tempDirectory, "MissingDescriptionValue.xml");
            File.WriteAllText(templatePath, BuildXmlTemplate(includeDescriptionProperty: true, includeDescriptionValue: false));

            var builder = new XmlDescriptionBuilder(tempDirectory);

            var exception = Assert.Throws<TemplateParseException>(() => _ = builder.BuiltTemplates);

            Assert.Equal(TemplateParseErrorCode.MissingPropertyValueAttribute, exception.ErrorCode);
            Assert.Equal(MetadataParameters.Description, exception.PropertyName);
            Assert.Equal(templatePath, exception.FileFullPath);
        }
        finally
        {
            Directory.Delete(tempDirectory, true);
        }
    }

    [Fact]
    public void XmlDescriptionBuilderThrowsStructuredErrorWhenClassNodeIsMissing()
    {
        var tempDirectory = CreateTempDirectory();

        try
        {
            var templatePath = Path.Combine(tempDirectory, "MissingXmlClassNode.xml");
            File.WriteAllText(
                templatePath,
                """
                <?xml version="1.0" encoding="utf-8"?>
                <classTemplate version="0.1">
                  <notClass />
                </classTemplate>
                """);

            var builder = new XmlDescriptionBuilder(tempDirectory);

            var exception = Assert.Throws<TemplateParseException>(() => _ = builder.BuiltTemplates);

            Assert.Equal(TemplateParseErrorCode.MissingRootNode, exception.ErrorCode);
            Assert.Equal(templatePath, exception.FileFullPath);
        }
        finally
        {
            Directory.Delete(tempDirectory, true);
        }
    }

    [Fact]
    public void XmlDescriptionBuilderDoesNotStopAtFirstNonElementPropertyDescription()
    {
        var tempDirectory = CreateTempDirectory();

        try
        {
            var templatePath = Path.Combine(tempDirectory, "XmlMixedPropertyDescriptions.xml");
            File.WriteAllText(templatePath, BuildXmlTemplateWithMixedPropertyDescriptions());

            var builder = new XmlDescriptionBuilder(tempDirectory);

            var description = Assert.IsType<XmlDescription>(Assert.Single(builder.BuiltTemplates.Values));

            Assert.Collection(
                description.Properties,
                property => Assert.Equal("Code", property.Name),
                property => Assert.Equal("Name", property.Name));
        }
        finally
        {
            Directory.Delete(tempDirectory, true);
        }
    }

    [Fact]
    public void ClassDescriptionBuilderThrowsStructuredErrorWhenRequiredClassPropertyElementIsMissing()
    {
        var tempDirectory = CreateTempDirectory();

        try
        {
            var templatePath = Path.Combine(tempDirectory, "MissingClassDescription.xml");
            var templateXml = BuildClassTemplate()
                .Replace("""<property name="$description" value="Description for the class." />""", string.Empty, StringComparison.Ordinal);
            File.WriteAllText(templatePath, templateXml);

            var builder = new ClassDescriptionBuilder(tempDirectory);

            var exception = Assert.Throws<TemplateParseException>(() => _ = builder.BuiltTemplates);

            Assert.Equal(TemplateParseErrorCode.MissingPropertyElement, exception.ErrorCode);
            Assert.Equal(MetadataParameters.Description, exception.PropertyName);
            Assert.Equal(templatePath, exception.FileFullPath);
        }
        finally
        {
            Directory.Delete(tempDirectory, true);
        }
    }

    [Fact]
    public void ClassDescriptionBuilderThrowsStructuredErrorWhenClassNodeIsMissing()
    {
        var tempDirectory = CreateTempDirectory();

        try
        {
            var templatePath = Path.Combine(tempDirectory, "MissingClassNode.xml");
            File.WriteAllText(
                templatePath,
                """
                <?xml version="1.0" encoding="utf-8"?>
                <classTemplate version="0.1">
                  <notClass />
                </classTemplate>
                """);

            var builder = new ClassDescriptionBuilder(tempDirectory);

            var exception = Assert.Throws<TemplateParseException>(() => _ = builder.BuiltTemplates);

            Assert.Equal(TemplateParseErrorCode.MissingRootNode, exception.ErrorCode);
            Assert.Null(exception.PropertyName);
            Assert.Equal(templatePath, exception.FileFullPath);
        }
        finally
        {
            Directory.Delete(tempDirectory, true);
        }
    }

    [Fact]
    public void ClassDescriptionBuilderThrowsStructuredErrorWhenPropertyValueAttributeIsMissing()
    {
        var tempDirectory = CreateTempDirectory();

        try
        {
            var templatePath = Path.Combine(tempDirectory, "MissingPropertyDatatypeValue.xml");
            var templateXml = BuildClassTemplate()
                .Replace("""<property name="$datatype" value="guid" />""", """<property name="$datatype" />""", StringComparison.Ordinal);
            File.WriteAllText(templatePath, templateXml);

            var builder = new ClassDescriptionBuilder(tempDirectory);

            var exception = Assert.Throws<TemplateParseException>(() => _ = builder.BuiltTemplates);

            Assert.Equal(TemplateParseErrorCode.MissingPropertyValueAttribute, exception.ErrorCode);
            Assert.Equal(MetadataParameters.DataType, exception.PropertyName);
            Assert.Equal(templatePath, exception.FileFullPath);
        }
        finally
        {
            Directory.Delete(tempDirectory, true);
        }
    }

    [Fact]
    public void TemplateDescriptionTypesEverythingCombinesAllFlags()
    {
        Assert.NotEqual(TemplateDescriptionTypes.None, TemplateDescriptionTypes.Everything);
        Assert.True(TemplateDescriptionTypes.Everything.HasFlag(TemplateDescriptionTypes.Entity));
        Assert.True(TemplateDescriptionTypes.Everything.HasFlag(TemplateDescriptionTypes.Constant));
        Assert.True(TemplateDescriptionTypes.Everything.HasFlag(TemplateDescriptionTypes.Controller));
        Assert.True(TemplateDescriptionTypes.Everything.HasFlag(TemplateDescriptionTypes.ControllerInterface));
        Assert.True(TemplateDescriptionTypes.Everything.HasFlag(TemplateDescriptionTypes.DataAccess));
    }

    [Fact]
    public void ClassDescriptionBuilderPopulatesImmutableMetadataFromTemplateFile()
    {
        var tempDirectory = CreateTempDirectory();

        try
        {
            var templatePath = Path.Combine(tempDirectory, "ClassTemplate.xml");
            File.WriteAllText(templatePath, BuildClassTemplate());

            var builder = new ClassDescriptionBuilder(tempDirectory);

            var description = Assert.IsType<ClassDescription>(Assert.Single(builder.BuiltTemplates.Values));

            Assert.Equal("airports", description.TableName);
            Assert.Equal(templatePath, description.FileFullPath);
            Assert.Null(typeof(ClassDescription).GetProperty(nameof(ClassDescription.TableName))?.SetMethod);
            Assert.Null(typeof(ClassDescription).GetProperty(nameof(ClassDescription.FileFullPath))?.SetMethod);
        }
        finally
        {
            Directory.Delete(tempDirectory, true);
        }
    }

    [Fact]
    public void XmlDescriptionBuilderPopulatesImmutableMetadataFromTemplateFile()
    {
        var tempDirectory = CreateTempDirectory();

        try
        {
            var templatePath = Path.Combine(tempDirectory, "XmlTemplate.xml");
            File.WriteAllText(templatePath, BuildXmlTemplate(includeDescriptionProperty: true, includeDescriptionValue: true));

            var builder = new XmlDescriptionBuilder(tempDirectory);

            var description = Assert.IsType<XmlDescription>(Assert.Single(builder.BuiltTemplates.Values));

            Assert.Equal("airports", description.TableName);
            Assert.Equal("urn:test:airports", description.Namespace);
            Assert.Equal(templatePath, description.FileFullPath);
            Assert.Null(typeof(XmlDescription).GetProperty(nameof(XmlDescription.TableName))?.SetMethod);
            Assert.Null(typeof(XmlDescription).GetProperty(nameof(XmlDescription.FileFullPath))?.SetMethod);
            Assert.Null(typeof(XmlDescription).GetProperty(nameof(XmlDescription.Namespace))?.SetMethod);
        }
        finally
        {
            Directory.Delete(tempDirectory, true);
        }
    }

    private static string CreateTempDirectory()
    {
        var path = Path.Combine(Path.GetTempPath(), "TemplateGenerator.Tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(path);
        return path;
    }

    private static string BuildClassTemplate()
    {
        return """
            <?xml version="1.0" encoding="utf-8"?>
            <classTemplate version="0.1">
              <class>
                <property name="$metainformation" value="Class" />
                <property name="$name" value="Entity" />
                <property name="$description" value="Description for the class." />
                <property name="$classtype" value="DataAccess" />
                <property name="$tablename" value="airports" />
                <propertyDescription>
                  <property name="$metainformation" value="$attribute" />
                  <property name="$description" value="A unique identification." />
                  <property name="$name" value="Id" />
                  <property name="$visibility" value="$public" />
                  <property name="$datatype" value="guid" />
                  <property name="$defaultvalue" value="guid" />
                </propertyDescription>
              </class>
            </classTemplate>
            """;
    }

    private static string BuildXmlTemplate(bool includeDescriptionProperty, bool includeDescriptionValue)
    {
        var descriptionProperty = includeDescriptionProperty
            ? includeDescriptionValue
                ? """<property name="$description" value="Airport feed." />"""
                : """<property name="$description" />"""
            : string.Empty;

        return $"""
            <?xml version="1.0" encoding="utf-8"?>
            <classTemplate version="0.1">
              <class>
                <property name="$metainformation" value="Xml" />
                <property name="$name" value="AirportFeed" />
                {descriptionProperty}
                <property name="$tablename" value="airports" />
                <property name="$namespace" value="urn:test:airports" />
                <propertyDescription>
                  <property name="$metainformation" value="Element" />
                  <property name="$name" value="Code" />
                </propertyDescription>
              </class>
            </classTemplate>
            """;
    }

    private static string BuildXmlTemplateWithMixedPropertyDescriptions()
    {
        return """
            <?xml version="1.0" encoding="utf-8"?>
            <classTemplate version="0.1">
              <class>
                <property name="$metainformation" value="Xml" />
                <property name="$name" value="AirportFeed" />
                <property name="$description" value="Airport feed." />
                <property name="$tablename" value="airports" />
                <property name="$namespace" value="urn:test:airports" />
                <propertyDescription>
                  <property name="$metainformation" value="Element" />
                  <property name="$name" value="Code" />
                </propertyDescription>
                <propertyDescription>
                  <property name="$metainformation" value="Attribute" />
                  <property name="$name" value="Ignored" />
                </propertyDescription>
                <propertyDescription>
                  <property name="$metainformation" value="Element" />
                  <property name="$name" value="Name" />
                </propertyDescription>
              </class>
            </classTemplate>
            """;
    }
}
