using System;
using System.Linq;

namespace TowerOfHanoi.Common
{
  public class TowerGame: ITowerGame
  {
    public TowerGame(int pegSize)
    {
      if (pegSize < 1)
      {
        throw new InvalidOperationException("Cannot start empty Game");
      }

      PegSize = pegSize;
      CreateNewGame();
    }

    private IPeg _peg1;
    private IPeg _peg2;
    private IPeg _peg3;

    private void CreateNewGame()
    {
      _peg1 = new Peg(PegSize);
      _peg2 = new Peg(PegSize);
      _peg3 = new Peg(PegSize);
      Peg1 = new ReadonlyPeg(_peg1);
      Peg2 = new ReadonlyPeg(_peg2);
      Peg3 = new ReadonlyPeg(_peg3);

      foreach (var size in Enumerable.Range(1, PegSize).Reverse())
      {
        _peg1.PlaceDisc(new Disc(size));
      }
    }
    public int PegSize { get; }
    public IReadonlyPeg Peg1 { get; private set; } 
    public IReadonlyPeg Peg2 { get; private set; }
    public IReadonlyPeg Peg3 { get; private set; }

    public bool IsGameUntouched()
    {
      return _peg1.CurrentNumberOfDiscs == PegSize;
    }

    public bool IsGameOver()
    {
      return _peg3.CurrentNumberOfDiscs == PegSize;
    }

    public void PerformMove(PegNumber from, PegNumber to)
    {
      if (from == to)
      {
        return;
      }

      var fromPeg = GetPegByNumber(from);
      var toPeg = GetPegByNumber(to);

      var disc = fromPeg.TakeTopDisc();
      try
      {
        toPeg.PlaceDisc(disc);
      }
      catch
      {
        fromPeg.PlaceDisc(disc);
        throw;
      }
    }

    public IReadonlyTowerGame AsReadonly()
    {
      return new ReadonlyTowerGame(this);
    }

    private IPeg GetPegByNumber(PegNumber pegNumber)
    {
      return pegNumber switch
      {
        PegNumber.One => _peg1,
        PegNumber.Two => _peg2,
        PegNumber.Three => _peg3,
        _ => throw new InvalidOperationException("Invalid peg number")
      };
    }
  }
}