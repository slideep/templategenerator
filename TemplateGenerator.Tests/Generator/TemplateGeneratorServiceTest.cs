using System;
using System.IO;
using TemplateGenerator;
using TemplateGenerator.Description;
using TemplateGenerator.Generator;
using TemplateGenerator.Template;
using Xunit;

namespace TemplateGenerator.Tests.Generator;

public class TemplateGeneratorServiceTest
{
    [Fact]
    public void TemplateRegistryRegistersAndResolvesTemplatesByName()
    {
        var registry = new TemplateRegistry();
        var template = CreateTemplate("EntityTemplate");

        registry.Register(template);

        Assert.True(registry.TryGet("EntityTemplate", out var resolved));
        Assert.Same(template, resolved);
        Assert.Single(registry.Assets);
        Assert.Single(registry.Descriptors);
        Assert.Throws<InvalidOperationException>(() => registry.Register(CreateTemplate("EntityTemplate")));
    }

    [Fact]
    public void TemplateGeneratorServiceGeneratesClassDescriptionUsingRegisteredTemplate()
    {
        var registry = new TemplateRegistry();
        registry.Register(CreateTemplate("EntityTemplate"));

        var service = new TemplateGeneratorService(registry);
        var description = CreateClassDescription(isDataAccessClass: false);

        var result = service.GenerateDescription(description, "EntityTemplate");

        Assert.NotNull(result);
        Assert.Contains("class Airport", result, StringComparison.Ordinal);
        Assert.Contains("public string Code", result, StringComparison.Ordinal);
        Assert.DoesNotContain(MetadataParameters.ClassName, result, StringComparison.Ordinal);
    }

    [Fact]
    public void GeneratorControllerUsesInjectedTemplateRegistryForRendering()
    {
        var registry = new TemplateRegistry();
        registry.Register(CreateTemplate("InjectedTemplate"));

        var missingDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        var controller = new GeneratorController(
            new TemplateGeneratorOptions { TemplateDirectory = missingDirectory },
            registry);

        var result = controller.Generate(CreateClassDescription(isDataAccessClass: false), "InjectedTemplate");

        Assert.NotNull(result);
        Assert.Contains("class Airport", result, StringComparison.Ordinal);
    }

    [Fact]
    public void DataAccessClassGenerationBuildsSqlPlaceholdersWithoutLegacySqlBuilder()
    {
        var registry = new TemplateRegistry();
        registry.Register(CreateTemplate("DataAccessTemplate"));

        var service = new TemplateGeneratorService(registry);
        var description = CreateClassDescription(isDataAccessClass: true);

        var result = service.GenerateDescription(description, "DataAccessTemplate");

        Assert.NotNull(result);
        Assert.Contains("\"SELECT Code,Name FROM airports\"", result, StringComparison.Ordinal);
        Assert.Contains("\"INSERT INTO airports (Code,Name) VALUES (:Code,:Name)\"", result, StringComparison.Ordinal);
        Assert.Contains("\"UPDATE airports SET Code = :Code,Name = :Name\"", result, StringComparison.Ordinal);
    }

    private static ClassDescription CreateClassDescription(bool isDataAccessClass)
    {
        IPropertyDescription[] properties =
        [
            new PropertyDescription("Code", "Airport code", "string"),
            new PropertyDescription("Name", "Airport name", "string")
        ];

        return new ClassDescription(
            "Airport",
            "Airport entity",
            properties,
            Array.Empty<OperationDescription>(),
            isDataAccessClass: isDataAccessClass,
            tableName: "airports");
    }

    private static TemplateAsset CreateTemplate(string name)
    {
        return new TemplateAsset(
            name,
            TemplateDescriptionTypes.Entity,
            """
            public class @class@
            {
            // @description@
            @properties@
            @selectsql@
            @insertsql@
            @updatesql@
            @parameters@
            @sovitusParametri@
            }
            """,
            "public @datatype@ @name@ { get; set; } // @description@\n",
            string.Empty,
            string.Empty,
            new TemplateAssetDescriptor(name, TemplateAssetSourceKind.InMemory, $"test:{name}"));
    }
}
