using System;
using Xunit;
using PhoneBook.Api;
using PhoneBook.Entities;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhoneBook.Api.Tests
{
    public class PhoneBookEntriesViewModelTests
    {
        /// <summary>
        /// Test to ensure that a phone book entry cannont be deemed valid
        /// if no phone number is captured, because it is a required field.
        /// </summary>
        [Fact]
        public void TestEmptyPhoneBookViewModelPhoneNumberError()
        {
            var entityVm = new Models.EntryViewModel();
            entityVm.FirstName = "Mark";
            entityVm.LastName = "van Dyk";

            var results = new List<ValidationResult>();
            var validationContext = new ValidationContext(entityVm, null, null);

            Validator.TryValidateObject(entityVm, validationContext, results, true);

            // TODO: Implement this interface
            if (entityVm is IValidatableObject) (entityVm as IValidatableObject).Validate(validationContext);

            Assert.Single(results);
            Assert.Equal("Valid Phone Number Required!", results[0].ErrorMessage);
        }

        /// <summary>
        /// Test to ensure that a phone book entry must
        /// contain a First Name of more than one character
        /// </summary>
        [Fact]
        public void TestEmptyPhoneBookViewModelFirstNameError()
        {
            var entityVm = new Models.EntryViewModel();
            entityVm.FirstName = "1";
            entityVm.LastName = "van Dyk";
            entityVm.PhoneNumber = "(083) 797 9777";
            var results = new List<ValidationResult>();
            var validationContext = new ValidationContext(entityVm, null, null);

            Validator.TryValidateObject(entityVm, validationContext, results, true);

            // TODO: Implement this interface
            if (entityVm is IValidatableObject) (entityVm as IValidatableObject).Validate(validationContext);

            Assert.Single(results);
            Assert.Equal("The field First Name must be a string with a minimum length of 2 and a maximum length of 128.", results[0].ErrorMessage);
        }

        /// <summary>
        /// Test to ensure that a phone book entry must
        /// contain a Last Name of more than one character
        /// </summary>
        [Fact]
        public void TestEmptyPhoneBookViewModelLastNameError()
        {
            var entityVm = new Models.EntryViewModel();
            entityVm.FirstName = "Mark";
            entityVm.LastName = "1";
            entityVm.PhoneNumber = "(083) 797 9777";

            var results = new List<ValidationResult>();
            var validationContext = new ValidationContext(entityVm, null, null);

            Validator.TryValidateObject(entityVm, validationContext, results, true);

            // TODO: Implement this interface
            if (entityVm is IValidatableObject) (entityVm as IValidatableObject).Validate(validationContext);

            Assert.Single(results);
            Assert.Equal("The field Last Name must be a string with a minimum length of 2 and a maximum length of 128.", results[0].ErrorMessage);
        }

        /// <summary>
        /// Tests to ensure that all of the fields (First Name, Last Name and Phone Number)
        /// are indeed required - whether passed as an empty string or NULL.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="expectedError"></param>
        [Theory]
        [InlineData("", "Tombs", "0823453456", "The First Name field is required.")]
        [InlineData("Mark", "", "083-797-9777", "The Last Name field is required.")]
        [InlineData("Mark", "van Dyk", "", "Valid Phone Number Required!")]
        [InlineData(null, "Tombs", "0823453456", "The First Name field is required.")]
        [InlineData("Mark", null, "083-797-9777", "The Last Name field is required.")]
        [InlineData("Mark", "van Dyk", null, "Valid Phone Number Required!")]
        public void TestMissingRequiredFieldsReturnsError(string firstName, string lastName, string phoneNumber, string expectedError)
        {
            var entryVm = new Api.Models.EntryViewModel();
            entryVm.FirstName = firstName;
            entryVm.LastName = lastName;
            entryVm.PhoneNumber = phoneNumber;

            var results = new List<ValidationResult>();
            var validationContext = new ValidationContext(entryVm, null, null);

            Validator.TryValidateObject(entryVm, validationContext, results, true);

            // TODO: Implement this interface
            if (entryVm is IValidatableObject) (entryVm as IValidatableObject).Validate(validationContext);

            Assert.Single(results);
            Assert.Equal(expectedError, results[0].ErrorMessage);
        }

        /// <summary>
        /// Test to ensure that the minimum required set of acceptable Phone Number formats
        /// pass validation.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="phoneNumber"></param>
        [Theory]
        [InlineData("Some", "Guy", "0837979777")]
        [InlineData("Some", "Guy", "083 797 9777")]
        [InlineData("Some", "Guy", "083-797-9777")]
        [InlineData("Some", "Guy", "(083) 797 9777")]
        public void TestValidPhoneNumberFormats(string firstName, string lastName, string phoneNumber)
        {
            var entryVm = new Api.Models.EntryViewModel();
            entryVm.FirstName = firstName;
            entryVm.LastName = lastName;
            entryVm.PhoneNumber = phoneNumber;

            var results = new List<ValidationResult>();
            var validationContext = new ValidationContext(entryVm, null, null);

            Validator.TryValidateObject(entryVm, validationContext, results, true);

            // TODO: Implement this interface
            if (entryVm is IValidatableObject) (entryVm as IValidatableObject).Validate(validationContext);

            Assert.Empty(results);
        }
    }
}
