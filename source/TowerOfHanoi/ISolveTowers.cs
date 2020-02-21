using System.Collections.Generic;

namespace TowerOfHanoi
{
  public interface ISolveTowers
  {
    int MillisecondDelayBetweenMoves { get; }
    IEnumerable<TowerGameMoveModel> SolveFromStart(TowerGame game);
  }
}