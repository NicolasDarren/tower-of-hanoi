using System.Dynamic;
using TowerOfHanoi.Common;

namespace TowerOfHanoi
{
  public class TowerGameMoveLogModel
  {
    public TowerGameMoveModel Move { get; set; } 
    public bool Valid { get; set; }
    public string Error { get; set; }
    public int Peg1CountBefore { get; set; }
    public int Peg1CountAfter { get; set; }
    public int Peg2CountBefore { get; set; }
    public int Peg2CountAfter { get; set; }
    public int Peg3CountBefore { get; set; }
    public int Peg3CountAfter { get; set; }
  }
}