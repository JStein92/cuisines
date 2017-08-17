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

    private static string _reviewFilter = "newToOld";

    public Restaurant(string name, int cuisineId, int id = 0)
    {
      _id = id;
      _name = name;
      _cuisineId = cuisineId;
    }
    public static void SetReviewFilter(string filter)
    {
      _reviewFilter = filter;
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

    public void Update(string newName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE restaurants SET name = @newName WHERE id = @thisId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@thisId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@newName";
      name.Value = newName;
      cmd.Parameters.Add(name);

      cmd.ExecuteNonQuery();
      conn.Close();
      _name = newName;
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM restaurants WHERE id = @thisId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@thisId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public List<Review> SearchAllReviews()
    {
      List<Review> reviewList = new List<Review> {};

      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;

      if (_reviewFilter == "bestToWorst")
      {
        cmd.CommandText = @"SELECT * FROM reviews WHERE restaurant_id = @thisId ORDER BY rating DESC;";
      }
      else if (_reviewFilter == "worstToBest")
      {
        cmd.CommandText = @"SELECT * FROM reviews WHERE restaurant_id = @thisId ORDER BY rating ASC;";
      }
      else if (_reviewFilter == "oldToNew")
      {
        cmd.CommandText = @"SELECT * FROM reviews WHERE restaurant_id = @thisId ORDER BY post_time ASC;";
      }
      else
      {
        cmd.CommandText = @"SELECT * FROM reviews WHERE restaurant_id = @thisId ORDER BY post_time DESC;";
      }


      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@thisId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      while(rdr.Read())
      {
          int reviewId = rdr.GetInt32(0);
          string poster = rdr.GetString(1);
          int rating = rdr.GetInt32(2);
          string comment = rdr.GetString(3);
          int restaurantId = rdr.GetInt32(4);
          DateTime postTime = rdr.GetDateTime(5);

          Review newReview = new Review(poster, rating, comment, restaurantId, reviewId);
          newReview.SetDateTime(postTime);
          reviewList.Add(newReview);

      }
      conn.Close();
      return reviewList;
    }
  }
}
