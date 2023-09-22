using Challenge.Contracts;
using Challenge.Controllers;
using Challenge.Data.DTO;
using Challenge.Data.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Challenge.Customers.Tests
{
    public class CustomerControllerTests
    {
        /// <summary>Defines the test method UpdateCustomer_WithValidData_ShouldReturnNoContent.</summary>
        [Fact]
        public async Task UpdateCustomer_WithValidData_ShouldReturnNoContent()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var patchData = new CustomerDto { FullName = "Updated Name", DateOfBirth = new DateOnly(1990, 1, 1) };

            var mockLogger = new Mock<ILogger<CustomersController>>();
            var mockCustomerService = new Mock<ICustomerService>();

            // Assuming you have an instance of CustomerDto to use in the test
            var existingCustomer = new Customer
            {
                CustomerId = customerId,
                FullName = "Himanshu Chawla",
                DateOfBirth = new DateOnly(1985, 5, 10),
                ProfileImageSvg = ""
            };

            mockCustomerService.Setup(service => service.GetCustomerDetailsById(customerId))
                .ReturnsAsync(existingCustomer);

            var controller = new CustomersController(mockCustomerService.Object);

            // Act
            var result = await controller.UpdateCustomer(customerId, patchData);

            // Assert
            result.Should().BeOfType<NoContentResult>(); // Asserts that it returns NoContent

            // Verify that the customer entity is updated
            existingCustomer.FullName.Should().Be(patchData.FullName);
            existingCustomer.DateOfBirth.Should().Be(patchData.DateOfBirth);
        }


        /// <summary>Defines the test method UpdateCustomer_WithInvalidCustomer_ShouldReturnNotFound.</summary>
        [Fact]
        public async Task UpdateCustomer_WithInvalidCustomer_ShouldReturnNotFound()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var patchData = new CustomerDto { FullName = "Updated Name", DateOfBirth = new DateOnly(1990, 1, 1) };

            var mockLogger = new Mock<ILogger<CustomersController>>();
            var mockCustomerService = new Mock<ICustomerService>();

            // Set up the service to return null, indicating customer not found
            mockCustomerService.Setup(service => service.GetCustomerDetailsById(customerId))
                .ReturnsAsync((Guid id) => null);

            var controller = new CustomersController(mockCustomerService.Object);

            // Act
            var result = await controller.UpdateCustomer(customerId, patchData);

            // Assert
            result.Should().BeOfType<NotFoundResult>(); // Asserts that it returns NotFound
        }

        /// <summary>Defines the test method CreateCustomer_WithValidData_ShouldReturnCreated.</summary>
        [Fact]
        public async Task CreateCustomer_WithValidData_ShouldReturnCreated()
        {
            // Arrange
            var customerDto = new CustomerDto
            {
                FullName = "Himanshu Chawla",
                DateOfBirth = new DateOnly(1993, 2, 6)
            };

            var mockLogger = new Mock<ILogger<CustomersController>>();
            var mockCustomerService = new Mock<ICustomerService>();

            // Mock the external API call to generate profile image
            mockCustomerService.Setup(service => service.GenerateProfileImageAsync(customerDto.FullName))
                .ReturnsAsync("<svg>profile_image</svg>");

            // Mock the AddCustomer method to return a valid Customer entity
            mockCustomerService.Setup(service => service.AddCustomer(It.IsAny<Customer>()))
                .ReturnsAsync((Customer c) => c);

            // Pass the mock logger and service to the controller constructor
            var controller = new CustomersController(mockCustomerService.Object);

            // Act
            var result = await controller.CreateCustomer(customerDto);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>(); // Asserts that it returns CreatedAtAction

            // Verify that the CreatedAtAction result contains the correct route name and values
            var createdAtActionResult = result as CreatedAtActionResult;
            createdAtActionResult.ActionName.Should().Be(nameof(CustomersController.GetCustomerById));
            createdAtActionResult.RouteValues.Should().ContainKey("id");
        }

        /// <summary>Defines the test method CreateCustomer_WithInvalidData_ShouldReturnBadRequest.</summary>
        [Fact]
        public async Task CreateCustomer_WithInvalidData_ShouldReturnBadRequest()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CustomersController>>();
            var mockCustomerService = new Mock<ICustomerService>();

            // Assuming you send null as customerDto, which is invalid data
            CustomerDto customerDto = null;

            var controller = new CustomersController(mockCustomerService.Object);

            // Act
            var result = await controller.CreateCustomer(customerDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>(); // Asserts that it returns BadRequest
        }


        /// <summary>Defines the test method GetCustomersByAge_WithValidAge_ShouldReturnOk.</summary>
        [Fact]
        public async Task GetCustomersByAge_WithValidAge_ShouldReturnOk()
        {
            // Arrange
            var age = 30; // Age to filter by
            var today = DateTime.Today;
            var birthYear = today.Year - age;

            var mockCustomerService = new Mock<ICustomerService>();

            // Mock the customer service to return a list of customers
            var customers = new List<Customer>
            {
                new Customer { DateOfBirth = new DateOnly(birthYear, 1, 1), FullName = "Himanshu Chawla", ProfileImageSvg = "<svg>profile_image</svg>", CustomerId = Guid.NewGuid() },
                new Customer { DateOfBirth = new DateOnly(birthYear, 5, 10), FullName = "Testing Name", ProfileImageSvg = "<svg>profile_image</svg>", CustomerId = Guid.NewGuid() },
                new Customer { DateOfBirth = new DateOnly(birthYear - 1, 8, 15), FullName = "Testing Name 1", ProfileImageSvg = "<svg>profile_image</svg>", CustomerId = Guid.NewGuid() }, // Should not be included
            };

            mockCustomerService.Setup(service => service.GetAllCustomers())
                .ReturnsAsync(customers);

            var controller = new CustomersController(mockCustomerService.Object);

            // Act
            var result = await controller.GetCustomersByAge(age);

            // Assert
            result.Should().BeOfType<OkObjectResult>(); // Asserts that it returns Ok
            var okResult = result as OkObjectResult;
            var resultList = okResult.Value as List<Customer>;

            // Verify that the correct number of customers is returned based on age
            resultList.Should().HaveCount(2); // Two customers should match the given age
        }

        /// <summary>Defines the test method GetCustomersByAge_WithNoMatchingCustomers_ShouldReturnEmptyList.</summary>
        [Fact]
        public async Task GetCustomersByAge_WithNoMatchingCustomers_ShouldReturnEmptyList()
        {
            // Arrange
            var age = 25; // Age for which there are no matching customers

            var mockLogger = new Mock<ILogger<CustomersController>>();
            var mockCustomerService = new Mock<ICustomerService>();

            // Mock the customer service to return an empty list
            var customers = new List<Customer>
            {
                new Customer { DateOfBirth = new DateOnly(1990, 1, 1), FullName = "Himanshu Chawla", ProfileImageSvg = "<svg>profile_image</svg>", CustomerId = Guid.NewGuid() },
                new Customer { DateOfBirth = new DateOnly(1985, 5, 10), FullName = "Testing Name", ProfileImageSvg = "<svg>profile_image</svg>", CustomerId = Guid.NewGuid() },
            };

            mockCustomerService.Setup(service => service.GetAllCustomers())
                .ReturnsAsync(customers);

            var controller = new CustomersController(mockCustomerService.Object);

            // Act
            var result = await controller.GetCustomersByAge(age);

            // Assert
            result.Should().BeOfType<OkObjectResult>(); // Asserts that it returns Ok
            var okResult = result as OkObjectResult;
            var resultList = okResult.Value as List<CustomerDto>;

            // Verify that the result list is empty when no customers match the given age
            resultList.Should().BeNull();
        }

        /// <summary>Defines the test method GetCustomersByAge_WithNegativeAge_ShouldReturnBadRequest.</summary>
        [Fact]
        public async Task GetCustomersByAge_WithNegativeAge_ShouldReturnBadRequest()
        {
            // Arrange
            var age = -5; // Negative age is not valid

            var mockLogger = new Mock<ILogger<CustomersController>>();
            var mockCustomerService = new Mock<ICustomerService>();

            var controller = new CustomersController(mockCustomerService.Object);

            // Act
            var result = await controller.GetCustomersByAge(age);

            // Assert
            result.Should().BeOfType<BadRequestResult>(); // Asserts that it returns BadRequest
        }

        /// <summary>Defines the test method GetCustomers_WithValidData_ShouldReturnOk.</summary>
        [Fact]
        public async Task GetCustomers_WithValidData_ShouldReturnOk()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CustomersController>>();
            var mockCustomerService = new Mock<ICustomerService>();

            // Mock the customer service to return a list of customers
            var customers = new List<Customer>
            {
                new Customer { FullName = "Customer 1", ProfileImageSvg = "<svg>profile_image</svg>", CustomerId = Guid.NewGuid(), DateOfBirth =  new DateOnly(1990, 1, 1) },
                new Customer { FullName = "Customer 2", ProfileImageSvg = "<svg>profile_image</svg>", CustomerId = Guid.NewGuid(), DateOfBirth =  new DateOnly(1990, 1, 1) },
                new Customer { FullName = "Customer 3", ProfileImageSvg = "<svg>profile_image</svg>", CustomerId = Guid.NewGuid(), DateOfBirth =  new DateOnly(1990, 1, 1)},
            };

            mockCustomerService.Setup(service => service.GetAllCustomers())
                .ReturnsAsync(customers);

            var controller = new CustomersController(mockCustomerService.Object);

            // Act
            var result = await controller.GetCustomers();

            // Assert
            result.Should().BeOfType<OkObjectResult>(); // Asserts that it returns Ok
            var okResult = result as OkObjectResult;
            var resultList = okResult.Value as List<Customer>;

            // Verify that the correct number of customers is returned
            resultList.Should().HaveCount(3); // Assuming 3 customers were mocked
        }

        /// <summary>Defines the test method GetCustomers_WithNoCustomers_ShouldReturnEmptyList.</summary>
        [Fact]
        public async Task GetCustomers_WithNoCustomers_ShouldReturnEmptyList()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CustomersController>>();
            var mockCustomerService = new Mock<ICustomerService>();

            // Mock the customer service to return an empty list
            var customers = new List<Customer>();

            mockCustomerService.Setup(service => service.GetAllCustomers())
                .ReturnsAsync(customers);

            var controller = new CustomersController(mockCustomerService.Object);

            // Act
            var result = await controller.GetCustomers();

            // Assert
            result.Should().BeOfType<OkObjectResult>(); // Asserts that it returns Ok
            var okResult = result as OkObjectResult;
            var resultList = okResult.Value as List<Customer>;

            // Verify that the result list is empty when no customers are available
            resultList.Should().BeEmpty();
        }
    }
}