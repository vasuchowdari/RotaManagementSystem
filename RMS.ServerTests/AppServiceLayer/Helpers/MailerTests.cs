using System;
using Moq;
using NUnit.Framework;
using RMS.AppServiceLayer.Helpers;
using RMS.AppServiceLayer.Helpers.Dto;
using RMS.AppServiceLayer.Helpers.Services;
using RMS.AppServiceLayer.Users.Dto;

namespace RMS.ServerTests.AppServiceLayer.Helpers
{
    [TestFixture]
    public class MailerTests : IDisposable
    {
        [OneTimeSetUp]
        public void Init()
        {
            
        }

        [OneTimeTearDown]
        public void CleanUp()
        {

        }

        [Test]
        public void Should_Send_New_User_Email_with_Account_Password()
        {
            // Arrange
            var userDto = new UserDto
            {
                Email = "test@testemail.com",
                Firstname = "Test",
                Lastname = "User",
                IsAccountLocked = false,
                IsActive = true,
                SystemAccessRoleId = 1,
                Password = CommonHelperAppService.RandomString(8)
            };

            // Act
            MailerService.SendUserRegisteredEmail(userDto);

            // Assert
            //Assert.AreEqual(expected, result);
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
