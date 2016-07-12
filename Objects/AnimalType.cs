using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace AnimalShelter
{
  public class AnimalType
  {
    private int _id;
    private string _type;

    public AnimalType(string Type, int Id = 0)
    {
      _id = Id;
      _type = Type;
    }

    public override bool Equals(System.Object otherAnimalType)
    {
      if (!(otherAnimalType is AnimalType))
      {
        return false;
      }
      else
      {
      AnimalType newAnimalType = (AnimalType) otherAnimalType;
      bool idEquality = this.GetId() == newAnimalType.GetId();
      bool typeEquality = this.GetType() == newAnimalType.GetType();
      return (idEquality && typeEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }

    public string GetType()
    {
      return _type;
    }

    public void SetType(string newType)
    {
      _type = newType;
    }

    public static List<AnimalType> GetAll()
    {
      List<AnimalType> allAnimalTypes = new List<AnimalType>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM animalTypes;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int animalTypeId = rdr.GetInt32(0);
        string animalTypeType = rdr.GetString(1);
        AnimalType newAnimalType = new AnimalType(animalTypeType, animalTypeId);
        allAnimalTypes.Add(newAnimalType);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allAnimalTypes;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO animalTypes (type) OUTPUT INSERTED.id VALUES (@AnimalTypeType);", conn);

      SqlParameter typeParameter = new SqlParameter();
      typeParameter.ParameterName = "@AnimalType";
      typeParameter.Value = this.GetType();
      cmd.Parameters.Add(typeParameter);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM animalTypes;", conn);
      cmd.ExecuteNonQuery();
    }

    public static AnimalType Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM animalTypes WHERE id = @AnimalTypeId;", conn);
      SqlParameter animalTypeIdParameter = new SqlParameter();
      animalTypeIdParameter.ParameterName = "@AnimalTypeId";
      animalTypeIdParameter.Value = id.ToString();
      cmd.Parameters.Add(animalTypeIdParameter);
      rdr = cmd.ExecuteReader();

      int foundAnimalTypeId = 0;
      string foundAnimalTypeAnimalName = null;

      while(rdr.Read())
      {
        foundAnimalTypeId = rdr.GetInt32(0);
        foundAnimalTypeAnimalName = rdr.GetString(1);
      }

      AnimalType foundAnimalType = new AnimalType(foundAnimalTypeAnimalName, foundAnimalTypeId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundAnimalType;
    }

  }
}
