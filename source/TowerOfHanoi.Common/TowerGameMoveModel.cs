namespace TowerOfHanoi.Common
{
  public class TowerGameMoveModel
  {
    public PegNumber From { get; set; }
    public PegNumber To { get; set; }

    public static TowerGameMoveModel CreateMove(PegNumber from, PegNumber to)
    {
      return new TowerGameMoveModel
      {
        From = from,
        To = to
      };
    }
  }
}