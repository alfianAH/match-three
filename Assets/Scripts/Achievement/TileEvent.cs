public abstract class TileEvent
{
    // Abstract class for base event from tile

    /// <summary>
    /// What happen when tile match
    /// </summary>
    public abstract void OnMatch();

    /// <summary>
    /// Check if event requirements has been achieved
    /// </summary>
    /// <returns></returns>
    public abstract bool AchievementCompleted();
}
