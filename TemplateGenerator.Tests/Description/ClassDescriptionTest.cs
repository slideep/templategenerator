using System;
using System.Collections.ObjectModel;
using System.Linq;
using Moq;
using TemplateGenerator.Description;
using Xunit;

namespace TemplateGenerator.Tests.Description
{
    /// <summary>
    /// A class containing tests for <see cref="ClassDescription"/>.
    /// </summary>
    public class ClassDescriptionTest
    {
        private Mock<IDescription> _moqDescription = null!;

        /// <summary>
        /// Gets the read-only collection of one <see cref="PropertyDescriptionPositive"/> instances.
        /// </summary>
        /// <value> &lt;see cref=&quot;ReadOnlyCollection{IPropertyDescription}&quot;/&gt; </value>
        private static ReadOnlyCollection<IPropertyDescription> PropertyDescriptionPositive
        {
            get
            {
                return
                    new IPropertyDescription[] {new PropertyDescription("Name", "Description", "string")}.ToList().
                        AsReadOnly();
            }
        }    
        
        /// <summary>
        /// Gets the read-only collection of one <see cref="PropertyDescriptionPositive"/> instances.
        /// </summary>
        /// <value> &lt;see cref=&quot;ReadOnlyCollection{IPropertyDescription}&quot;/&gt; </value>
        private static ReadOnlyCollection<IPropertyDescription> PropertyDescriptionNegative
        {
            get
            {
                return
                    new IPropertyDescription[] {new PropertyDescription(null!, "Description", "string")}.ToList().
                        AsReadOnly();
            }
        }

        /// <summary>
        /// A test for conforming that table- / collection name is the same.
        /// </summary>
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

        /// <summary>
        /// A test for conforming that 
        /// </summary>
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
            _moqDescription.SetupGet(d => d.Properties).Returns(PropertyDescriptionPositive);

            // Act

            // Assert
            Assert.Single(_moqDescription.Object.Properties);
        }
        
        [Fact]
        public void ShouldContainPropertyDescriptionDetailsMatch()
        {
            // Arrange
            _moqDescription = new Mock<IDescription>();
            _moqDescription.SetupGet(d => d.Properties).Returns(PropertyDescriptionPositive);

            // Act

            // Assert
            var propertyDescription = Assert.Single(_moqDescription.Object.Properties);

            Assert.Same("Name", propertyDescription.Name);
            Assert.Same("Description", propertyDescription.Description);
            Assert.Same("string", propertyDescription.DotNetDataType);
        }
        
        
        [Fact]
        public void ShouldThrowExceptionWhenPropertyDescriptionCreated()
        {
            // Arrange
            _moqDescription = new Mock<IDescription>();

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(
                () => _moqDescription.SetupGet(d => d.Properties).Returns(PropertyDescriptionNegative));
        }


    }
}
