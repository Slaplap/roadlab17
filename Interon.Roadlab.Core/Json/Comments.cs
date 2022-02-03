using System;
using System.Collections.Generic;

namespace Interon.Roadlab.Core.Json
{
    public class Comments
    {
        public List<Comment> CommentsList { get; set; } = new List<Comment>();
    }

    public class Comment
    {
        public DateTime DateTime { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
    }
}
