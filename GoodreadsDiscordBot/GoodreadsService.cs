using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodreadsDiscordBot
{
    public class GoodreadsService
    {
        public async Task<Book> ParsePage(string url)
        {
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = await web.LoadFromWebAsync(url);
            var converter = new ReverseMarkdown.Converter();

            var img = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='BookCover__image']/div/img");

            return new Book()
            {
                Title = htmlDoc.DocumentNode.SelectSingleNode("//h1[@data-testid='bookTitle']").InnerText,
                Description = converter.Convert(htmlDoc.DocumentNode.SelectSingleNode("//div[@class='BookPageMetadataSection__description']").InnerHtml),
                ImageUrl = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='BookCover__image']/div/img").Attributes["src"]?.Value,
            };
        }
    }
}
