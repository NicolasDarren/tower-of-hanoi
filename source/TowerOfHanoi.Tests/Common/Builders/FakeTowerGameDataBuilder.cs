using NSubstitute;
using TowerOfHanoi.Common;
using TowerOfHanoi.Tests.Common.DomainRandom;

namespace TowerOfHanoi.Tests.Common.Builders
{
  public class FakeTowerGameDataBuilder : TestDataBuilder<FakeTowerGameDataBuilder, ITowerGame>
  {
    public override FakeTowerGameDataBuilder WithRandomProps()
    {
      return WithUntouchedGame().
        WithRandomMaxDiscsPerPeg();
    }

    public FakeTowerGameDataBuilder WithRandomMaxDiscsPerPeg()
    {
      return WithProp(f => f.MaxDiscsPerPeg.Returns(RandomTower.Peg.MaxDiscs));
    }

    public FakeTowerGameDataBuilder WithMaxDiscsPerPeg(int discs)
    {
      return WithProp(f => f.MaxDiscsPerPeg.Returns(discs));
    }

    public FakeTowerGameDataBuilder WithUntouchedGame()
    {
      return WithProp(f => f.IsGameUntouched().Returns(true));
    }
    public FakeTowerGameDataBuilder WithTouchedGame()
    {
      return WithProp(f => f.IsGameUntouched().Returns(false));
    }
  }
}