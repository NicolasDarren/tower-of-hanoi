namespace TowerOfHanoi.Common
{
  public class Disc : IDisc
  {
    public Disc(int size)
    {
      Size = size;
    }

    public int Size { get; }
  }
}