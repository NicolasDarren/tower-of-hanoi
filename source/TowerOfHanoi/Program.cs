using System;
using System.Collections.Generic;

namespace TowerOfHanoi
{
  class Program
  {
    static void Main(string[] args)
    {
      var game = new TowerGame(9);

      var lastError = "";
      while (true)
      {
        Console.Clear();
        DrawGame(game);

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

    private static readonly Dictionary<int, ConsoleColor> discColors = new Dictionary<int, ConsoleColor>
    {
      { 1, ConsoleColor.Blue },
      { 2, ConsoleColor.Red },
      { 3, ConsoleColor.Yellow },
      { 4, ConsoleColor.Green },
      { 5, ConsoleColor.Magenta },
      { 6, ConsoleColor.Blue },
      { 7, ConsoleColor.Red },
      { 8, ConsoleColor.Yellow },
      { 9, ConsoleColor.Green },
    };


    /*
    private static void CreateConsoleColors(int pegSize)
    {
      var colors = new[]
      {
        ConsoleColor.Blue, ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.Green, ConsoleColor.Magenta
      };
      var index = 0;

      discColors.cl
    }*/

    private static void DrawGame(TowerGame game)
    {
      DrawPegOutline(1, game.PegSize);
      DrawPegOutline(2, game.PegSize);
      DrawPegOutline(3, game.PegSize);

      DrawPegDiscs(1, game.Peg1);
      DrawPegDiscs(2, game.Peg2);
      DrawPegDiscs(3, game.Peg3);
    }

    private static void DrawPegDiscs(int pegNumber, IReadonlyPeg peg)
    {
      var discNumber = 0;
      var leftOffset = ((pegNumber - 1) * peg.MaxDiscs) + 1;
      foreach (var disc in peg.AllDiscs())
      {
        DrawDisc(leftOffset, peg.MaxDiscs, discNumber, disc.Size, discColors[disc.Size]);
        discNumber++;
      }
    }

    private static void DrawDisc(int pegLeftOffset, int pegSize, int discNumber, int size, ConsoleColor color)
    {
      Console.ForegroundColor = color;
      Console.CursorTop = pegSize - 1 - discNumber;
      Console.CursorLeft = pegLeftOffset;
      var output = "|";
      for (var i = 0; i < size - 1; i++)
      {
        output = $"+{output}+";
      }
      output = $"[{output}]";
      while (output.Length < pegSize)
      {
        output = $" {output} ";
      }
      Console.Write(output);
    }

    private static void DrawPegOutline(int pegNumber, int pegSize)
    {
      var topStart = 0;
      var leftOffset = ((pegNumber - 1) * pegSize) + 1;
      Console.ForegroundColor = ConsoleColor.Cyan;
      for (var i = 0; i < pegSize; i++)
      {
        ConsoleWrite(topStart++, leftOffset, "          |          ");
      }
      ConsoleWrite(topStart++, leftOffset, new string('=', pegSize));
      Console.ForegroundColor = ConsoleColor.DarkCyan;
      ConsoleWrite(topStart++, leftOffset, $"        Peg {pegNumber} ");
    }

    private static void ConsoleWrite(int topOffset, int leftOffset, string output)
    {
      Console.CursorTop = topOffset;
      Console.CursorLeft = leftOffset;
      Console.Write(output);
    }
  }
}
