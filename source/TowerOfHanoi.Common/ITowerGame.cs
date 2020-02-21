﻿namespace TowerOfHanoi.Common
{
  public interface IReadonlyTowerGame
  {
    int PegSize { get; }
    IReadonlyPeg Peg1 { get; }
    IReadonlyPeg Peg2 { get; }
    IReadonlyPeg Peg3 { get; }
    bool IsGameOver();
  }
  public interface ITowerGame: IReadonlyTowerGame
  {
    void PerformMove(int fromPegNumber, int toPegNumber);
    IReadonlyTowerGame AsReadonly();
  }
}