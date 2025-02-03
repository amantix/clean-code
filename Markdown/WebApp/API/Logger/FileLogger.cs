namespace API.Logger;

public class FileLogger : ILogger, IDisposable
{
    private readonly string _filePath;
    private static readonly object Lock = new object();

    public FileLogger(string filePath) => _filePath = filePath;

    public IDisposable BeginScope<TState>(TState state) => this;

    public void Dispose(){}

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId,
        TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        lock (Lock)
        {
            File.AppendAllText(_filePath, formatter(state, exception) + Environment.NewLine);
        }
    }
}