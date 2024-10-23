using dotnetcoreproject.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Repositories;

public class SessionService : ISessionService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SessionService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? GetLangId()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session == null) return null;
        var langId = session.GetInt32("LangId");
        return session.GetInt32("IsLang") == 0 ? 0 : langId;
    }
}