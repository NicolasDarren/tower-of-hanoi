namespace TowerOfHanoi
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