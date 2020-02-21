using System;
using System.Collections.Generic;

namespace TowerOfHanoi
{
  class Program
  {
    static void Main(string[] args)
    {
      var game = new TowerGame(3);
      var drawer = new ConsoleDrawer();

      var lastError = "";
      while (true)
      {
        drawer.Draw(game);

        Console.CursorTop = game.PegSize + 3;
        Console.CursorLeft = 0;

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(lastError);
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Enter your move (Source Peg, Destination Peg):");
        var input = Console.ReadLine();
        if (input == "") break;
        var splitInput = input.Split(",");
        var sourcePeg = int.Parse(splitInput[0]);
        var destPeg = int.Parse(splitInput[1]);
        try
        {
          lastError = "";
          game.PerformMove(sourcePeg, destPeg);
        }
        catch (Exception e)
        {
          lastError = e.Message;
        }
      }
    }
  }
}
