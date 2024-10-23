using Homework_Food_02.Models;
using Microsoft.AspNetCore.Mvc;

namespace Homework_Food_02.Controllers;

public class FoodController : Controller
{
    private static List<Food> _foodList = new List<Food>
    {
        new Food { Id = 1, Name = "Борщ", Weight = 350, Price = 50 },
        new Food { Id = 2, Name = "Вареники", Weight = 200, Price = 40 }
    };
    
    public IActionResult Index()
    {
        return View(_foodList);
    }
    
    [HttpGet]
    public IActionResult AddFood()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult AddFood(Food food)
    {
        if (ModelState.IsValid)
        {
            food.Id = _foodList.Max(f => f.Id) + 1;
            _foodList.Add(food);
            return RedirectToAction("Index");
        }
        return View(food);
    }
}