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

      yield return new TowerGameMoveModel { FromPegNumber = 1, ToPegNumber = 3 };
      yield return new TowerGameMoveModel { FromPegNumber = 1, ToPegNumber = 2 };
      yield return new TowerGameMoveModel { FromPegNumber = 3, ToPegNumber = 2 };
      yield return new TowerGameMoveModel { FromPegNumber = 1, ToPegNumber = 3 };
      yield return new TowerGameMoveModel { FromPegNumber = 2, ToPegNumber = 1 };
      yield return new TowerGameMoveModel { FromPegNumber = 2, ToPegNumber = 3 };
      yield return new TowerGameMoveModel { FromPegNumber = 1, ToPegNumber = 3 };
    }
  }
}
