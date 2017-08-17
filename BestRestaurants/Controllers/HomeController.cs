using Microsoft.AspNetCore.Mvc;
using BestRestaurants.Models;
using System.Collections.Generic;
using System;

namespace BestRestaurants.Controllers
{
    public class HomeController : Controller
    {
      [HttpGet("/")]
      public ActionResult Index()
      {
        return View(Cuisine.GetAll());
      }

      [HttpPost("/")]
      public ActionResult IndexPost()
      {
        string cuisineName = Request.Form["cuisine-name"];
        Cuisine newCuisine = new Cuisine(cuisineName);
        newCuisine.Save();
        List<Cuisine> allCuisines = Cuisine.GetAll();
        return View("Index", allCuisines);
      }

      [HttpGet("/cuisine-details/{id}")]
      public ActionResult CuisineDetails(int id)
      {
        Cuisine newCuisine = Cuisine.Find(id);

        return View(newCuisine);
      }

      [HttpPost("/cuisine-details/{id}")]
      public ActionResult CuisineDetailsPost(int id)
      {
        Restaurant newRestaurant = new Restaurant(Request.Form["restaurant-name"], id);

        newRestaurant.Save();

        return View("CuisineDetails", Cuisine.Find(id));
      }

      [HttpGet("/cuisine-details/{id}/edit")]
      public ActionResult CuisineEdit(int id)
      {
        return View(Cuisine.Find(id));
      }

      [HttpPost("/cuisine-edited/{id}")]
      public ActionResult CuisineEdited(int id)
      {

        string updatedCuisineName = Request.Form["updated-cuisine"];

        Cuisine newCuisine = Cuisine.Find(id);
        newCuisine.Update(updatedCuisineName);

        return View("Index",Cuisine.GetAll());
      }

      [HttpGet("/cuisine-details/{cuisineId}/edit/{restId}")]
      public ActionResult RestaurantEdit(int cuisineId, int restId)
      {
        Dictionary<string,object> model = new Dictionary<string,object>{};
        Cuisine myCuisine = Cuisine.Find(cuisineId);
        Restaurant myRestaurant = Restaurant.Find(restId);

        model.Add("cuisine", myCuisine);
        model.Add("restaurant", myRestaurant);
        return View(model);
      }

      [HttpPost("cuisine-details/{cuisineId}/restaurant-edited/{restId}")]
      public ActionResult RestaurantEdited(int cuisineId, int restId)
      {
        Restaurant updatedRestaurant = Restaurant.Find(restId);
        updatedRestaurant.Update(Request.Form["updated-restaurant"]);

        return View("CuisineDetails", Cuisine.Find(cuisineId));
      }
    }
}
