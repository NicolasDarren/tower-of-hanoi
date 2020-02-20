using System;
using System.Transactions;
using FluentAssertions;
using NUnit.Framework;
using TowerOfHanoi.Tests.Common.Builders;
using TowerOfHanoi.Tests.Common.DomainRandom;

namespace TowerOfHanoi.Tests
{
  [TestFixture]
  public class TestPeg
  {
    [TestFixture]
    public class Constructor
    {
      [Test]
      public void ShouldSetMaxDiscs()
      {
        //Arrange
        var maxDiscs = RandomTower.Peg.MaxDiscs;
        //Act
        var sut = CreateSut(maxDiscs);
        //Assert
        sut.MaxDiscs.Should().Be(maxDiscs);
      }

      [Test]
      public void ShouldSetCurrentNumberOfDiscsToZero()
      {
        //Arrange
        var sut = CreateSut();
        //Act
        //Assert
        sut.CurrentNumberOfDiscs.Should().Be(0);
      }
    }

    [TestFixture]
    public class TopDisc
    {
      [Test]
      public void GivenEmptyPeg_ShouldReturnNull()
      {
        //Arrange
        var sut = CreateSut();
        //Act
        var actual = sut.TopDisc;
        //Assert
        actual.Should().BeNull();
      }
    }

    [TestFixture]
    public class PlaceDisc
    {
      [Test]
      public void WithEmptyPeg_ShouldAllowDiscOntoPeg()
      {
        //Arrange
        var disc = FakeDiscDataBuilder.Create().WithSize(1).Build();
        var sut = CreateSut(2);
        //Act
        sut.PlaceDisc(disc);
        //Assert
        sut.CurrentNumberOfDiscs.Should().Be(1);
      }

      [Test]
      public void ShouldNotAllowMoreTanMaxDiscsOnPeg()
      {
        //Arrange
        var disc1 = FakeDiscDataBuilder.Create().WithSize(3).Build();
        var disc2 = FakeDiscDataBuilder.Create().WithSize(2).Build();
        var disc3 = FakeDiscDataBuilder.Create().WithSize(1).Build();
        var sut = CreateSut(2);
        //Act
        sut.PlaceDisc(disc1);
        sut.PlaceDisc(disc2);
        Action action = ()=>sut.PlaceDisc(disc3);
        //Assert
        action.Should().Throw<InvalidOperationException>().WithMessage("This peg is full");
      }

      [Test]
      public void ShouldNotAllowBigDiscOnTopOfSmallDiscs()
      {
        //Arrange
        var disc1 = FakeDiscDataBuilder.Create().WithSize(1).Build();
        var disc2 = FakeDiscDataBuilder.Create().WithSize(2).Build();
        var sut = CreateSut(2);
        //Act
        sut.PlaceDisc(disc1);
        Action action = () => sut.PlaceDisc(disc2);
        //Assert
        action.Should().Throw<InvalidOperationException>().WithMessage("Cannot place bigger disc on top of smaller disc");
      }

      [Test]
      public void ShouldAllowSkippingDiscSizes()
      {
        //Arrange
        var disc1 = FakeDiscDataBuilder.Create().WithSize(3).Build();
        var disc2 = FakeDiscDataBuilder.Create().WithSize(1).Build();
        var sut = CreateSut(3);
        //Act
        sut.PlaceDisc(disc1);
        sut.PlaceDisc(disc2);
        //Assert
        sut.CurrentNumberOfDiscs.Should().Be(2);
      }
    }

    [TestFixture]
    public class TakeTopDisc
    {
      [Test]
      public void GivenEmptyPeg_ShouldThrow()
      {
        //Arrange
        var sut = CreateSut();
        //Act
        Action action = () => sut.TakeTopDisc();
        //Assert
        action.Should().Throw<InvalidOperationException>().WithMessage("This peg is empty");
      }

      [Test]
      public void ShouldReturnLastDiscFromPeg()
      {
        //Arrange
        var disc1 = FakeDiscDataBuilder.Create().WithSize(3).Build();
        var disc2 = FakeDiscDataBuilder.Create().WithSize(2).Build();
        var sut = CreateSut(2);
        sut.PlaceDisc(disc1);
        sut.PlaceDisc(disc2);
        //Act
        var actual = sut.TakeTopDisc();
        //Assert
        actual.Should().Be(disc2);
      }

      [Test]
      public void ShouldRemoveLastDiscFromPeg()
      {
        //Arrange
        var disc1 = FakeDiscDataBuilder.Create().WithSize(3).Build();
        var disc2 = FakeDiscDataBuilder.Create().WithSize(2).Build();
        var sut = CreateSut(2);
        sut.PlaceDisc(disc1);
        sut.PlaceDisc(disc2);
        //Act
        var actual = sut.TakeTopDisc();
        //Assert
        sut.CurrentNumberOfDiscs.Should().Be(1);
      }
    }

    [TestFixture]
    public class AllDiscs
    {
      [Test]
      public void ShouldEnumrateTheDiscsFromTheBottomUp()
      {
        //Arrange
        var disc1 = FakeDiscDataBuilder.Create().WithSize(3).Build();
        var disc2 = FakeDiscDataBuilder.Create().WithSize(2).Build();
        var disc3 = FakeDiscDataBuilder.Create().WithSize(1).Build();
        var sut = CreateSut(3);
        sut.PlaceDisc(disc1);
        sut.PlaceDisc(disc2);
        sut.PlaceDisc(disc3);
        //Act
        //Assert
        sut.AllDiscs().Should().ContainInOrder(disc1, disc2, disc3);
      }
    }

    private static Peg CreateSut(
      int? maxDiscs = null)
    {
      maxDiscs ??= RandomTower.Peg.MaxDiscs;
      return new Peg(maxDiscs.Value);
    }
  }
}