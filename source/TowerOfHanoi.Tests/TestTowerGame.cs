using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using TowerOfHanoi.Tests.Common.DomainRandom;

namespace TowerOfHanoi.Tests
{
  [TestFixture]
  public class TestTowerGame
  {
    [TestFixture]
    public class Constructor
    {
      [Test]
      public void ShouldCreate3DiscsWithSameSize()
      {
        //Arrange
        var expectedSize = RandomTower.Peg.MaxDiscs;
        //Act
        var sut = CreateSut(expectedSize);
        //Assert
        sut.Peg1.MaxDiscs.Should().Be(expectedSize);
        sut.Peg2.MaxDiscs.Should().Be(expectedSize);
        sut.Peg3.MaxDiscs.Should().Be(expectedSize);
      }

      [Test]
      public void ShouldCreate1FullPegAnd3EmptyPegs()
      {
        //Arrange
        //Act
        var sut = CreateSut(3);
        //Assert
        sut.Peg1.CurrentNumberOfDiscs.Should().Be(3);
        sut.Peg2.CurrentNumberOfDiscs.Should().Be(0);
        sut.Peg3.CurrentNumberOfDiscs.Should().Be(0);
      }
    }

    [TestFixture]
    public class PerformMove
    {
      [Test]
      public void WithValidMove_ShouldMoveDiscToNewPeg()
      {
        //Arrange
        var sut = CreateSut(3);
        //Act
        sut.PerformMove(1,2);
        //Assert
        sut.Peg1.CurrentNumberOfDiscs.Should().Be(2);
        sut.Peg2.CurrentNumberOfDiscs.Should().Be(1);
        sut.Peg3.CurrentNumberOfDiscs.Should().Be(0);
      }

      [Test]
      public void WithInvalidValidMove_ShouldThrow()
      {
        //Arrange
        var sut = CreateSut(3);
        sut.PerformMove(1, 2);
        //Act
        Action action = ()=>sut.PerformMove(1, 2);
        //Assert
        action.Should().Throw<InvalidOperationException>().WithMessage("*bigger disc*");
      }

      [Test]
      public void WithInvalidValidMove_ShouldNotRemoveDiscFromTheSourcePeg()
      {
        //Arrange
        var sut = CreateSut(3);
        sut.PerformMove(1, 2);
        //Act
        Action action = () => sut.PerformMove(1, 2);
        //Assert
        action.Should().Throw<InvalidOperationException>();
        sut.Peg1.CurrentNumberOfDiscs.Should().Be(2);
        sut.Peg2.CurrentNumberOfDiscs.Should().Be(1);
        sut.Peg3.CurrentNumberOfDiscs.Should().Be(0);
      }

      [Test]
      public void WithTryingToTakeFromEmptyPeg_ShouldThrow()
      {
        //Arrange
        var sut = CreateSut(3);
        //Act
        Action action = () => sut.PerformMove(2, 3);
        //Assert
        action.Should().Throw<InvalidOperationException>().WithMessage("*empty*");
      }
    }

    private static TowerGame CreateSut(
      int? pegSize = null)
    {
      pegSize ??= RandomTower.Peg.MaxDiscs;
      return new TowerGame(pegSize.Value);
    }
  }
}