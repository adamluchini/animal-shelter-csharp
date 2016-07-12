using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace AnimalShelter
{
  public class AnimalShelterTest : IDisposable
  {
    public AnimalShelterTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=animal_shelter;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      Animal.DeleteAll();
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arange, Act
      int result = Animal.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfDescriptionsAreTheSame()
    {
      //Arrange, Act
      Animal firstAnimal = new Animal("Pickles");
      Animal secondAnimal = new Animal("Pickles");

      //Assert
      Assert.Equal(firstAnimal, secondAnimal);
    }

    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      //Arrange
      Animal testAnimal = new Animal("Pickles");

      //Act
      testAnimal.Save();
      List<Animal> result = Animal.GetAll();
      List<Animal> testList = new List<Animal>{testAnimal};

      //Assert
      Assert.Equal(testList, result);
    }
  }
}
