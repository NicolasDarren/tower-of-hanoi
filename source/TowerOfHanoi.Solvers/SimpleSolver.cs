using System;
using System.Collections.Generic;
using TowerOfHanoi.Common;

namespace TowerOfHanoi.Solvers
{
  public class SimpleSolver : ISolveTowers
  {
    public int MillisecondDelayBetweenMoves { get; } = 400;
    public IEnumerable<TowerGameMoveModel> SolveFromStart(IReadonlyTowerGame game)
    {
      if (game.MaxDiscsPerPeg != 3)
      {
        throw new NotSupportedException("I'm a SIMPLE solver, I can only solve towers that have 3 discs");
      }

      yield return new TowerGameMoveModel { From = PegNumber.One, To = PegNumber.Three };
      yield return new TowerGameMoveModel { From = PegNumber.One, To = PegNumber.Two };
      yield return new TowerGameMoveModel { From = PegNumber.Three, To = PegNumber.Two };
      yield return new TowerGameMoveModel { From = PegNumber.One, To = PegNumber.Three };
      yield return new TowerGameMoveModel { From = PegNumber.Two, To = PegNumber.One };
      yield return new TowerGameMoveModel { From = PegNumber.Two, To = PegNumber.Three };
      yield return new TowerGameMoveModel { From = PegNumber.One, To = PegNumber.Three };
    }
  }
}
