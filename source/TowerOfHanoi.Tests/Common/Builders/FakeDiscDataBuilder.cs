using NSubstitute;
using TowerOfHanoi.Common;
using TowerOfHanoi.Tests.Common.DomainRandom;

namespace TowerOfHanoi.Tests.Common.Builders
{
  public class FakeDiscDataBuilder : TestDataBuilder<FakeDiscDataBuilder, IDisc>
  {
    public override FakeDiscDataBuilder WithRandomProps()
    {
      return WithRandomSize();
    }

    public FakeDiscDataBuilder WithRandomSize()
    {
      return WithProp(f => f.Size.Returns(RandomTower.Disc.Size));
    }

    public FakeDiscDataBuilder WithSize(int size)
    {
      return WithProp(f => f.Size.Returns(size));
    }
  }
}