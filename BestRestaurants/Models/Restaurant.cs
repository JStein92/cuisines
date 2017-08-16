using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace BestRestaurants.Models
{
  public class Restaurant
  {
    private int _id;
    private string _name;
    private int _cuisineId;

    public Restaurant(string name, int cuisineId, int id = 0)
    {
      _id = id;
      _name = name;
      _cuisineId = cuisineId;
    }
    public string GetName()
    {
      return _name;
    }
    public int GetId()
    {
      return _id;
    }
    public int GetCuisineId()
    {
      return _cuisineId;
    }

    public override bool Equals(Object otherRestaurant)
    {
      if (!(otherRestaurant is Restaurant))
      {
        return false;
      }
      else
      {
        Restaurant newRestaurant = (Restaurant) otherRestaurant;

        bool idEquality = (this.GetId() == newRestaurant.GetId());
        bool nameEquality = (this.GetName() == newRestaurant.GetName());
        bool cuisineEquality = (this.GetCuisineId() == newRestaurant.GetCuisineId());

        return (idEquality && nameEquality && cuisineEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.GetName().GetHashCode();
    }

    public static List<Restaurant> GetAll()
    {
      List<Restaurant> restaurantList = new List<Restaurant> {};

      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM restaurants;";

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int restaurantId = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        int cuisineId = rdr.GetInt32(2);
        Restaurant newRestaurant = new Restaurant(name, cuisineId, restaurantId);
        restaurantList.Add(newRestaurant);
      }
      conn.Close();
      return restaurantList;

    }
    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM restaurants;";
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO restaurants (name, cuisine_id) VALUES (@name, @cuisineId);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = this._name;
      cmd.Parameters.Add(name);

      MySqlParameter cuisineId = new MySqlParameter();
      cuisineId.ParameterName = "@cuisineId";
      cuisineId.Value = this._cuisineId;
      cmd.Parameters.Add(cuisineId);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
    }

    public static Restaurant Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM restaurants WHERE id = @restaurantId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@restaurantId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int restaurantId = 0;
      string restaurantName = "";
      int cuisineId = 0;

      while(rdr.Read())
      {
        restaurantId = rdr.GetInt32(0);
        restaurantName = rdr.GetString(1);
        cuisineId = rdr.GetInt32(2);
      }
      Restaurant foundRestaurant = new Restaurant(restaurantName, cuisineId, restaurantId);
      conn.Close();
      return foundRestaurant;
    }
  }
}
