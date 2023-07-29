using Microsoft.AspNetCore.Mvc;
using MotorsportAPI.Services;
using MotorsportAPI.Business;

namespace MotorsportAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet("imsa")]
        public async Task<IEnumerable<RaceEvent>> GetImsaSchedule()
        {
            var schedule = await _scheduleService.GetSchedule(Series.IMSA);
            return schedule;
        }

        [HttpGet("f1")]
        public async Task<IEnumerable<RaceEvent>> GetF1Schedule()
        {
            var schedule = await _scheduleService.GetSchedule(Series.F1);
            return schedule;
        }

        [HttpGet("indy")]
        public async Task<IEnumerable<RaceEvent>> GetIndySchedule()
        {
            var schedule = await _scheduleService.GetSchedule(Series.INDYCAR);
            return schedule;
        }
    }
}
