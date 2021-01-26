using System;
using Xunit;
using PhoneBook.Api;
using PhoneBook.Entities;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhoneBook.Api.Tests
{
    public class PhoneBookViewModelTests
    {
        /// <summary>
        /// Test to ensure that a phone book cannot be created without a name
        /// </summary>
        [Fact]
        public void TestEmptyPhoneBookViewModelNameError()
        {
            var phoneBookVm = new Api.Models.PhoneBookViewModel();
            phoneBookVm.Name = "";

            var results = new List<ValidationResult>();
            var validationContext = new ValidationContext(phoneBookVm, null, null);

            Validator.TryValidateObject(phoneBookVm, validationContext, results, true);

            // TODO: Implement this interface
            if (phoneBookVm is IValidatableObject) (phoneBookVm as IValidatableObject).Validate(validationContext);

            Assert.Single(results);
            Assert.Equal("The Book Name field is required.", results[0].ErrorMessage);
        }

        /// <summary>
        /// Tests to ensure that a phone book is valid so long as it has a name with
        /// a length not longer than the maximum, and not less than the minimum number of characters.
        /// The long string below contains the maximum allowed number of characters, 128.
        /// </summary>
        /// <param name="phoneBookName"></param>
        [Theory]
        [InlineData("a")]
        [InlineData("A phone book containing the names of Mark's best chommies")]
        [InlineData("Tkd38iDkExXr8XhdcCYAW6Qv4Vi2Nsttf2e9d7aPxRIQOxDdah1zwNI1F3865rdm1G2DhgU248ouhB80s5zx1I9W0h06vHgZ270m28L063L17W9az83vopi5vb4z1ajJ")]
        public void TestValidNameLengthIsValid(string phoneBookName)
        {
            var phoneBookVm = new Api.Models.PhoneBookViewModel();
            phoneBookVm.Name = phoneBookName;

            var results = new List<ValidationResult>();
            var validationContext = new ValidationContext(phoneBookVm, null, null);

            Validator.TryValidateObject(phoneBookVm, validationContext, results, true);

            // TODO: Implement this interface
            if (phoneBookVm is IValidatableObject) (phoneBookVm as IValidatableObject).Validate(validationContext);

            Assert.Empty(results);
        }

        /// <summary>
        /// Test to ensure that a phone book with a name who's length is above the maximum number
        /// of characters fails validation. The attempted name below contains 129 characters.
        /// One more than the allowed limit of 128.
        /// </summary>
        /// <param name="phoneBookName"></param>
        [Theory]
        [InlineData("uSZ5SI2JYTi7r7Cu7Dci1LkNoYb5VagyIGxtiB9ClbOues7FxJXc6dS55hFEjYtT1W8D7E4lDY47cGObE0e3xzqUu40F0Rsh9SmIfpNHM92etOFfqRrAnez1djcAknefN")]
        public void TestMaximiumNameLengthExceededNotValid(string phoneBookName)
        {
            var phoneBookVm = new Api.Models.PhoneBookViewModel();
            phoneBookVm.Name = phoneBookName;

            var results = new List<ValidationResult>();
            var validationContext = new ValidationContext(phoneBookVm, null, null);

            Validator.TryValidateObject(phoneBookVm, validationContext, results, true);

            // TODO: Implement this interface
            if (phoneBookVm is IValidatableObject) (phoneBookVm as IValidatableObject).Validate(validationContext);

            Assert.Single(results);
            Assert.Equal("The field Book Name must be a string with a minimum length of 1 and a maximum length of 128.", results[0].ErrorMessage);
        }

    }
}
