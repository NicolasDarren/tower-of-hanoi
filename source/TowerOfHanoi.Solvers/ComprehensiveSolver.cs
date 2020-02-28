using System;
using System.Collections.Generic;
using TowerOfHanoi.Common;

namespace TowerOfHanoi.Solvers
{
  public class ComprehensiveSolver : ISolveTowers
  {
    public int MillisecondDelayBetweenMoves { get; } = 100;

    public IEnumerable<TowerGameMoveModel> SolveFromStart(IReadonlyTowerGame game)
    {
      if (!game.IsGameUntouched())
      {
        throw new NotSupportedException("This solver can only solve games from the start");
      }

      //TODO: implement this.
      //Your code goes here
      //Sample:
      //yield return TowerGameMoveModel.CreateMove(PegNumber.One, PegNumber.Three);
    }
  }
}
