using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace AnimalShelter
{
  public class AnimalTypeTest : IDisposable
  {
    public AnimalTypeTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=animal_shelter;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_AnimalTypeEmptyAtFirst()
    {
      //Arrange, Act
      int result = AnimalType.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueforSameType()
    {
      //Arrange, Act
      AnimalType firstAnimalType = new AnimalType("Dog");
      AnimalType secondAnimalType = new AnimalType("Dog");

      //Assert
      Assert.Equal(firstAnimalType, secondAnimalType);
    }

    [Fact]
    public void Test_Save_SavesAnimalTypeToDatabase()
    {
      //Arrange
      AnimalType testAnimalType = new AnimalType("Dog");
      testAnimalType.Save()

      //Act
      List<AnimalType> result = AnimalType.GetAll();
      List<AnimalType> testList = new List<AnimalType>{testAnimalType};

      //Assert
      Assert.Equal(testList, result);
    }

      [Fact]
      public void Test_Save_AssignsIdToAnimalTypeObject()
      {
        //Arrange
        AnimalType testAnimalType = new AnimalType("Dog");
        testAnimalType.Save();

        //Act
        AnimalType savedAnimalType = AnimalType.GetAll()[0];

        int result = savedAnimalType.GetId();
        int testId = testAnimalType.GetId();

        //Assert
        Assert.Equal(testId, result);
      }

      [Fact]
      public void Test_Find_FindsAnimalTypeInDatabase()
      {
        //Arrange
        AnimalType testAnimalType = new AnimalType("Dog");
        testAnimalType.Save();

        //Act
        AnimalType foundAnimalType = AnimalType.Find(testAnimalType.GetId());

        //Assert
        Assert.Equal(testAnimalType, foundAnimalType);
      }

      public void Dispose()
      {
        Animal.DeleteAll();
        AnimalType.DeleteAll()
      }
  }
}
