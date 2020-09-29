public class LollipopTileEvent : TileEvent
{
    private int matchCount,
        requiredAmount;

    public LollipopTileEvent(int amount)
    {
        requiredAmount = amount;
    }

    public override void OnMatch()
    {
        matchCount++;
    }

    public override bool AchievementCompleted()
    {
        return matchCount == requiredAmount;
    }
}
