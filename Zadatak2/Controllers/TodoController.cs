using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Zadatak1;
using Zadatak1.Models;
using Zadatak2.Data;
using Zadatak2.ViewModels;

namespace Zadatak2.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        //private List<TodoItemLabel> _currentTodoItemLabels;

        public TodoController(ITodoRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            List<TodoItem> todoItems = _repository.GetActive(new Guid(currentUser.Id));
            IndexViewModel indexViewModel = new IndexViewModel();
            foreach (TodoItem item in todoItems)
            {
                TodoViewModel model = new TodoViewModel();
                model.Id = item.Id;
                model.Labels = item.Labels;
                model.Text = item.Text;
                model.UserId = item.UserId;
                model.DateCreated = item.DateCreated;
                model.DateDue = item.DateDue;
                model.DateCompleted = item.DateCompleted;
                indexViewModel.Items.Add(model);
            }
            return View(indexViewModel);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTodoViewModel item)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
                TodoItem todoItem= new TodoItem(item.Text, new Guid(currentUser.Id));
                todoItem.DateDue = item.DateDue;

                if (item.Labels.Count > 0)
                {
                    foreach (var label in item.Labels)
                    {
                        todoItem.Labels.Add(new TodoItemLabel(label));
                    }
                }
                _repository.Add(todoItem);
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> Completed()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            List<TodoItem> todoItems = _repository.GetCompleted(new Guid(currentUser.Id));
            List<TodoViewModel> todoViewModels = new List<TodoViewModel>();
            CompletedViewModel completedViewModel = new CompletedViewModel();
            foreach (TodoItem item in todoItems)
            {
                TodoViewModel model = new TodoViewModel();
                model.Id = item.Id;
                model.Labels = item.Labels;
                model.Text = item.Text;
                model.UserId = item.UserId;
                model.DateCreated = item.DateCreated;
                model.DateDue = item.DateDue;
                model.DateCompleted = item.DateCompleted;
                completedViewModel.Completed.Add(model);
            }

            return View(completedViewModel);
        }

        [HttpGet("RemoveFromCompleted/{Id}")]
        public async Task<IActionResult> RemoveFromCompleted(Guid Id)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            _repository.Remove(Id, new Guid(currentUser.Id));
            return RedirectToAction("Index");
        }


        [HttpGet("MarkAsCompleted/{Id}")]
        public async Task<IActionResult> MarkAsCompleted(Guid Id)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            _repository.MarkAsCompleted(Id, new Guid(currentUser.Id));
            return RedirectToAction("Index");

        }
    }
}