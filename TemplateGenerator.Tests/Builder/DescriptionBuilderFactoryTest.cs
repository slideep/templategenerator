using System.Xml.Linq;
using Moq;
using TemplateGenerator.Builder;
using Xunit;

namespace TemplateGenerator.Tests.Builder
{
    public class DescriptionBuilderFactoryTest
    {
        [Fact]
        public void ShouldRegisterTypeAndGetInstance()
        {
            // act
            // arrange
            // assert

            var mockBuilderFactory = new Mock<DescriptionBuilderFactory<XElement>>();
            //mockBuilderFactory

            var builderFactory = new DescriptionBuilderFactory<XElement>();
            builderFactory.Define(() => new XmlDescriptionBuilder());

            var builder = builderFactory.Build<XmlDescriptionBuilder>();

            Assert.NotNull(builder);
        }
    }
}