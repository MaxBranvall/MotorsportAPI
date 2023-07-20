using MotorsportAPI.Business;
using HtmlAgilityPack;

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

            var content = await _httpClient.GetAsync("https://www.imsa.com/weathertech/weathertech-2023-schedule/");
            var htmlContent = await content.Content.ReadAsStringAsync();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlContent);

            switch(series)
            {
                case Series.IMSA:
                    schedule = htmlDoc.DocumentNode
                        .SelectNodes("//div[@class='schedule-item']")
                        .Select(s => new RaceEvent(s.SelectSingleNode(".//h2[@class='schedule-title']").InnerHtml,
                        s.SelectSingleNode(".//div[@class='schedule-location']").InnerHtml,
                        s.SelectSingleNode(".//div[@class='schedule-date']").InnerHtml));
                    break;

                // this isnt done yet
                case Series.F1:
                    schedule = htmlDoc.DocumentNode
                        .SelectNodes("//div[@class='schedule-item']")
                        .Select(s => new RaceEvent(s.SelectSingleNode(".//h2[@class='schedule-title']").InnerHtml,
                        s.SelectSingleNode(".//div[@class='schedule-location']").InnerHtml,
                        s.SelectSingleNode(".//div[@class='schedule-date']").InnerHtml));
                    break;

                default:
                    schedule = new List<RaceEvent>();
                    break;
            }

            return schedule;
        }
    }
}
