using System;
using System.IO;
using TemplateGenerator;
using TemplateGenerator.Generator;
using Xunit;

namespace TemplateGenerator.Tests.Generator
{
    public class GeneratorControllerTest
    {
        [Fact]
        public void ShouldReturnEmptyDescriptionsWhenTemplateDirectoryDoesNotExist()
        {
            var missingDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            var controller = new GeneratorController(new TemplateGeneratorOptions
            {
                TemplateDirectory = missingDirectory
            });

            Assert.Empty(controller.Descriptions);
        }

        [Fact]
        public void ShouldLoadDescriptionsFromConfiguredTemplateDirectory()
        {
            var tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDirectory);

            try
            {
                var destinationTemplate = Path.Combine(tempDirectory, "ClassTemplateExample.xml");
                var templateXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<classTemplate version=""0.1"">
  <class>
    <property name=""$metainformation"" value=""Class"" />
    <property name=""$name"" value=""Entity"" />
    <property name=""$description"" value=""Description for the class."" />
    <property name=""$classtype"" value=""DataAccess"" />
    <property name=""$tablename"" value=""airports"" />
    <propertyDescription>
      <property name=""$metainformation"" value=""Attribute"" />
      <property name=""$description"" value=""A unique identification."" />
      <property name=""$name"" value=""Id"" />
      <property name=""$visibility"" value=""$public"" />
      <property name=""$datatype"" value=""guid"" />
      <property name=""$defaultvalue"" value=""guid"" />
    </propertyDescription>
  </class>
</classTemplate>";

                File.WriteAllText(destinationTemplate, templateXml);

                var controller = new GeneratorController(new TemplateGeneratorOptions
                {
                    TemplateDirectory = tempDirectory
                });

                Assert.Contains(controller.Descriptions, description => description.Name == "Entity");
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
        public void ShouldThrowWhenTemplateDirectoryIsBlank()
        {
            Assert.Throws<ArgumentException>(() => new GeneratorController(new TemplateGeneratorOptions
            {
                TemplateDirectory = " "
            }));
        }
    }
}
