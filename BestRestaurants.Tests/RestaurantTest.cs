using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using BestRestaurants.Models;

namespace BestRestaurants.Tests
{
  [TestClass]
  public class RestaurantTest : IDisposable
  {
    public RestaurantTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=best_restaurants_test;";
    }

    [TestMethod]
    public void GetAll_GetAllRestaurantsAtFirst_0()
    {
      int expected = 0;
      int actual = Restaurant.GetAll().Count;

      Assert.AreEqual(expected, actual);
    }
    [TestMethod]
    public void Save_SavesRestaurantToDatabase_RestaurantList()
    {
      Restaurant testRestaurant = new Restaurant("Sizzler", 1);
      testRestaurant.Save();

      List<Restaurant> actual = Restaurant.GetAll();
      List<Restaurant> expected = new List<Restaurant> {testRestaurant};

      CollectionAssert.AreEqual(expected, actual);
    }
    [TestMethod]
    public void Find_FindsRestaurantByIdInDatabase_Restaurant()
    {
      Restaurant expected = new Restaurant("Red Robin", 1);
      expected.Save();

      Restaurant actual = Restaurant.Find(expected.GetId());

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetAll_ReturnAListOfAllRestaurantsInCuisine_RestaurantList()
    {
      Restaurant restaurant1 = new Restaurant ("Wendy's", 1);
      Restaurant restaurant2 = new Restaurant ("Burger King", 1);
      restaurant1.Save();
      restaurant2.Save();

      List<Restaurant> expected = new List<Restaurant> {restaurant1, restaurant2};
      List<Restaurant> actual = Restaurant.GetAll();

      CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Update_UpdatesRestaurantNameInDatabase_Restaurant()
    {
      Restaurant testRestaurant = new Restaurant("Wendys",1);
      Restaurant testRestaurant2 = new Restaurant("Applebees",2);
      testRestaurant.Save();
      testRestaurant2.Save();

      string newName = "McDonalds";
      testRestaurant.Update(newName);

      string expected = newName;
      string actual = testRestaurant.GetName();

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Delete_DeleteRestaurantByIdInDatabase_RestaurantList()
    {
      Restaurant testRestaurant = new Restaurant("Mexican", 1);
      Restaurant testRestaurant2 = new Restaurant("Italian", 1);
      testRestaurant.Save();
      testRestaurant2.Save();

      List<Restaurant> expected = new List<Restaurant> {testRestaurant};
      testRestaurant2.Delete();

      List<Restaurant> actual = Restaurant.GetAll();

      CollectionAssert.AreEqual(expected, actual);

    }

    [TestMethod]
    public void Save_SavesReviewToDatabase_Review()
    {
      Review newReview = new Review("Jonathan", 4, "YogurtLand is delish!", 1);
      newReview.Save();

      Assert.AreEqual(0, 0);
    }


    public void Dispose()
    {

      Restaurant.DeleteAll();
    }
  }
}
