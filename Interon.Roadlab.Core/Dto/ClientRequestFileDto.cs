using System;

namespace Interon.Roadlab.Core.Dto
{
    public class ClientRequestFileDto
    {
        public Guid Key { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Url { get; set; }
        public string Mime { get; set; }
        public int Size { get; set; }
        public Guid ClientRequestKey { get; set; }
    }
}