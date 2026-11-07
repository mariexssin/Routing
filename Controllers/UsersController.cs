using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsWithRouting.Models;
using ProductsWithRouting.Services;

namespace ProductsWithRouting.Controllers
{
    public class UsersController : Controller
    {
        private List<User> myUsers;

        public UsersController(Data data)
        {
            myUsers = data.Users;
        }

        [HttpGet]
        [Route("users/index")]
        public IActionResult Index()
        {
            return Unauthorized();
        }

        [HttpPost]
        [Route("users/index")]
        public IActionResult Index([FromForm] string adminKey)
        {
            if (adminKey == "df2323eoT")
            {
                return View("Index", myUsers);
            }

            return Unauthorized();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}