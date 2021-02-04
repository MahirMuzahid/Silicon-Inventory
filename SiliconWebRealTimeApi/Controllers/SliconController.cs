using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace SiliconWebRealTimeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SliconController : Controller
    {
        private readonly IHubContext<SiliconHub> _hubContext;
        public SliconController(IHubContext<SiliconHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost("refreshCall")]
        public async Task<IActionResult> refreshCall(int refresh)
        {
            await _hubContext.Clients.All.SendAsync("refreshCall", refresh);

            return Ok("All Ok");
        }
    }
}
