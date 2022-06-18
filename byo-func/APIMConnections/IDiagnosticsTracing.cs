
namespace MSHA.ApiConnections
{
    /// <summary>
    /// Common interface for diagnostics tracing (e.g. to MDS/tracing).
    /// </summary>
    public interface IDiagnosticsTracing
    {
        void Verbose(string message);

        void Informational(string message);

        void Warning(string message);

        void Error(string message);

        void Critical(string message);
    }
}
