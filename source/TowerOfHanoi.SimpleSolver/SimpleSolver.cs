using System;
using System.Collections.Generic;
using TowerOfHanoi.Common;

namespace TowerOfHanoi.SimpleSolver
{
  public class SimpleSolver : ISolveTowers
  {
    public int MillisecondDelayBetweenMoves { get; } = 400;
    public IEnumerable<TowerGameMoveModel> SolveFromStart(IReadonlyTowerGame game)
    {
      if (game.PegSize != 3)
      {
        throw new NotSupportedException("I can only solve towers that have 3 discs");
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
