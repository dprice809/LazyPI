﻿namespace LazyPI.Common
{
    /// <summary>
    /// Values to indicate which summary type calculation(s) should be performed.
    /// </summary>
    public enum SummaryType
    {
        None,
        Total,
        Average,
        Minimum,
        Maximum,
        Range,
        StdDev,
        PopulationStdDev,
        Count,
        PercentGood,
        All,
        AllForNonNumeric
    }

    /// <summary>
    /// Defines the timestamp returned for a value when a summary calculation is done.
    /// </summary>
    public enum TimeType
    {
        Auto,
        EarliestTime,
        MostRecentTime
    }

    /// <summary>
    /// Defines the evaluation of an expression over a time range.
    /// </summary>
    public enum SampleType
    {
        ExpressionRecordedValues,
        Interval
    }

    /// <summary>
    /// Defines the possible calculation options when performing summary calculations over time-series data.
    /// </summary>
    public enum CalculationBasis
    {
        TimeWeighted,
        EventWeighted,
        TimeWeightedContinuous,
        TimeWeightedDiscrete,
        EventWeightedExcludeMostRecentEvent,
        EventWeightedExcludeEarliestEvent,
        EventWeightedIncludeBothEnds
    }

    /// <summary>
    /// The possible values for the type of an element
    /// </summary>
    public enum ElementType
    {
        None,
        Other,
        Node,
        Measurement,
        Flow,
        Transfer,
        Boundry,
        PIPoint,
        Any
    }

    public enum SearchMode
    {
        None,
        StartInclusive,
        EndInclusive,
        Inclusive,
        Overlapped,
        InProgress,
        BackwardFromStartTime,
        ForwardFromStartTime,
        BackwardFromEndTime,
        ForwardFromEndTime,
        ForwardInProgress
    }

    public enum BoundryType
    {
        Inside,
        Outside,
        Interpolated
    }

    public enum RetreivalMode
    {
        Auto,
        AtOrBefore,
        Before,
        AtOrAfter,
        After,
        Exact
    }

    public enum UpdateOption
    {
        Replace,
        Insert,
        NoReplace,
        ReplaceOnly,
        InsertNoCompression,
        Remove
    }

    public enum BufferOption
    {
        DoNotBuffer,
        BufferIfPossible,
        Buffer
    }
}