using System;

namespace Interon.Roadlab.App.Core.Models
{
    public class SecureStorageModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
