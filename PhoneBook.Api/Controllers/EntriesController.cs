using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhoneBook.Api.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Entities;

namespace PhoneBook.Api.Controllers
{
    public class EntriesController : Controller
    {
        private readonly ILogger<EntriesController> _logger;
        private readonly Entities.Data.PhoneBookContext _context;

        /// <summary>
        /// Constructor for the Entries Controller
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="context"></param>
        public EntriesController(ILogger<EntriesController> logger, Entities.Data.PhoneBookContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Indexes and lists all the Entries for the Phone Book Id specified.
        /// </summary>
        /// <param name="id">The Id of the Phone Book to list Entries for.</param>
        /// <param name="confirmDelete">Id of the Phone Book to potentially delete</param>
        /// <param name="searchString">Any search string that was applied to the Phone Book collection</param>
        /// <returns></returns>
        public IActionResult Index(int id, int? confirmDelete, string searchString)
        {
            if (id <= 0) return Redirect("/");

            ViewData["PhoneBookName"] = _context.PhoneBooks
                .Where(pb => pb.Id == id)
                .Select(pb => pb.Name)
                .FirstOrDefault();

            if (ViewData["PhoneBookName"] == null) return Redirect("/");

            ViewData["PhoneBookId"] = id;

            ViewData["CurrentFilter"] = searchString;
            ViewData["IdToDelete"] = confirmDelete;

            var Entries = _context.Entries.Where(e => e.PhoneBookId == id);
            if (!String.IsNullOrEmpty(searchString))
            {
                Entries = Entries.Where(s => s.FirstName.Contains(searchString) || s.LastName.Contains(searchString));
            }
            return View(Entries.ToViewModelCollection());
        }

        /// <summary>
        /// Returns the view responsible for capturing a new Phone Book Entry
        /// </summary>
        /// <param name="id">the Id of the Phone Book to add the Entry to.</param>
        /// <returns></returns>
        public IActionResult Create(int id)
        {
            if (id <= 0) return Redirect("/");
            ViewData["PhoneBookId"] = id;
            return View();
        }

        /// <summary>
        /// Performs the actions necessary to take user defined values and write them to the database as a new Phone Book Entry.
        /// </summary>
        /// <param name="id">The Id of the Phone Book to add the Entry to</param>
        /// <param name="entry">A view model of the Entry to add to the database</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int id, EntryViewModel entry)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation(string.Format("Adding the following Entry to the Database: '{0}', '{1}', '{2}'", entry.FirstName, entry.LastName, entry.PhoneNumber));
                    _context.Entries.Add(new Entities.Models.Entry()
                    {
                        FirstName = entry.FirstName,
                        LastName = entry.LastName,
                        PhoneNumber = entry.PhoneNumber,
                        PhoneBookId = id
                    });
                    _context.SaveChanges();
                    _logger.LogInformation("Sucessfully added the new Entry to the database.");

                    return RedirectToAction("Index", new { id });
                }
                catch (Exception e)
                {
                    _logger.LogError(string.Format("An error has occured while writing the new Entry to the database: {0}", e.Message));
                    ModelState.AddModelError("PhoneNumber", "An unknown error has occured. Please re-capture details and try again. If you have added any spaces after a name, remove them.");
                }

            }

            if (id <= 0) return Redirect("/");
            ViewData["PhoneBookId"] = id;

            return View(entry);
        }

        /// <summary>
        /// Returns the view responsible for editing details of a Phone Book Entry
        /// </summary>
        /// <param name="id">The Id of the Entry to Edit.</param>
        /// <param name="PhoneBookId">The Id of the Phone Book that this Entry falls under.</param>
        /// <returns></returns>
        public IActionResult Edit(int id, int PhoneBookId)
        {
            EntryViewModel Entry = _context.Entries.Where(e => e.Id == id).ToViewModelCollection().FirstOrDefault();
            if (Entry == null) return RedirectToAction("Index");
            ViewData["PhoneBookId"] = PhoneBookId;
            return View(Entry);
        }

        /// <summary>
        /// Performs the actions necessary to take user defined values and update the according
        /// Entry in the database with those values
        /// </summary>
        /// <param name="Entry">View model of the entry with edits made by the user</param>
        /// <param name="PhoneBookId">The Id of the Phone Book that this Entry falls under</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EntryViewModel Entry, int PhoneBookId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation(string.Format("Updating the Entry with id '{3}' in the Database: '{0}', '{1}', '{2}'", Entry.FirstName, Entry.LastName, Entry.PhoneNumber, Entry.Id));

                    var EntryEntity = _context.Entries.Where(pb => pb.Id == Entry.Id).FirstOrDefault();

                    if (EntryEntity == null) return View(EntryEntity);

                    EntryEntity.FirstName = Entry.FirstName;
                    EntryEntity.LastName = Entry.LastName;
                    EntryEntity.PhoneNumber = Entry.PhoneNumber;
                    _context.SaveChanges();
                    _logger.LogInformation("Sucessfully updated the Entry in the database.");
                    return RedirectToAction("Index", new { id = PhoneBookId });
                }
                catch (Exception e)
                {
                    _logger.LogError(string.Format("An error has occured while writing the updated Entry to the database: {0}", e.Message));
                    ModelState.AddModelError("PhoneNumber", "An unknown error has occured. Please re-capture details and try again. If you have added any spaces after a name, remove them.");
                }

            }

            ViewData["PhoneBookId"] = PhoneBookId;
            return View(Entry);
        }

        /// <summary>
        /// Returns the view responsible for allowing the user to confirm the delete of an Entry
        /// </summary>
        /// <param name="id">The Id of the Entry to potentially delete.</param>
        /// <param name="PhoneBookId">The Id of the Phone Book that this entry falls under</param>
        /// <param name="searchString">Any search criteria currently being applied to the list view</param>
        /// <returns></returns>
        public IActionResult Delete(int id, int PhoneBookId, string searchString)
        {
            return RedirectToAction("Index", "Entries", new { id = PhoneBookId, confirmDelete = id, searchString });
        }

        /// <summary>
        /// Performs the actions necessary to delete a Entry from the database
        /// </summary>
        /// <param name="id">The Id of the Phone Book that this Entry falls under</param>
        /// <param name="confirmDelete">The Id of the Entry to Delete</param>
        /// <returns></returns>
        public IActionResult ConfirmDelete(int id, int confirmDelete)
        {
            try
            {
                _logger.LogInformation(string.Format("Attempting to delete Entry with id '{0}' from the database.", confirmDelete));
                var EntryEntity = _context.Entries.Where(e => e.Id == confirmDelete).FirstOrDefault();

                if (EntryEntity == null) return RedirectToAction("Index");
                _context.Remove(EntryEntity);
                _context.SaveChanges();
                _logger.LogInformation("Sucessfully removed the Entry from the database.");
            }
            catch(Exception e)
            {
                _logger.LogError(string.Format("An error has occured while removing the Entry from the database: {0}", e.Message));
            }


            return RedirectToAction("Index", new { id });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
