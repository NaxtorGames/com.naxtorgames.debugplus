
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
        Ignore,
        Info,
        Warning,
        Error,
        Print,
        Add
    }

    public enum LogType
    {
        None,
        Explicit,
        All
    }

    public enum LogResult
    {
        Log,
        DoNotLog,
        Missing
    }
}