using System.Collections.ObjectModel;
using Moq;
using System.Linq;
using TemplateGenerator.Description;
using Xunit;

namespace TemplateGenerator.Tests.Description
{
    public class ClassDescriptionTest
    {
        private Mock<IDescription> _moqDescription;

        [Fact]
        public void ShouldContainTableNameWhenInstantiated()
        {
            // Arrange
            _moqDescription = new Mock<IDescription>();
            _moqDescription.SetupGet(d => d.TableName).Returns("TableName");

            // Act

            // Assert
            Assert.Same("TableName", _moqDescription.Object.TableName);
        }

        [Fact]
        public void ShouldContainNameWhenInstantiated()
        {
            // Arrange
            _moqDescription = new Mock<IDescription>();
            _moqDescription.SetupGet(d => d.Name).Returns("Name");

            // Act

            // Assert
            Assert.Same("Name", _moqDescription.Object.Name);
        }        

        [Fact]   
        public void ShouldContainClassDescriptionTextWhenInstantiated()
        {
            // Arrange
            _moqDescription = new Mock<IDescription>();
            _moqDescription.SetupGet(d => d.Description).Returns("Description");

            // Act

            // Assert
            Assert.Same("Description", _moqDescription.Object.Description);   
        }        
        
        [Fact]   
        public void ShouldContainFileFullPathWhenInstantiated()
        {
            // Arrange
            _moqDescription = new Mock<IDescription>();
            _moqDescription.SetupGet(d => d.FileFullPath).Returns(@"C:\TEMP");            

            // Act            

            // Assert
            Assert.Same(@"C:\TEMP", _moqDescription.Object.FileFullPath);   
        }
        
        [Fact]
        public void ShouldContainPropertyDescription()
        {
            // Arrange
            _moqDescription = new Mock<IDescription>();
            _moqDescription.SetupGet(d => d.Properties).Returns(GetPropertyDescription());
        
            // Act

            // Assert
            Assert.Single(_moqDescription.Object.Properties);
        }

        private static ReadOnlyCollection<IPropertyDescription> GetPropertyDescription()
        {
            return new IPropertyDescription[] {new PropertyDescription("Name", "Description", "string")}.ToList().AsReadOnly();
        }
    }
}
