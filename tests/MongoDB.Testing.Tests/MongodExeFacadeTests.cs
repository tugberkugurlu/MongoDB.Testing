using System.Diagnostics;
using MongoDB.Testing.Mongo;
using Moq;
using Xunit;

namespace MongoDB.Testing.Tests
{
    public class MongodExeFacadeTests
    {
        [Fact]
        public void Start_Should_Return_The_Return_Value_Of_ProcessStarter_Start()
        {
            // ARRANGE
            const string location = @"c:\mongo\bin\mongod.exe";
            var options = new MongodExeOptions(27017, @"c:\temp\radom-path");
            var exeLocatorMock = new Mock<IMongoExeLocator>();
            exeLocatorMock.Setup(l => l.Locate()).Returns(() => location);
            var processMock = new Mock<IProcess>();
            processMock.SetupAllProperties();
            var processStarterMock = new Mock<IProcessStarter>();
            processStarterMock.Setup(starter => starter.Start(It.IsAny<ProcessStartInfo>()))
                .Returns<ProcessStartInfo>(info => processMock.Object);

            var exeFacade = new MongodExeFacade(exeLocatorMock.Object, processStarterMock.Object);

            // ACT
            IProcess process = exeFacade.Start(options);

            // ASSERT
            processStarterMock.Verify(starter => starter.Start(It.IsAny<ProcessStartInfo>()), Times.Once());
            Assert.True(ReferenceEquals(process, processMock.Object));
        }

        [Fact]
        public void Start_Should_Call_ProcessStarter_With_Correct_Parameters()
        {
            // ARRANGE
            const string location = @"c:\mongo\bin\mongod.exe";
            var options = new MongodExeOptions(27017, @"c:\temp\radom-path");
            var exeLocatorMock = new Mock<IMongoExeLocator>();
            exeLocatorMock.Setup(l => l.Locate()).Returns(() => location);
            var processStarterMock = new Mock<IProcessStarter>();
            processStarterMock.Setup(starter => starter.Start(It.IsAny<ProcessStartInfo>()))
                .Returns<ProcessStartInfo>(info =>
                {
                    var processMock = new Mock<IProcess>();
                    processMock.Setup(p => p.StartInfo).Returns(info);

                    return processMock.Object;
                });

            var exeFacade = new MongodExeFacade(exeLocatorMock.Object, processStarterMock.Object);

            // ACT
            IProcess process = exeFacade.Start(options);

            // ASSERT
            processStarterMock.Verify(starter => starter.Start(It.IsAny<ProcessStartInfo>()), Times.Once());
            Assert.Equal(options.ToString(), process.StartInfo.Arguments);
            Assert.Equal(location, process.StartInfo.FileName);
        }
    }
}
