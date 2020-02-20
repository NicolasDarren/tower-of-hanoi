using Bogus;

namespace TowerOfHanoi.Tests.Common.DomainRandom
{
  public class RandomDisc
  {
    private Faker _faker;

    public RandomDisc()
    {
      _faker = new Faker();
    }

    public int Size => _faker.Random.Int(1, 15);
  }
}