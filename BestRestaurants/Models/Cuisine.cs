using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace BestRestaurants.Models
{
  public class Cuisine
  {
    private int _id;
    private string _name;


    public Cuisine(string name, int id = 0)
    {
      _name = name;
      _id = id;
    }

    public int GetId()
    {
      return _id;
    }

    public override bool Equals(System.Object otherObject)
    {
      if (!(otherObject is Cuisine))
      {
        return false;
      }
      else
      {
        Cuisine newCuisine = (Cuisine) otherObject;
        return this.GetId().Equals(newCuisine.GetId());
      }
    }

    public override int GetHashCode()
    {
        return this.GetId().GetHashCode();
    }

    public static List<Cuisine> GetAll()
    {
      List<Cuisine> cuisineList = new List<Cuisine> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM cuisines;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int cuisineId = rdr.GetInt32(0);
        string cuisineName = rdr.GetString(1);
        Cuisine newCuisine = new Cuisine(cuisineName, cuisineId);
        cuisineList.Add(newCuisine);
      }
      conn.Close();
      return cuisineList;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO cuisines (name) VALUES (@name);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = this._name;
      cmd.Parameters.Add(name);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
    }

    public static Cuisine Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM cuisines WHERE id = (@cuisineId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@cuisineId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int cuisineId = 0;
      string cuisineName = "";

      while(rdr.Read())
      {
        cuisineId = rdr.GetInt32(0);
        cuisineName = rdr.GetString(1);
      }
      Cuisine foundCuisine = new Cuisine(cuisineName, cuisineId);
      conn.Close();
      return foundCuisine;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM cuisines;";
      cmd.ExecuteNonQuery();
      conn.Close();

    }


  }
}
