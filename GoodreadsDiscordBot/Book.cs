using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodreadsDiscordBot
{
    public record Book
    {
        public string? Title { get; init; }
        public string? Description { get; init; }
        public string? ImageUrl { get; init; }
        public IEnumerable<string>? Authors { get; init; }
    };
}
