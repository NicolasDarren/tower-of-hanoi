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

        foreach (var solution in Solve(game.MaxDiscsPerPeg, 1, 3, 2))
            yield return solution;
    }

    public IEnumerable<TowerGameMoveModel> Solve(int gameMaxDiscsPerPeg, int start, int destination, int auxiliary)
    {
        if (gameMaxDiscsPerPeg > 0)
        {
            foreach (var solution in Solve(gameMaxDiscsPerPeg - 1, start, auxiliary, destination))
                yield return solution;
            yield return new TowerGameMoveModel {From = (PegNumber) start, To = (PegNumber) destination };
            foreach (var solution in Solve(gameMaxDiscsPerPeg - 1, auxiliary, destination, start))
                yield return solution;
        }
    }
  }
}
