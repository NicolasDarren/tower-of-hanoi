using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TowerOfHanoi.Common;
using TowerOfHanoi.Tests.Common.Builders;
using TowerOfHanoi.Tests.Common.DomainRandom;

namespace TowerOfHanoi.Solvers.Tests
{
  [TestFixture]
  public class TestComprehensiveSolver
  {
    [Test]
    public void GivenGameNotAtStart_Throws()
    {
      //Arrange
      var game = FakeTowerGameDataBuilder.CreateWithRandomProps().WithTouchedGame().Build();
      var sut = CreateSut();
      //Act
      Action action = () => sut.SolveFromStart(game).ToArray();
      //Assert
      action.Should().Throw<NotSupportedException>()
        .WithMessage("This solver can only solve games from the start");
    }

    [Test]
    [Explicit]
    public void ShouldBeAbleToSolveARealGame()
    {
      //Arrange
      var game = new TowerGame(RandomTower.Peg.MaxDiscs);
      var sut = CreateSut();
      //Act
      foreach (var move in sut.SolveFromStart(game))
      {
        game.PerformMove(move.From, move.To);
      }
      //Assert
      game.IsGameOver().Should().BeTrue("The game has not been solved");
    }

    [Test]
    [Explicit]
    public void ShouldReturnOptimalAmountOfMoves()
    {
      //Arrange
      var discsPerPeg = RandomTower.Peg.MaxDiscs;
      var expectedMoves = Convert.ToInt32(Math.Pow(2, discsPerPeg) - 1);
      var game = FakeTowerGameDataBuilder
        .CreateWithRandomProps()
        .WithMaxDiscsPerPeg(discsPerPeg)
        .WithUntouchedGame()
        .Build();
      var sut = CreateSut();
      //Act
      var actual = sut.SolveFromStart(game).ToArray();
      //Assert
      actual.Should().HaveCount(expectedMoves);
    }

    private static ComprehensiveSolver CreateSut()
    {
      return new ComprehensiveSolver();
    }
  }
}