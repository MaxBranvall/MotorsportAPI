using MotorsportAPI.Business;
using HtmlAgilityPack;
using System.Linq;

namespace MotorsportAPI.Services
{
    public class ScheduleService : IScheduleService
    {

        private readonly HttpClient _httpClient;

        public ScheduleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<RaceEvent>> GetSchedule(Series series)
        {
            IEnumerable<RaceEvent> schedule;
            HtmlDocument htmlDoc;

            switch(series)
            {
                case Series.IMSA:
                    htmlDoc = await GetHtmlDocumentAsync("https://www.imsa.com/weathertech/weathertech-2023-schedule/");

                    schedule = htmlDoc.DocumentNode
                        .SelectNodes("//div[@class='schedule-item']")
                        .Select(s => new RaceEvent(s.SelectSingleNode(".//h2[@class='schedule-title']").InnerHtml,
                        s.SelectSingleNode(".//div[@class='schedule-location']").InnerHtml,
                        s.SelectSingleNode(".//div[@class='schedule-date']").InnerHtml));
                    break;

                // this isnt done yet
                case Series.F1:
                    htmlDoc = await GetHtmlDocumentAsync("https://www.formula1.com/en/racing/2023.html");

                    var t = htmlDoc.DocumentNode
                        .SelectNodes("//div[@class='event-list']/div[@class='col-12']")
                        .Select(s => s.SelectSingleNode(".//a[@class='event-item-link']"));

                    schedule = htmlDoc.DocumentNode
                        .SelectNodes("//div[@class='schedule-item']")
                        .Select(s => new RaceEvent(s.SelectSingleNode(".//h2[@class='schedule-title']").InnerHtml,
                        s.SelectSingleNode(".//div[@class='schedule-location']").InnerHtml,
                        s.SelectSingleNode(".//div[@class='schedule-date']").InnerHtml));
                    break;

                case Series.INDYCAR:
                    htmlDoc = await GetHtmlDocumentAsync("https://www.indycar.com/Schedule");

                    schedule = htmlDoc.DocumentNode
                        .SelectNodes("//li[@class='schedule-list__item']")
                        .Where(s => s.SelectSingleNode(".//div[@class='schedule-list__date']") is not null)
                        .Select(s => new RaceEvent(s.SelectSingleNode(".//div[@class='schedule-list__content']").ChildNodes.ElementAt(1).InnerText.Trim(),
                            s.SelectSingleNode(".//img[@class='race-image']").ChildAttributes("alt").First().Value,
                            s.SelectSingleNode(".//div[@class='schedule-list__date']").ChildNodes.ElementAt(0).InnerText.Trim() + " " +
                            s.SelectSingleNode(".//div[@class='schedule-list__date']").ChildNodes.ElementAt(1).InnerText.Trim()))
                        .Where(r => r.EventName.Length > 0);
                    break;

                default:
                    schedule = new List<RaceEvent>();
                    break;
            }

            return schedule;
        }

        private async Task<HtmlDocument> GetHtmlDocumentAsync(string url)
        {
            var content = await _httpClient.GetAsync(url);
            var html = await content.Content.ReadAsStringAsync();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            return htmlDoc;
        }
    }
}
