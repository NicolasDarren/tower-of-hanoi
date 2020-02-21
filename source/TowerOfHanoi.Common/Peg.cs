using System;
using System.Collections.Generic;
using System.Linq;

namespace TowerOfHanoi.Common
{
  public class Peg: IPeg
  {
    public Peg(int maxDiscs)
    {
      MaxDiscs = maxDiscs;
    }

    private readonly List<IDisc> _discs = new List<IDisc>();

    public int MaxDiscs { get; }

    public int CurrentNumberOfDiscs => _discs.Count;

    public IDisc TopDisc => _discs.LastOrDefault();

    public IEnumerable<IDisc> AllDiscs()
    {
      return _discs;
    }

    public IDisc TakeTopDisc()
    {
      var topDisc = TopDisc;

      if (topDisc == null)
      {
        throw new InvalidOperationException("This peg is empty");
      }

      _discs.Remove(topDisc);

      return topDisc;
    }

    public void PlaceDisc(IDisc disc)
    {
      if (CurrentNumberOfDiscs >= MaxDiscs)
      {
        throw new InvalidOperationException("This peg is full");
      }

      if (TopDisc != null &&
          TopDisc.Size <= disc.Size)
      {
        throw new InvalidOperationException("Cannot place bigger disc on top of smaller disc");
      }

      _discs.Add(disc);
    }
  }
}