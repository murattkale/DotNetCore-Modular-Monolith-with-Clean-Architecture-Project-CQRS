using System;
using dotnetcoreproject.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace dotnetcoreproject.Application.Services;

public class ErrorHandlingService(ILogger<ErrorHandlingService> logger) : IErrorHandlingService
{
    public void LogError(Exception ex, string additionalInfo = null)
    {
        if (additionalInfo != null)
            logger.LogError(ex, additionalInfo);
        else
            logger.LogError(ex, "An error occurred");
    }
}