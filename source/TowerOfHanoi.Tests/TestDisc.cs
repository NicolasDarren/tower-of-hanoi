using FluentAssertions;
using NUnit.Framework;
using TowerOfHanoi.Tests.Common.DomainRandom;

namespace TowerOfHanoi.Tests
{
  [TestFixture]
  public class TestDisc
  {
    [TestFixture]
    public class Constructor
    {
      [Test]
      public void ShouldSetSize()
      {
        //Arrange
        var size = RandomTower.Disc.Size;
        //Act
        var sut = CreateSut(size);
        //Assert
        sut.Size.Should().Be(size);
      }
    }

    private static Disc CreateSut(int size)
    {
      return new Disc(size);
    }
  }
}