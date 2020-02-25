using System;
using System.Collections.Generic;
using TowerOfHanoi.Common;

namespace TowerOfHanoi
{
  public interface IDrawTheTowerGame
  {
    void Draw(ITowerGame game);
  }

  public class ConsoleDrawer : IDrawTheTowerGame
  {
    public void Draw(ITowerGame game)
    {
      Console.Clear();

      Console.ForegroundColor = ConsoleColor.White;
      foreach (var headerLine in _header)
      {
        Console.WriteLine(headerLine);
      }

      CreateConsoleColors(game.PegSize);

      DrawPegOutline(1, game.PegSize);
      DrawPegOutline(2, game.PegSize);
      DrawPegOutline(3, game.PegSize);

      DrawPegDiscs(1, game.Peg1);
      DrawPegDiscs(2, game.Peg2);
      DrawPegDiscs(3, game.Peg3);

      Console.CursorTop = _header.Length + game.PegSize;
      Console.CursorLeft = 0;
      Console.WriteLine("");
      Console.WriteLine("");
    }

    private readonly string[] _header = new string[]
    {
      "Tower Of Hanoi",
      "Discovering the Power of Recursion",
      ""
    };

    private static readonly Dictionary<int, ConsoleColor> discColors = new Dictionary<int, ConsoleColor>();

    private void CreateConsoleColors(int pegSize)
    {
      var colors = new[]
      {
        ConsoleColor.Blue, ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.Green, ConsoleColor.Magenta
      };
      var index = 0;

      discColors.Clear();
      for (var i = 1; i <= pegSize; i++)
      {
        discColors.Add(i, colors[index]);
        index++;
        if (index >= colors.Length)
        {
          index = 0;
        }
      }
    }

    private void DrawPegDiscs(int pegNumber, IReadonlyPeg peg)
    {
      var discNumber = 0;
      var leftOffset = CalculateLeftOffset(pegNumber, peg.MaxDiscs);
      foreach (var disc in peg.AllDiscs())
      {
        DrawDisc(leftOffset, peg.MaxDiscs, discNumber, disc.Size, discColors[disc.Size]);
        discNumber++;
      }
    }

    private void DrawDisc(int pegLeftOffset, int pegSize, int discNumber, int size, ConsoleColor color)
    {
      Console.ForegroundColor = color;
      Console.CursorTop = _header.Length + pegSize - 1 - discNumber;
      Console.CursorLeft = pegLeftOffset;
      var output = "|";
      for (var i = 0; i < size - 1; i++)
      {
        output = $"+{output}+";
      }
      output = $"[{output}]";
      output = PadOnBothSides(pegSize*2, ' ', output);
      Console.Write(output);
    }

    private void DrawPegOutline(int pegNumber, int pegSize)
    {
      var topStart = _header.Length;
      var leftOffset = CalculateLeftOffset(pegNumber, pegSize);
      Console.ForegroundColor = ConsoleColor.Cyan;
      for (var i = 0; i < pegSize; i++)
      {
        ConsoleWrite(topStart++, leftOffset, PadOnBothSides(pegSize * 2, ' ', "|"));
      }
      ConsoleWrite(topStart++, leftOffset, PadOnBothSides(pegSize * 2, '=', "="));
      Console.ForegroundColor = ConsoleColor.DarkCyan;
      ConsoleWrite(topStart++, leftOffset, PadOnBothSides(pegSize * 2, ' ', $"Peg {pegNumber}"));
    }

    private static int CalculateLeftOffset(int pegNumber, int pegSize)
    {
      return ((pegNumber - 1) * ((pegSize * 2) + 1)) + pegNumber;
    }

    private static string PadOnBothSides(int padUntilLength, char padChar, string input)
    {
      while (input.Length < padUntilLength)
      {
        input = $"{padChar}{input}{padChar}";
      }

      return input;
    }

    private void ConsoleWrite(int topOffset, int leftOffset, string output)
    {
      Console.CursorTop = topOffset;
      Console.CursorLeft = leftOffset;
      Console.Write(output);
    }
  }
}