namespace TowerOfHanoi.Common
{
  public interface IReadonlyTowerGame
  {
    int MaxDiscsPerPeg { get; }
    IReadonlyPeg Peg1 { get; }
    IReadonlyPeg Peg2 { get; }
    IReadonlyPeg Peg3 { get; }
    bool IsGameUntouched();
    bool IsGameOver();
  }
  
  public interface ITowerGame: IReadonlyTowerGame
  {
    void PerformMove(PegNumber from, PegNumber to);
    IReadonlyTowerGame AsReadonly();
  }
}