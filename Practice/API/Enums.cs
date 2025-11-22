namespace Practice
{
    public enum DuelRoom
    {
        HczCheckpointA,
        HczCheckpointB,
        LczCheckpointA,
        LczCheckpointB,
        GateA,
        GateB,
        Glassroom,
        Chamber049,
    }

    public enum DuelType
    {
        Classic,
        Jailbird,
    }

    public enum DuelMode
    {
        OneVsOne,
        TwoVsTwo,
    }

    public enum DuelState
    {
        Waiting,
        CountingDown,
        InProgress,
        Finished
    }
}
