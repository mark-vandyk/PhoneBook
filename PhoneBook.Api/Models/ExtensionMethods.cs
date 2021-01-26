using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Entities;

namespace PhoneBook.Api.Models
{
    public static class ModelExtensions
    {
        /// <summary>
        /// Returns a View model from an Entity model for a collection of Entries
        /// </summary>
        /// <param name="DbEntityCollection">The entity models to generate view models for</param>
        /// <returns>View models based on the entity models</returns>
        public static List<EntryViewModel> ToViewModelCollection(this IEnumerable<Entities.Models.Entry> DbEntityCollection)
        {
            List<EntryViewModel> OutputCollection = new List<EntryViewModel>();

            foreach (Entities.Models.Entry dbe in DbEntityCollection)
            {
                OutputCollection.Add(new EntryViewModel
                {
                    Id = dbe.Id,
                    FirstName = dbe.FirstName,
                    LastName = dbe.LastName,
                    PhoneNumber = dbe.PhoneNumber
                });
            }

            return OutputCollection;
        }


        /// <summary>
        /// Returns a View models from a collection of Phone Book models
        /// </summary>
        /// <param name="DbEntityCollection">The entity models to generate view models for</param>
        /// <returns>View models based on the entity models</returns>
        public static List<PhoneBookViewModel> ToViewModelCollection(this IEnumerable<Entities.Models.PhoneBook> DbEntityCollection)
        {
            List<PhoneBookViewModel> OutputCollection = new List<PhoneBookViewModel>();

            foreach (Entities.Models.PhoneBook dbe in DbEntityCollection)
            {
                OutputCollection.Add(new PhoneBookViewModel
                {
                    Id = dbe.Id,
                    Name = dbe.Name,
                    Entries = dbe.Entries.Count()
                });
            }

            return OutputCollection;
        }
    }

}
