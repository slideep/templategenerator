using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TemplateGenerator.Template;
using Xunit;

namespace TemplateGenerator.Tests.Template;

public class TemplateAssetProviderTest
{
    [Fact]
    public void FileSystemTemplateAssetProviderLoadsAssetsAndDescriptors()
    {
        var tempDirectory = Path.Combine(Path.GetTempPath(), "TemplateGenerator.Tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            Directory.CreateDirectory(Path.Combine(tempDirectory, "nested"));
            File.WriteAllText(Path.Combine(tempDirectory, "alpha.template.json"), CreateTemplateJson("Alpha", "Entity"));
            File.WriteAllText(Path.Combine(tempDirectory, "nested", "beta.template.json"), CreateTemplateJson("Beta", "Controller"));

            var provider = new FileSystemTemplateAssetProvider(tempDirectory);

            var descriptors = provider.GetDescriptors().ToArray();
            var assets = descriptors.Select(provider.LoadAsset).ToArray();

            Assert.Equal(2, descriptors.Length);
            Assert.Equal(2, assets.Length);
            Assert.All(descriptors, descriptor => Assert.Equal(TemplateAssetSourceKind.FileSystem, descriptor.SourceKind));
            Assert.Equal(["Alpha", "Beta"], assets.Select(static asset => asset.Name).ToArray());
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void TemplateRegistryCanLoadFromFileSystemProvider()
    {
        var tempDirectory = Path.Combine(Path.GetTempPath(), "TemplateGenerator.Tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            File.WriteAllText(Path.Combine(tempDirectory, "entity.template.json"), CreateTemplateJson("EntityTemplate", "Entity"));

            var provider = new FileSystemTemplateAssetProvider(tempDirectory);
            var registry = new TemplateRegistry();

            registry.LoadFrom(provider);

            Assert.True(registry.TryGet("EntityTemplate", out var asset));
            Assert.NotNull(asset);
            Assert.Equal(TemplateAssetSourceKind.FileSystem, asset!.Descriptor.SourceKind);
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void TemplateRegistryLoadFromIsAtomicWhenProviderLoadFails()
    {
        var registry = new TemplateRegistry();
        registry.Register(CreateTemplateAsset("Existing", TemplateDescriptionTypes.Entity));

        var provider = new DelegateTemplateAssetProvider(
            descriptors:
            [
                new TemplateAssetDescriptor("First", TemplateAssetSourceKind.InMemory, "mem:first"),
                new TemplateAssetDescriptor("Broken", TemplateAssetSourceKind.InMemory, "mem:broken")
            ],
            loadAsset: descriptor => descriptor.SourceId switch
            {
                "mem:first" => CreateTemplateAsset("First", TemplateDescriptionTypes.Entity),
                "mem:broken" => throw new TemplateAssetLoadException(
                    TemplateAssetLoadErrorCode.InvalidJson,
                    TemplateAssetSourceKind.InMemory,
                    descriptor.SourceId,
                    "broken",
                    innerException: new System.Text.Json.JsonException("broken")),
                _ => throw new InvalidOperationException()
            });

        var exception = Assert.Throws<TemplateAssetLoadException>(() => registry.LoadFrom(provider));

        Assert.Equal(TemplateAssetLoadErrorCode.InvalidJson, exception.ErrorCode);
        Assert.Equal(["Existing"], registry.Assets.Select(static asset => asset.Name).ToArray());
        Assert.False(registry.TryGet("First", out _));
    }

    [Fact]
    public void TemplateRegistryLoadFromIsAtomicWhenBatchContainsDuplicateNames()
    {
        var registry = new TemplateRegistry();

        var provider = new DelegateTemplateAssetProvider(
            descriptors:
            [
                new TemplateAssetDescriptor("One", TemplateAssetSourceKind.InMemory, "mem:1"),
                new TemplateAssetDescriptor("Two", TemplateAssetSourceKind.InMemory, "mem:2")
            ],
            loadAsset: _ => CreateTemplateAsset("Duplicate", TemplateDescriptionTypes.Entity));

        var exception = Assert.Throws<InvalidOperationException>(() => registry.LoadFrom(provider));

        Assert.Contains("Duplicate", exception.Message, StringComparison.Ordinal);
        Assert.Empty(registry.Assets);
    }

    [Fact]
    public void EmbeddedResourceTemplateAssetProviderLoadsSampleTemplatePackAssets()
    {
        var provider = new EmbeddedResourceTemplateAssetProvider(
            typeof(TemplateAsset).Assembly,
            "TemplateGenerator.TemplatePacks.Sample.");

        var descriptors = provider.GetDescriptors().ToArray();
        var assets = descriptors.Select(provider.LoadAsset).ToArray();

        Assert.Equal(2, descriptors.Length);
        Assert.Equal(2, assets.Length);
        Assert.Contains(descriptors, descriptor => descriptor.Name == "SampleEntity");
        Assert.Contains(descriptors, descriptor => descriptor.Name == "SampleController");
        Assert.All(descriptors, descriptor => Assert.Equal(TemplateAssetSourceKind.EmbeddedResource, descriptor.SourceKind));
    }

    [Fact]
    public void TemplateAssetNormalizesDescriptorNameToMatchAssetName()
    {
        var descriptor = new TemplateAssetDescriptor("DifferentName", TemplateAssetSourceKind.InMemory, "mem:test");
        var asset = new TemplateAsset(
            "CanonicalName",
            TemplateDescriptionTypes.Entity,
            "class @class@ {}",
            "@datatype@ @name@",
            string.Empty,
            string.Empty,
            descriptor);

        Assert.Equal("CanonicalName", asset.Name);
        Assert.Equal("CanonicalName", asset.Descriptor.Name);
        Assert.Equal(TemplateAssetSourceKind.InMemory, asset.Descriptor.SourceKind);
        Assert.Equal("mem:test", asset.Descriptor.SourceId);
    }

    [Fact]
    public void FileSystemTemplateAssetProviderThrowsStructuredExceptionForInvalidJson()
    {
        var tempDirectory = Path.Combine(Path.GetTempPath(), "TemplateGenerator.Tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            var filePath = Path.Combine(tempDirectory, "broken.template.json");
            File.WriteAllText(filePath, "{ this is not json");

            var provider = new FileSystemTemplateAssetProvider(tempDirectory);
            var descriptor = Assert.Single(provider.GetDescriptors());

            var exception = Assert.Throws<TemplateAssetLoadException>(() => provider.LoadAsset(descriptor));

            Assert.Equal(TemplateAssetLoadErrorCode.InvalidJson, exception.ErrorCode);
            Assert.Equal(TemplateAssetSourceKind.FileSystem, exception.SourceKind);
            Assert.Equal(filePath, exception.SourceId);
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    [Fact]
    public void EmbeddedResourceTemplateAssetProviderThrowsStructuredExceptionForMissingResource()
    {
        var provider = new EmbeddedResourceTemplateAssetProvider(typeof(TemplateAsset).Assembly);
        var descriptor = new TemplateAssetDescriptor(
            "Missing",
            TemplateAssetSourceKind.EmbeddedResource,
            "TemplateGenerator.TemplatePacks.Sample.DoesNotExist.template.json");

        var exception = Assert.Throws<TemplateAssetLoadException>(() => provider.LoadAsset(descriptor));

        Assert.Equal(TemplateAssetLoadErrorCode.ResourceNotFound, exception.ErrorCode);
        Assert.Equal(TemplateAssetSourceKind.EmbeddedResource, exception.SourceKind);
        Assert.Equal(descriptor.SourceId, exception.SourceId);
    }

    [Fact]
    public void FileSystemTemplateAssetProviderThrowsStructuredExceptionForInvalidDescriptionType()
    {
        var tempDirectory = Path.Combine(Path.GetTempPath(), "TemplateGenerator.Tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            File.WriteAllText(
                Path.Combine(tempDirectory, "invalid-type.template.json"),
                CreateTemplateJson("BadType", "NotAType"));

            var provider = new FileSystemTemplateAssetProvider(tempDirectory);
            var descriptor = Assert.Single(provider.GetDescriptors());

            var exception = Assert.Throws<TemplateAssetLoadException>(() => provider.LoadAsset(descriptor));

            Assert.Equal(TemplateAssetLoadErrorCode.InvalidDescriptionType, exception.ErrorCode);
            Assert.Equal(TemplateAssetSourceKind.FileSystem, exception.SourceKind);
            Assert.Equal("descriptionType", exception.FieldName);
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
        }
    }

    private static string CreateTemplateJson(string name, string descriptionType)
    {
        return $$"""
            {
              "name": "{{name}}",
              "descriptionType": "{{descriptionType}}",
              "classTemplate": "public class @class@ { }",
              "propertyTemplate": "public @datatype@ @name@ { get; set; }",
              "parameterTemplate": "",
              "sovitusParameterTemplate": ""
            }
            """;
    }

    private static TemplateAsset CreateTemplateAsset(string name, TemplateDescriptionTypes descriptionType)
        => new(
            name,
            descriptionType,
            "public class @class@ { }",
            "public @datatype@ @name@ { get; set; }",
            string.Empty,
            string.Empty);

    private sealed class DelegateTemplateAssetProvider(
        IReadOnlyList<TemplateAssetDescriptor> descriptors,
        Func<TemplateAssetDescriptor, TemplateAsset> loadAsset)
        : ITemplateAssetProvider
    {
        public IEnumerable<TemplateAssetDescriptor> GetDescriptors() => descriptors;

        public TemplateAsset LoadAsset(TemplateAssetDescriptor descriptor) => loadAsset(descriptor);
    }
}
