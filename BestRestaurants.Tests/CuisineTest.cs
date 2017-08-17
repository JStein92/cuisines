using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using BestRestaurants.Models;
using System;

namespace BestRestaurants.Tests
{
  [TestClass]
  public class CuisineTest : IDisposable
  {
    public CuisineTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=best_restaurants_test;";
    }

    [TestMethod]
    public void GetAll_GetAllCuisinesAtFirst_0()
    {
      int actual = Cuisine.GetAll().Count;

      Assert.AreEqual(0, actual);
    }

    [TestMethod]
    public void Save_SavesCuisineToDataBase_CuisineList()
    {
      Cuisine newCuisine = new Cuisine("Chinese");
      newCuisine.Save();

      List<Cuisine> expectedCuisineList = new List<Cuisine>{newCuisine};
      List<Cuisine> actualCuisineList = Cuisine.GetAll();

      CollectionAssert.AreEqual(expectedCuisineList, actualCuisineList);
    }

    [TestMethod]
    public void Find_FindsCuisineByIdInDatabase_Cuisine()
    {
      Cuisine testCuisine = new Cuisine("Chinese");
      testCuisine.Save();

      Cuisine expected = testCuisine;
      Cuisine actual = Cuisine.Find(testCuisine.GetId());

      Assert.AreEqual(expected, actual);

    }

    [TestMethod]
    public void Update_UpdatesCuisineNameInDatabase_Cuisine()
    {
      string name = "Thai";
      Cuisine testCuisine = new Cuisine(name);
      testCuisine.Save();
      string newName = "Thai Fusion";

      testCuisine.Update(newName);

      string expected = newName;
      string actual = testCuisine.GetName();

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Delete_DeleteCuisineByIdInDatabase_CuisineList()
    {
      Cuisine cuisine1 = new Cuisine("Mexican");
      Cuisine cuisine2 = new Cuisine("Italian");
      cuisine1.Save();
      cuisine2.Save();

      List<Cuisine> expected = new List<Cuisine> {cuisine2};
      cuisine1.Delete();

      List<Cuisine> actual = Cuisine.GetAll();

      CollectionAssert.AreEqual(expected, actual);

    }

    [TestMethod]
    public void Delete_DeleteAllResturantsFromSpecificCuisine_RestaurantList()
    {
      Cuisine cuisine1 = new Cuisine("Mexican");
      Cuisine cuisine2 = new Cuisine("Italian");
      cuisine1.Save();
      cuisine2.Save();

      Restaurant restaurant1 = new Restaurant("El Camion", cuisine1.GetId());
      Restaurant restaurant2 = new Restaurant("Hot Mama's Pizza", cuisine2.GetId());
      restaurant1.Save();
      restaurant2.Save();

      List<Restaurant> expected = new List<Restaurant> {restaurant2};

      cuisine1.Delete();
      List<Restaurant> actual = Restaurant.GetAll();

      CollectionAssert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void SearchAllRestaurants_ReturnsAllRestaurantsOfSelectedCuisine_RestaurantList()
    {
      Cuisine cuisine1 = new Cuisine("Mexican");
      cuisine1.Save();
      Cuisine cuisine2 = new Cuisine("Italian");
      cuisine1.Save();

      Restaurant restaurant1 = new Restaurant("El Camion", cuisine1.GetId());
      Restaurant restaurant2 = new Restaurant("Taco Time", cuisine1.GetId());
      Restaurant restaurant3 = new Restaurant("Hot Mama's Pizza", cuisine2.GetId());

      restaurant1.Save();
      restaurant2.Save();
      restaurant3.Save();

      List<Restaurant> actual = cuisine1.SearchAllRestaurants();
      List<Restaurant> expected = new List<Restaurant>{restaurant1, restaurant2};

      CollectionAssert.AreEqual(expected,actual);
    }

    public void Dispose()
    {
      Restaurant.DeleteAll();
      Cuisine.DeleteAll();
    }
  }
}
