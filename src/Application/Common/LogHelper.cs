using Discord;
using Microsoft.Extensions.Logging;

namespace Application.Common;

public static class LogHelper
{
    public static Task OnLogAsync(ILogger logger, LogMessage msg)
    {
        switch (msg.Severity)
        {
            case LogSeverity.Verbose:
            case LogSeverity.Info:
                logger.LogInformation("{Message}", msg.ToString());
                break;
            case LogSeverity.Warning:
                logger.LogWarning("{Message}", msg.ToString());
                break;
            case LogSeverity.Error:
                logger.LogError("{Message}", msg.ToString());
                break;
            case LogSeverity.Critical:
                logger.LogCritical("{Message}", msg.ToString());
                break;
            case LogSeverity.Debug:
                logger.LogDebug("{Message}", msg.ToString());
                break; 
            default:
                throw new NotSupportedException($"LogSeverity {msg.Severity.ToString()} is not supported.");
        }
        
        return Task.CompletedTask;
    }
}