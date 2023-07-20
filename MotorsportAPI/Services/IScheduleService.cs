using MotorsportAPI.Business;

namespace MotorsportAPI.Services
{
    public interface IScheduleService
    {
        Task<IEnumerable<RaceEvent>> GetSchedule(Series series);
    }
}
