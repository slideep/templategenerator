using System.Diagnostics;
using Moq;
using TemplateGenerator.Common;
using Xunit;

namespace TemplateGenerator.Tests.Common
{
    public class ServiceUtilsTest
    {
        [Fact]
        public void ShouldFindVsProcessRunning()
        {
            // Arrange & create mocks
            var mockRepository = new Mock<Process>();

            // Act

            // Assert
        }
    }
}
