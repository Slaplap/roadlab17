using System;
using System.Threading;
using System.Threading.Tasks;

namespace Interon.Roadlab.Web.Net.Core.Services
{
    public class TokenBucketRateLimiter
    {
        private readonly int _capacity;
        private readonly int _refillRate;
        private readonly object _lock = new object();
        private int _tokens;
        private DateTime _lastRefill;

        public TokenBucketRateLimiter(int capacity, int refillRate)
        {
            _capacity = capacity;
            _refillRate = refillRate;
            _tokens = capacity;
            _lastRefill = DateTime.UtcNow;
        }

        public async Task<bool> TryConsumeAsync(int tokensRequired = 1)
        {
            lock (_lock)
            {
                RefillTokens();
                if (_tokens >= tokensRequired)
                {
                    _tokens -= tokensRequired;
                    return true;
                }
                return false;
            }
        }

        private void RefillTokens()
        {
            var now = DateTime.UtcNow;
            var timePassed = now - _lastRefill;
            var tokensToAdd = (int)(timePassed.TotalSeconds * _refillRate);
            
            if (tokensToAdd > 0)
            {
                _tokens = Math.Min(_capacity, _tokens + tokensToAdd);
                _lastRefill = now;
            }
        }
    }
}