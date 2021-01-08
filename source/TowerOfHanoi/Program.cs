using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using TowerOfHanoi.Common;

namespace TowerOfHanoi
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.Write("Enter the number of discs per peg (2 - 18): ");
      var pegSize = int.Parse(Console.ReadLine() ?? "3");

      Console.WriteLine("Choose the solver to run: ");
      var solvers = FindAllSolvers().ToArray();
      var solverNumber = 0;
      foreach (var solverType in solvers)
      {
        solverNumber++;
        Console.WriteLine($"{solverNumber}: {solverType.FullName}");
      }

      solverNumber = int.Parse(Console.ReadLine() ?? "1");

      var game = new TowerGame(pegSize);
      var drawer = new ConsoleDrawer();
      var solver = (ISolveTowers)Activator.CreateInstance(solvers[solverNumber - 1]);

      drawer.Draw(game);

      var moves = RunTowerThroughTheSolver(solver, game, drawer);

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

      var numberOfInvalidMoves = moves.Count(m => !m.Valid);
      Console.WriteLine($"Number of moves: {moves.Count}. Invalid Moves: {numberOfInvalidMoves}.");
      Console.WriteLine("Print the move history? (Y/N)");
      var printAnswer = Console.ReadLine()?.ToUpper() ?? "";
      if (printAnswer == "Y")
      {
        PrintMoveLogToConsole(moves);
      }
    }

    private static List<TowerGameMoveLogModel> RunTowerThroughTheSolver(
      ISolveTowers solver, 
      ITowerGame game,
      IDrawTheTowerGame drawer)
    {
      var moves = new List<TowerGameMoveLogModel>();

      foreach (var move in solver.SolveFromStart(game.AsReadonly()))
      {
        var thisMove = new TowerGameMoveLogModel
        {
          Move = move,
          Valid = true,
          Peg1CountBefore = game.Peg1.CurrentNumberOfDiscs,
          Peg2CountBefore = game.Peg2.CurrentNumberOfDiscs,
          Peg3CountBefore = game.Peg3.CurrentNumberOfDiscs
        };

        Thread.Sleep(solver.MillisecondDelayBetweenMoves);
        try
        {
          game.PerformMove(move.From, move.To);
        }
        catch (Exception e)
        {
          thisMove.Valid = false;
          thisMove.Error = e.Message;
        }

        thisMove.Peg1CountAfter = game.Peg1.CurrentNumberOfDiscs;
        thisMove.Peg2CountAfter = game.Peg2.CurrentNumberOfDiscs;
        thisMove.Peg3CountAfter = game.Peg3.CurrentNumberOfDiscs;

        moves.Add(thisMove);

        drawer.Draw(game);

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(thisMove.Error);
        Console.ForegroundColor = ConsoleColor.White;
      }

      return moves;
    }

    private static void PrintMoveLogToConsole(List<TowerGameMoveLogModel> moves)
    {
      Console.Clear();
      Console.ForegroundColor = ConsoleColor.White;
      for (var moveIndex = 0; moveIndex < moves.Count; moveIndex++)
      {
        var moveNumber = moveIndex + 1;
        var log = moves[moveIndex];
        Console.ForegroundColor = log.Valid ? ConsoleColor.White : ConsoleColor.Red;
        var line = $"{moveNumber:000}: ";
        line += $"[{log.Peg1CountBefore}|{log.Peg2CountBefore}|{log.Peg3CountBefore}]";
        line += $" {log.Move.From.ToString().PadRight(5)} -> {log.Move.To.ToString().PadRight(5)} ";
        line += $"[{log.Peg1CountAfter}|{log.Peg2CountAfter}|{log.Peg3CountAfter}]";
        line += $"  {log.Error}";
        Console.WriteLine(line);

        if ((moveNumber + 1) % Console.WindowHeight == 0)
        {
          Console.ForegroundColor = ConsoleColor.White;
          Console.Write("Press ENTER for next page");
          Console.ReadLine();
        }
      }

      Console.ForegroundColor = ConsoleColor.White;
    }

    private static IEnumerable<Type> FindAllSolvers()
    {
      var scanFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      foreach (var dllFilePath in Directory.GetFiles(scanFolder, "*.dll"))
      {
        Assembly assembly = null;
        try
        {
          assembly = Assembly.LoadFrom(dllFilePath);
        }
        catch
        {
          //skip it
        }

        if (assembly != null)
        {
          foreach (var solver in assembly
            .DefinedTypes
            .Where(t => t
              .ImplementedInterfaces
              .Contains(typeof(ISolveTowers))))
          {
            yield return solver;
          }
        }
      }
    }
  }
}
