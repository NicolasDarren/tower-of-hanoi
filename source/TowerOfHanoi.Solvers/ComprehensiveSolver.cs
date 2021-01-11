using System;
using System.Collections.Generic;
using TowerOfHanoi.Common;

namespace TowerOfHanoi.Solvers
{
  public class ComprehensiveSolver : ISolveTowers
  {
    public int MillisecondDelayBetweenMoves { get; } = 200;

    public IEnumerable<TowerGameMoveModel> SolveFromStart(IReadonlyTowerGame game)
    {
        if (!game.IsGameUntouched())
        {
            throw new NotSupportedException("This solver can only solve games from the start");
        }

        foreach (var solution in Solve(game.MaxDiscsPerPeg, (PegNumber) 1, (PegNumber) 3, (PegNumber) 2))
            yield return solution;
    }

    public IEnumerable<TowerGameMoveModel> Solve(int gameMaxDiscsPerPeg, PegNumber start, PegNumber destination, PegNumber spare)
    {
        if (gameMaxDiscsPerPeg > 0)
        {
            foreach (var solution in Solve(gameMaxDiscsPerPeg - 1, start, spare, destination))
                yield return solution;
            
            yield return new TowerGameMoveModel {From = start, To = destination};
            
            foreach (var solution in Solve(gameMaxDiscsPerPeg - 1, spare, destination, start))
                yield return solution;
        }
    }
  }
}
