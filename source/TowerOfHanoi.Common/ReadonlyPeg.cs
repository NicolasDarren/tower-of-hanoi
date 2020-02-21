using System.Collections.Generic;

namespace TowerOfHanoi.Common
{
  public class ReadonlyPeg : IReadonlyPeg
  {
    private readonly IPeg _underlying;

    public ReadonlyPeg(IPeg underlying)
    {
      _underlying = underlying;
    }

    public int MaxDiscs => _underlying.MaxDiscs;
    public int CurrentNumberOfDiscs => _underlying.CurrentNumberOfDiscs;
    public IDisc TopDisc => _underlying.TopDisc;
    public IEnumerable<IDisc> AllDiscs()
    {
      return _underlying.AllDiscs();
    }
  }
}