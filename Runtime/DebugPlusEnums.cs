
namespace NaxtorGames.Debugging
{
    public enum LogLevelType
    {
        Info,
        Warning,
        Error
    }

    public enum MissingTagBehaviorType
    {
        /// <summary> Dose not print any hint or message of missing tags.</summary>
        Ignore,
        /// <summary> Informs about the missing tag at 'Info'-log level. Dose not show the message.</summary>
        Info,
        /// <summary> Informs about the missing tag at 'Warning'-log level. Dose not show the message.</summary>
        Warning,
        /// <summary> Informs about the missing tag at 'Error'-log level. Dose not show the message.</summary>
        Error,
        /// <summary> Informs about the missing tag. Also prints the message. Log level is the original message log level.</summary>
        Print,
        /// <summary> Adds the missing tag to the 'Custom Tag'-list.</summary>
        Add
    }

    public enum LogType
    {
        None,
        OnlyWithoutTag,
        OnlyWithTag,
        All
    }

    public enum LogResult
    {
        Log,
        DoNotLog,
        Missing
    }
}