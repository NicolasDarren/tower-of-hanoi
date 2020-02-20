using System;
using System.Collections.Generic;

namespace TowerOfHanoi
{
  class Program
  {
    static void Main(string[] args)
    {
      var game = new TowerGame();

      var lastError = "";
      while (true)
      {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        DrawPegOutline(peg1LeftOffset, 1);
        DrawPegOutline(peg2LeftOffset, 2);
        DrawPegOutline(peg3LeftOffset, 3);

        DrawPegDiscs(peg1LeftOffset, game.Peg1);
        DrawPegDiscs(peg2LeftOffset, game.Peg2);
        DrawPegDiscs(peg3LeftOffset, game.Peg3);

        Console.CursorTop = 12;
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

    private const int pegMaxWidth = 21;
    private const int peg1LeftOffset = 0;
    private const int peg2LeftOffset = 25;
    private const int peg3LeftOffset = 50;
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

    private static void DrawPegDiscs(int leftOffset, IReadonlyPeg peg)
    {
      var discNumber = 0;
      foreach (var disc in peg.AllDiscs())
      {
        DrawDisc(leftOffset, discNumber, disc.Size, discColors[disc.Size]);
        discNumber++;
      }
    }

    private static void DrawDisc(int pegLeftOffset, int discNumber, int size, ConsoleColor color)
    {
      Console.ForegroundColor = color;
      Console.CursorTop = 8 - discNumber;
      Console.CursorLeft = pegLeftOffset;
      var output = "|";
      for (var i = 0; i < size - 1; i++)
      {
        output = $"+{output}+";
      }
      output = $"[{output}]";
      while (output.Length < pegMaxWidth)
      {
        output = $" {output} ";
      }
      Console.Write(output);
    }

    private static void DrawPegOutline(int leftOffset, int pegNumber)
    {
      var topStart = 0;
      Console.ForegroundColor = ConsoleColor.Cyan;
      ConsoleWrite(topStart++, leftOffset, "          |          ");
      ConsoleWrite(topStart++, leftOffset, "          |          ");
      ConsoleWrite(topStart++, leftOffset, "          |          ");
      ConsoleWrite(topStart++, leftOffset, "          |          ");
      ConsoleWrite(topStart++, leftOffset, "          |          ");
      ConsoleWrite(topStart++, leftOffset, "          |          ");
      ConsoleWrite(topStart++, leftOffset, "          |          ");
      ConsoleWrite(topStart++, leftOffset, "          |          ");
      ConsoleWrite(topStart++, leftOffset, "          |          ");
      ConsoleWrite(topStart++, leftOffset, "=====================");
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
