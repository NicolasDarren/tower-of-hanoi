using Bogus;

namespace TowerOfHanoi.Tests.Common.DomainRandom
{
  public class RandomPeg
  {
    private Faker _faker;

    public RandomPeg()
    {
      _faker = new Faker();
    }

    public int MaxDiscs => _faker.Random.Int(1, 15);
  }
}