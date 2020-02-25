using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

      var lastError = "";
      drawer.Draw(game);

      foreach (var move in solver.SolveFromStart(game.AsReadonly()))
      {
        Thread.Sleep(solver.MillisecondDelayBetweenMoves);

        try
        {
          game.PerformMove(move.From, move.To);
        }
        catch (Exception e)
        {
          lastError = e.Message;
        }

        drawer.Draw(game);

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
