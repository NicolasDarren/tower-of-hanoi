using System;
using System.Linq;

namespace TowerOfHanoi
{
  public class TowerGame
  {
    public TowerGame()
    {
      CreateNewGame();
    }

    private const int _pegSize = 9;
    private IPeg _peg1;
    private IPeg _peg2;
    private IPeg _peg3;

    private void CreateNewGame()
    {
      _peg1 = new Peg(_pegSize);
      _peg2 = new Peg(_pegSize);
      _peg3 = new Peg(_pegSize);

      foreach (var size in Enumerable.Range(1,_pegSize).Reverse())
      {
        _peg1.PlaceDisc(new Disc(size));
      }
    }

    public IReadonlyPeg Peg1 => _peg1;
    public IReadonlyPeg Peg2 => _peg2;
    public IReadonlyPeg Peg3 => _peg3;

    public bool IsGameOver()
    {
      return _peg3.CurrentNumberOfDiscs == _pegSize;
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