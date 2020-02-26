using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using TowerOfHanoi.Tests.Common.Builders;

namespace TowerOfHanoi.Solvers.Tests
{
  [TestFixture]
  public class TestSimpleSolver
  {
    [Test]
    public void GivenTowerWithMoreThan3Discs_Throws()
    {
      //Arrange
      var game = FakeTowerGameDataBuilder.CreateWithRandomProps().WithMaxDiscsPerPeg(4).Build();
      var sut = CreateSut();
      //Act
      Action action = () => sut.SolveFromStart(game).ToArray();
      //Assert
      action.Should().Throw<NotSupportedException>()
        .WithMessage("I'm a SIMPLE solver, I can only solve towers that have 3 discs");
    }

    [Test]
    public void ShouldReturn7Moves()
    {
      //Arrange
      var game = FakeTowerGameDataBuilder.CreateWithRandomProps().WithMaxDiscsPerPeg(3).Build();
      var sut = CreateSut();
      //Act
      var actual = sut.SolveFromStart(game).ToArray();
      //Assert
      actual.Should().HaveCount(7);
    }

    private static SimpleSolver CreateSut()
    {
      return new SimpleSolver();
    }
  }
}
