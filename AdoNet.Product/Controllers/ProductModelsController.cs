using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdoNet.Product.Models;
using System.Data.SqlClient;
using DAL;
using System.Diagnostics;
using BL.Logic;
using Microsoft.AspNetCore.Http;

namespace AdoNet.Product.Controllers
{
    public class ProductModelsController : Controller
    {
        private readonly Logic pro = null;
        public ProductModelsController()
        {
            pro = new Logic();
        }
        public IActionResult Index()
        {
            List<ProductModel> prod = new List<ProductModel>();
            prod = pro.GetAllProducts();
            return View(prod);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ProductModel prod)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    pro.InsertNewProduct(prod);
                }
            }

            catch (NullReferenceException ex)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, new { ErrorMessage = "Vat Baner mi Ara" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ErroMessage = ex.Message, Code = StatusCodes.Status400BadRequest });
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            pro.DeleteSelectedProduct(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            ProductModel product = new ProductModel();
            product = pro.FoundProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductModel product)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    pro.SaveEditedProduct(product);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        public IActionResult Details(int id)
        {

            var apranq = pro.FoundProduct(id);

            if (apranq == null)
            {
                return NotFound();
            }

            return View(apranq);
        }

    }
}
