using System;

namespace dotnetcoreproject.Application.Interfaces;

public interface IErrorHandlingService
{
    void LogError(Exception ex, string additionalInfo = null);
}