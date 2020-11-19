using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KinAndCarta.API.Controllers;
using KinAndCarta.API.Models;
using KinAndCarta.API.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace KinAndCarta.API.Tests
{
    [TestClass]
    public class ContactControllerTest
    {
        [TestMethod]
        public void GetContactByAddress()
        {
            // Arrange
            Mock<IContact> mock = new Mock<IContact>();
            mock.Setup(x=> x.GetAllContactsByAddress(null,null)).Returns(GetTestContact());
            ContactController controller = new ContactController(mock.Object);
            // Act
            var result = controller.GetContactsByAddress("Chicago", "Illinois");
            // Assert
            Assert.IsNotNull(mock.Object);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("name Test 1", result.Select(s => s.name).First());
            Assert.AreEqual("Test1@test.com", result.Select(s => s.email).First());
        }
        private List<Contact> GetTestContact()
        {
            var testContacts = new List<Contact>();
            testContacts.Add(new Contact {
                name = "name Test 1",
                company = "Kin+Cartar Test 1",
                profileImageUri = "default-profile.jpg",
                email = "Test1@test.com",
                birthdate = DateTime.Today.ToShortDateString(),
                personalPhone = "123456789",
                workPhone = "123456789",
                address = new Address
                {
                    city = "Chicago",
                    state = "Illinois",
                    street = "8925 NE Ter"
                }
            });

            return testContacts;
        }
    }
}
