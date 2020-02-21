using System.Collections.Generic;

namespace TowerOfHanoi.Common
{
  public interface ISolveTowers
  {
    int MillisecondDelayBetweenMoves { get; }
    IEnumerable<TowerGameMoveModel> SolveFromStart(IReadonlyTowerGame game);
  }
}