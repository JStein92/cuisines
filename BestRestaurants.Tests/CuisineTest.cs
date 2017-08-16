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
    public void Update_UpdatesNameOfCuisineInDatabase_Cuisine()
    {
      Cuisine testCuisine = new Cuisine("Thai");
      testCuisine.Save();
      testCuisine.Update("Thai Fusion");

      
    }

    public void Dispose()
    {
      // Restaurant.DeleteAll();
      Cuisine.DeleteAll();
    }
  }
}
