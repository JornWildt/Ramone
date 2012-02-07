using System;
namespace Ramone.Tests.Common
{
  // Cats have nine lives - and multiple representations (text, json and html)
  public class Cat
  {
    public string Name { get; set; }
    public DateTime DateOfBirth { get; set; }

    public Cat()
    {
      DateOfBirth = new DateTime(2012, 11, 24, 9, 11, 13);
    }
  }
}
