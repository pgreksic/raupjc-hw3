using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zadatak1;

namespace Zadatak2.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoRepository _repository;

        public TodoController(ITodoRepository repository)
        {
            _repository = repository;
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}