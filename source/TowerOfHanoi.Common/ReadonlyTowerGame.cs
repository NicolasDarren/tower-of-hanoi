using System.Collections.Generic;

namespace TowerOfHanoi.Common
{
  public class ReadonlyTowerGame : IReadonlyTowerGame
  {
    private readonly ITowerGame _underlying;

    public ReadonlyTowerGame(ITowerGame underlying)
    {
      _underlying = underlying;
    }

    public int PegSize => _underlying.PegSize;

    public IReadonlyPeg Peg1 => _underlying.Peg1;

    public IReadonlyPeg Peg2 => _underlying.Peg2;

    public IReadonlyPeg Peg3 => _underlying.Peg3;

    public bool IsGameUntouched()
    {
      return _underlying.IsGameUntouched();
    }

    public bool IsGameOver()
    {
      return _underlying.IsGameOver();
    }
  }
}