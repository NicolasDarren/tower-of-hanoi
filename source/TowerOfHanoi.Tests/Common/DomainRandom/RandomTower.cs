namespace TowerOfHanoi.Tests.Common.DomainRandom
{
  public static class RandomTower
  {
    public static RandomDisc Disc { get; } = new RandomDisc();
    public static RandomPeg Peg { get; } = new RandomPeg();
  }
}