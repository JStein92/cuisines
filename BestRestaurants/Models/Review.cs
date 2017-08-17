using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace BestRestaurants.Models
{
  public class Review
  {
    private int _id;
    private string _poster;
    private int _rating;
    private string _comment;
    private int _restaurantId;
    private DateTime _postTime;

    public Review(string poster, int rating, string comment, int restaurantId, int id = 0)
    {
      _id = id;
      _poster = poster;
      _rating = rating;
      _comment = comment;
      _restaurantId = restaurantId;
    }
    public int GetId()
    {
      return _id;
    }
    public string GetPoster()
    {
      return _poster;
    }
    public int GetRating()
    {
      return _rating;
    }
    public string GetComment()
    {
      return _comment;
    }
    public DateTime GetPostTime()
    {
      return _postTime;
    }
    public void SetDateTime(DateTime postTime)
    {
      _postTime = postTime;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO reviews (poster, rating, comment, post_time, restaurant_id) VALUES (@poster, @rating, @comment, @postTime, @restaurant_id);";

      MySqlParameter poster = new MySqlParameter();
      poster.ParameterName = "@poster";
      poster.Value = _poster;
      cmd.Parameters.Add(poster);

      MySqlParameter rating = new MySqlParameter();
      rating.ParameterName = "@rating";
      rating.Value = _rating;
      cmd.Parameters.Add(rating);

      MySqlParameter comment = new MySqlParameter();
      comment.ParameterName = "@comment";
      comment.Value = _comment;
      cmd.Parameters.Add(comment);

      MySqlParameter restaurantId = new MySqlParameter();
      restaurantId.ParameterName = "@restaurant_id";
      restaurantId.Value = _restaurantId;
      cmd.Parameters.Add(restaurantId);

      MySqlParameter postTime = new MySqlParameter();
      postTime.ParameterName = "@postTime";
      _postTime = DateTime.Now;
      postTime.Value = DateTime.Now;
      cmd.Parameters.Add(postTime);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
    }

  }
}
