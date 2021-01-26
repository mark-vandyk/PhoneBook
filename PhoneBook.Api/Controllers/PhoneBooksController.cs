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
    public class PhoneBooksController : Controller
    {
        private readonly ILogger<PhoneBooksController> _logger;
        private readonly Entities.Data.PhoneBookContext _context;

        /// <summary>
        /// Cconstructor for the PhoneBooks controller
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="context"></param>
        public PhoneBooksController(ILogger<PhoneBooksController> logger, Entities.Data.PhoneBookContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Indexs and lists the available phone books.
        /// </summary>
        /// <param name="confirmDelete">Id of the Phone Book to potentially delete</param>
        /// <param name="searchString">Any search string that was applied to the Phone Book collection</param>
        /// <returns></returns>
        public IActionResult Index(int? confirmDelete, string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["IdToDelete"] = confirmDelete;

            var PhoneBooks = _context.PhoneBooks.Include("Entries");
            if (!String.IsNullOrEmpty(searchString))
            {
                PhoneBooks = PhoneBooks.Where(s => s.Name.Contains(searchString));
            }
            return View(PhoneBooks.ToViewModelCollection());
        }

        /// <summary>
        /// Returns the view responsible for capturing a new Phone Book.
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Performs the actions necessary to take user defined values and write them to the database
        /// </summary>
        /// <param name="PhoneBook">View model of the Phone book to add to the database</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PhoneBookViewModel PhoneBook)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation(string.Format("Adding the following Phone Book to the Database: '{0}'", PhoneBook.Name));
                    _context.PhoneBooks.Add(new Entities.Models.PhoneBook()
                    {
                        Name = PhoneBook.Name
                    });
                    _context.SaveChanges();
                    _logger.LogInformation("Sucessfully added the new Phone Book to the database.");
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    _logger.LogError(string.Format("An error has occured while writing the new Phone Book to the database: {0}", e.Message));
                    ModelState.AddModelError("Name", "An unknown error has occured. Please re-capture details and try again. If you have added any spaces after a name, remove them.");
                }

            }

            return View(PhoneBook);
        }

        /// <summary>
        /// Returns the view responsible for editing details of a Phone Book
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Edit(int id)
        {
            PhoneBookViewModel PhoneBook = _context.PhoneBooks.Include("Entries").Where(pb => pb.Id == id).ToViewModelCollection().FirstOrDefault();
            if (PhoneBook == null) return RedirectToAction("Index");
            return View(PhoneBook);
        }

        /// <summary>
        /// Performs the actions necessary to take user defined values and update the according
        /// Phone Book in the database with those values
        /// </summary>
        /// <param name="PhoneBook">The view model containing edits to the Phone Book details</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PhoneBookViewModel PhoneBook)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation(string.Format("Updating the Phone Book with id '{1}' in the Database: '{0}'", PhoneBook.Name, PhoneBook.Id));
                    var PhoneBookEntity = _context.PhoneBooks.Where(pb => pb.Id == PhoneBook.Id).FirstOrDefault();

                    if (PhoneBookEntity == null) return View(PhoneBook);

                    PhoneBookEntity.Name = PhoneBook.Name;
                    _context.SaveChanges();
                    _logger.LogInformation("Sucessfully updated the Phone Book in the database.");
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    _logger.LogError(string.Format("An error has occured while writing the updated Phone Book to the database: {0}", e.Message));
                    ModelState.AddModelError("Name", "An unknown error has occured. Please re-capture details and try again. If you have added any spaces after a name, remove them.");
                }
            }

            return View(PhoneBook);
        }

        /// <summary>
        /// Returns the view responsible for allowing the user to confirm the delete of a Phone Book
        /// </summary>
        /// <param name="id">The Id of the Phone Book to potentially delete from the database</param>
        /// <param name="searchString">Any search string that was applied to the Phone Book collection</param>
        /// <returns></returns>
        public IActionResult Delete(int id, string searchString)
        {
            return RedirectToAction("Index", "PhoneBooks", new { confirmDelete = id, searchString });
        }

        /// <summary>
        /// Performs the actions necessary to delete a Phone Book and all Entries therein from the database
        /// </summary>
        /// <param name="confirmDelete">The Id of the Phone Book to remove from the database</param>
        /// <returns></returns>
        public IActionResult ConfirmDelete(int confirmDelete)
        {
            try
            {
                _logger.LogInformation(string.Format("Attempting to delete Phone Book with id '{0}' from the database.", confirmDelete));
                var PhoneBookEntity = _context.PhoneBooks.Where(pb => pb.Id == confirmDelete).FirstOrDefault();

                if (PhoneBookEntity == null) return RedirectToAction("Index");

                _context.Remove(PhoneBookEntity);
                _context.SaveChanges();
                _logger.LogInformation("Sucessfully removed the Phone Book from the database.");
            }
            catch (Exception e)
            {
                _logger.LogError(string.Format("An error has occured while removing the Phone Book from the database: {0}", e.Message));
            }


            return RedirectToAction("Index");
        }

        #region Home Pages

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Help()
        {
            return View();
        }

        public IActionResult Faq()
        {
            return View();
        }

        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
