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
        RandomClassic,
        Jailbird,
        ParticleDisruptor,
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
