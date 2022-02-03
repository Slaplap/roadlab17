using System.Collections.Generic;
using Interon.Roadlab.Core.Models;

namespace Interon.Roadlab.Core.Dto
{
    public class NotificationsDto
    {
        public List<string> Ids { get; set; } = new List<string>();
        public List<NotificationJsonDto> NotificationJsonDtos { get; set; } = new List<NotificationJsonDto>();
    }
}