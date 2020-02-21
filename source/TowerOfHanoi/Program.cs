using System;
using System.Threading;
using TowerOfHanoi.Common;

namespace TowerOfHanoi
{
  class Program
  {
    static void Main(string[] args)
    {
      var game = new TowerGame(3);
      var drawer = new ConsoleDrawer();
      var solver = new SimpleSolver.SimpleSolver();

      var lastError = "";
      drawer.Draw(game);

      Console.CursorLeft = 0;
      Console.CursorTop = game.PegSize + 2;

      foreach (var move in solver.SolveFromStart(game.AsReadonly()))
      {
        Thread.Sleep(solver.MillisecondDelayBetweenMoves);

        try
        {
          game.PerformMove(move.FromPegNumber, move.ToPegNumber);
        }
        catch (Exception e)
        {
          lastError = e.Message;
        }

        drawer.Draw(game);

        Console.CursorLeft = 0;
        Console.CursorTop = game.PegSize + 2;

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(lastError);
        lastError = "";
        Console.ForegroundColor = ConsoleColor.White;
      }

      if (game.IsGameOver())
      {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("The Tower has been SOLVED!!!");
      }
      else
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("The Tower has NOT been solved. :(");
      }

      Console.ForegroundColor = ConsoleColor.White;
      Console.ReadLine();
    }
  }
}
