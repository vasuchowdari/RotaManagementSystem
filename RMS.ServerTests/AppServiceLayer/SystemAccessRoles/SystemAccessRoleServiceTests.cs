using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Moq;
using NUnit.Framework;
using RMS.AppServiceLayer;
using RMS.AppServiceLayer.AuditLogs.Interfaces;
using RMS.AppServiceLayer.SystemAccessRoles.Dto;
using RMS.AppServiceLayer.SystemAccessRoles.Interfaces;
using RMS.AppServiceLayer.SystemAccessRoles.Services;
using RMS.ServerTests.Moqs;

namespace RMS.ServerTests.AppServiceLayer.SystemAccessRoles
{
    [TestFixture]
    public class SystemAccessRoleServiceTests : IDisposable
    {
        private ISystemAccessRoleAppService _systemAccessRoleAppService;
        private readonly Mock<IAuditLogAppService> _mAuditLogAppService = new Mock<IAuditLogAppService>();

        [OneTimeSetUp]
        public void Init()
        {
            _systemAccessRoleAppService = new SystemAccessRoleAppService(_mAuditLogAppService.Object);

            AutoMapperAslConfig.RegisterMappings();
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            
        }

        [Test]
        public void Get_By_Id_Should_Return_Null_Reference_Exception_When_No_Id_Is_Found()
        {
            // Arrange
            var dto = new SystemAccessRoleDto
            {
                Id = -1
            };

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _systemAccessRoleAppService.GetById(dto.Id));
        }

        [Test]
        public void TestFindById()
        {
            // Arrange
            const long expectedId = 1;
            var expected = new SystemAccessRole
            {
                Id = 1,
                IsActive = true,
                Name = "Mock_SAR_1"
            };

            var mockData = new List<SystemAccessRole>
            {
                expected,
                new SystemAccessRole {Id = 2, IsActive = true, Name = "Mock_SAR_2"},
                new SystemAccessRole {Id = 3, IsActive = true, Name = "Mock_SAR_3"},
                new SystemAccessRole {Id = 4, IsActive = true, Name = "Mock_SAR_4"},
                new SystemAccessRole {Id = 5, IsActive = true, Name = "Mock_SAR_5"}
            }.AsQueryable();

            var mockDbSet = new Mock<IDbSet<SystemAccessRole>>();
            mockDbSet.Setup(m => m.Provider).Returns(mockData.Provider);
            mockDbSet.Setup(m => m.Expression).Returns(mockData.Expression);
            mockDbSet.Setup(m => m.ElementType).Returns(mockData.ElementType);
            mockDbSet.Setup(m => m.GetEnumerator()).Returns(mockData.GetEnumerator());


            var customDbContextMock = new Mock<DbContextMock>();
            customDbContextMock.Setup(x => x.SystemAccessRoles).Returns(mockDbSet.Object);

            var classUnderTest = new Processor(customDbContextMock.Object);

            // Act
            var result = classUnderTest.FindSystemAccessRoleById(expectedId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected.Id, result.Id);
            Assert.AreEqual(expected.Name, result.Name);
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
