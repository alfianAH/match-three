namespace Achievement.TileEvents
{
    public class CakeTileEvent : TileEvent
    {
        private int matchCount,
            requiredAmount;

        public CakeTileEvent(int amount)
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
}
