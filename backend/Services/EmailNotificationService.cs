using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Services
{
    public sealed class EmailNotificationService
    {
        public async Task SendMessageAsync(string recipient, string title, string message,
        CancellationToken cancellationToken = default)
        { }
    }
}