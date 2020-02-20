using NSubstitute;
using TowerOfHanoi.Tests.Common.DomainRandom;

namespace TowerOfHanoi.Tests.Common.Builders
{
  public class FakeDiscDataBuilder
  {
    private IDisc _fake = null;

    private FakeDiscDataBuilder()
    {
      _fake = Substitute.For<IDisc>();
    }

    public static FakeDiscDataBuilder Create()
    {
      return new FakeDiscDataBuilder()
        .WithRandomSize();
    }

    public FakeDiscDataBuilder WithRandomSize()
    {
      return WithSize(RandomTower.Disc.Size);
    }

    public FakeDiscDataBuilder WithSize(int size)
    {
      _fake.Size.Returns(size);
      return this;
    }

    public IDisc Build()
    {
      return _fake;
    }
  }
}