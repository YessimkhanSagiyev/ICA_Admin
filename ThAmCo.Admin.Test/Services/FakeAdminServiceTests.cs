using ThAmCo.Admin.Models;
using ThAmCo.Admin.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace ThAmCo.Admin.Test.Services
{
    [TestClass]
    public class FakeAdminServiceTests
    {
        private FakeAdminService _fakeService;

        [TestInitialize]
        public void Setup()
        {
            // Initialize the FakeAdminService before each test
            _fakeService = new FakeAdminService();
        }

        [TestMethod]
        public async Task GetAllUsers_ShouldReturnAllUsers()
        {
            // Act
            var users = await _fakeService.GetAllUsersAsync();

            // Assert
            Assert.AreEqual(2, users.Count());
            Assert.IsTrue(users.Any(u => u.Name == "John Doe"));
            Assert.IsTrue(users.Any(u => u.Name == "Jane Smith"));
        }

        [TestMethod]
        public async Task ChangeOrderStatus_ShouldUpdateOrderStatus()
        {
            // Arrange
            var orderId = 1;
            var newStatus = "Dispatched";

            // Act
            var result = await _fakeService.ChangeOrderStatusAsync(orderId, newStatus);

            // Assert
            Assert.IsTrue(result);

            // Verify the updated status
            var order = _fakeService.GetAllOrders().FirstOrDefault(o => o.Id == orderId);
            Assert.IsNotNull(order);
            Assert.AreEqual(newStatus, order.Status);
        }

        [TestMethod]
        public async Task DeleteUser_ShouldRemoveUserAndAssociatedOrders()
        {
            // Arrange
            var userId = 1;

            // Act
            var result = await _fakeService.DeleteUserAccountAsync(userId);

            // Assert
            Assert.IsTrue(result);
            var users = await _fakeService.GetAllUsersAsync();
            Assert.AreEqual(1, users.Count()); // Only one user should remain
            Assert.IsFalse(users.Any(u => u.Id == userId)); // Deleted user should not exist
        }

        [TestMethod]
        public async Task DeleteUser_ShouldReturnFalseIfUserDoesNotExist()
        {
            // Arrange
            var nonExistentUserId = 99;

            // Act
            var result = await _fakeService.DeleteUserAccountAsync(nonExistentUserId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task MarkOrderAsDispatched_ShouldUpdateDispatchDate()
        {
            // Arrange
            var orderId = 1;
            var dispatchDate = System.DateTime.UtcNow;

            // Act
            var order = await _fakeService.MarkOrderAsDispatchedAsync(orderId, dispatchDate);

            // Assert
            Assert.IsNotNull(order);
            Assert.AreEqual(dispatchDate, order.DispatchDate);
            Assert.AreEqual("Dispatched", order.Status);
        }
    }
}
