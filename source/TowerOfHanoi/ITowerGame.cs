namespace TowerOfHanoi
{
  public interface ITowerGame
  {
    IReadonlyPeg Peg1 { get; }
    IReadonlyPeg Peg2 { get; }
    IReadonlyPeg Peg3 { get; }
    bool IsGameOver();
    void PerformMove(int fromPegNumber, int toPegNumber);
  }
}