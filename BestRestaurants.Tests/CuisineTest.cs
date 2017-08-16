using Microsoft.VisualStudio.TestTools.UnitTesting;
using BestRestaurants.Models;
using Cuisines.Models;

namespace BestRestaurants.Tests
{
  [TestClass]
  public class CuisineTest
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

    // [TestMethod]
    // public void GetAll_GetAllCuisinesAfterCuisineAdded_CuisineList()
    // {
    //   Cuisine newCuisine = new Cuisine("Chinese");
    //   newCuisine.Save();
    //
    //   List<Cuisine> cuisineList
  }
}
