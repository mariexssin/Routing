using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductsWithRouting.Models;
using ProductsWithRouting.Services;

namespace ProductsWithRouting.Controllers
{
    public class ProductsController : Controller
    {
        private List<Product> myProducts;

        public ProductsController(Data data)
        {
            myProducts = data.Products;
        }

        [HttpGet]
        [Route("products")]
        [Route("products/index")]
        [Route("items")]
        [Route("items/index")]
        public IActionResult Index(int? filterId, string filtername)
        {
            var query = myProducts.AsEnumerable();

            if (filterId.HasValue)
            {
                query = query.Where(p => p.Id == filterId.Value);
            }

            if (!string.IsNullOrEmpty(filtername))
            {
                query = query.Where(p => p.Name != null && p.Name.Contains(filtername, StringComparison.OrdinalIgnoreCase));
            }

            return View(query.ToList());
        }

        [HttpGet]
        [Route("products/{id:int}")]
        public IActionResult View(int id)
        {
            var product = myProducts.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return RedirectToAction("NotFoundCustom");
            }
            return View(product);
        }

        [HttpGet]
        [Route("products/edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var product = myProducts.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return RedirectToAction("NotFoundCustom");
            }
            return View(product);
        }

        [HttpPost]
        [Route("products/edit/{id:int}")]
        public IActionResult Edit(Product product)
        {
            var existingProduct = myProducts.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct == null)
            {
                return RedirectToAction("NotFoundCustom");
            }

            if (ModelState.IsValid)
            {
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [HttpGet]
        [Route("products/create")]
        [Route("products/new")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("products/create")]
        [Route("products/new")]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                product.Id = myProducts.Any() ? myProducts.Max(p => p.Id) + 1 : 1;
                myProducts.Add(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [HttpGet]
        [Route("products/delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            var product = myProducts.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return RedirectToAction("NotFoundCustom");
            }
            myProducts.Remove(product);
            return RedirectToAction("Index");
        }

        [Route("products/notfound")]
        public IActionResult NotFoundCustom()
        {
            Response.StatusCode = 404;
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}