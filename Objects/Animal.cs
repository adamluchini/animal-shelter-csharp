using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace AnimalShelter
{
  public class Animal
  {
    private int _id;
    private string _animalName;

    public Animal(string AnimalName, int Id = 0)
    {
      _id = Id;
      _animalName = AnimalName;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetAnimalName()
    {
      return _animalName;
    }

    public void SetAnimalName(string newAnimalName)
    {
      _animalName = newAnimalName;
    }

    public static List<Animal> GetAll()
    {
      List<Animal> allAnimals = new List<Animal>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM animals;", conn);
      rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        int animalId = rdr.GetInt32(0);
        string animalAnimalName = rdr.GetString(1);
        Animal newAnimal = new Animal(animalAnimalName, animalId);
        allAnimals.Add(newAnimal);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allAnimals;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM animals;", conn);
      cmd.ExecuteNonQuery();
    }

    public override bool Equals(System.Object otherAnimal)
    {
      if (!(otherAnimal is Animal))
      {
        return false;
      }
      else
      {
        Animal newAnimal = (Animal) otherAnimal;
        bool animalNameEquality = (this.GetAnimalName() == newAnimal.GetAnimalName());
        return (animalNameEquality);
      }
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO animals (animal_name) OUTPUT INSERTED.id VALUES (@AnimalAnimalName);", conn);

      SqlParameter animalNameParameter = new SqlParameter();
      animalNameParameter.ParameterName = "@AnimalAnimalName";
      animalNameParameter.Value = this.GetAnimalName();
      cmd.Parameters.Add(animalNameParameter);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr.Read())
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static Animal Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM animals WHERE id = @AnimalId;", conn);
      SqlParameter animalIdParameter = new SqlParameter();
      animalIdParameter.ParameterName = "AnimalId";
      animalIdParameter.Value = id.ToString();
      cmd.Parameters.Add(animalIdParameter);
      rdr = cmd.ExecuteReader();

      int foundAnimalId = 0;
      string foundAnimalAnimalName = null;
      while(rdr.Read())
      {
        foundAnimalId = rdr.GetInt32(0);
        foundAnimalAnimalName = rdr.GetString(1);
      }
      Animal foundAnimal = new Animal(foundAnimalAnimalName, foundAnimalId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn !=null)
      {
        conn.Close();
      }

      return foundAnimal;
      
    }

  }
}
