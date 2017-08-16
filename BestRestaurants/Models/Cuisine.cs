using System;
using System.Collections.Generic;
using BestRestaurants.Models;
using MySql.Data.MySqlClient;

namespace Cuisines.Models
{
  public class Cuisine
  {
    private int _id;
    private string _name;


    public Cuisine(string name, int id = 0)
    {
      _name = name;
    }

    public static List<Cuisine> GetAll()
    {
      List<Cuisine> cuisineList = new List<Cuisine> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM cuisines;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read());
      {
        int cuisineId = rdr.GetInt32(0);
        string cuisineName = rdr.GetString(1);
        Cuisine newCuisine = new Cuisine(cuisineName, cuisineId);
        cuisineList.Add(newCuisine);
      }
      conn.Close();
      return cuisineList;
    }
  }
}
