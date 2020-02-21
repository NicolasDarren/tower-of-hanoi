﻿using System;
using System.Linq;

namespace TowerOfHanoi
{
  public class TowerGame: ITowerGame
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