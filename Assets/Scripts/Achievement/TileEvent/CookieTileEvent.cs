public class CookieTileEvent : TileEvent
{
    private int matchCount,
        requiredAmount;

    public CookieTileEvent(int amount)
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
