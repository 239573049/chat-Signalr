using Chat.Web.Code.Gadget;
using Chat.Web.Code.Job;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Web.Controllers
{
    /// <summary>
    /// 定时器调试
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly ILogger<JobsController> _logger;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IHubContext<ChatHub> chatHub;
        private IScheduler _scheduler;
        public JobsController(ILogger<JobsController> logger, ISchedulerFactory schedulerFactory, IHubContext<ChatHub> chatHub)
        {
            _logger = logger;
            this.chatHub=chatHub;
            _schedulerFactory = schedulerFactory;
        }
        /// <summary>
        /// 启动系统信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Demo()
        {
            _scheduler = await _schedulerFactory.GetScheduler();
            Jobs.HubContext = chatHub;
            await _scheduler.Start();
            var trigger = TriggerBuilder.Create()
                            .WithSimpleSchedule(x => x.WithIntervalInSeconds(2).RepeatForever())
                            .Build();
            var jobDetail = JobBuilder.Create<Jobs>()
                            .WithIdentity("Myjob", "SystemData")
                            .Build();
            await _scheduler.ScheduleJob(jobDetail, trigger);
            return Ok();
        }

    }
}
