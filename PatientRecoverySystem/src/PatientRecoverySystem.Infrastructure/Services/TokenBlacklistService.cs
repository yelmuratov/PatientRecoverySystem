using System.Collections.Concurrent;
using PatientRecoverySystem.Application.Interfaces;

namespace PatientRecoverySystem.Infrastructure.Services
{

    public class TokenBlacklistService : ITokenBlacklistService
    {
        private readonly ConcurrentDictionary<string, bool> _blacklistedTokens = new();

        public void BlacklistToken(string token)
        {
            _blacklistedTokens.TryAdd(token, true);
        }

        public bool IsTokenBlacklisted(string token)
        {
            return _blacklistedTokens.ContainsKey(token);
        }
    }
}
