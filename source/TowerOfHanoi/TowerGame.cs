using System;
using System.Linq;

namespace TowerOfHanoi
{
  public class TowerGame
  {
    public TowerGame(int pegSize)
    {
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

      foreach (var size in Enumerable.Range(1, PegSize).Reverse())
      {
        _peg1.PlaceDisc(new Disc(size));
      }
    }
    public int PegSize { get; }
    public IReadonlyPeg Peg1 => _peg1;
    public IReadonlyPeg Peg2 => _peg2;
    public IReadonlyPeg Peg3 => _peg3;

    public bool IsGameOver()
    {
      return _peg3.CurrentNumberOfDiscs == PegSize;
    }

    public void PerformMove(int fromPegNumber, int toPegNumber)
    {
      if (fromPegNumber == toPegNumber)
      {
        return;
      }

      var fromPeg = GetPegByNumber(fromPegNumber);
      var toPeg = GetPegByNumber(toPegNumber);

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

    private IPeg GetPegByNumber(int pegNumber)
    {
      return pegNumber switch
      {
        1 => _peg1,
        2 => _peg2,
        3 => _peg3,
        _ => throw new InvalidOperationException("Invalid peg number")
      };
    }
  }
}