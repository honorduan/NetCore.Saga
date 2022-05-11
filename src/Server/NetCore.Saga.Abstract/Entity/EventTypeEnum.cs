namespace NetCore.Saga.Abstract.Entity
{
    /// <summary>
    /// EventTypeEnum
    /// </summary>
    public enum EventTypeEnum
    {
        EventStart,
        EventEnd,
        Aborted,
        Compensated,
    }
}
