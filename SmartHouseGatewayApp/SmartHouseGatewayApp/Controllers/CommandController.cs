using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartHouseCoreAbstraction.Commands;
using SmartHouseGatewayApp.Dto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartHouseGatewayApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly ICommandInvoker _commandInvoker;

        public CommandController(ICommandInvoker commandInvoker)
        {
            _commandInvoker = commandInvoker;
        }

        [HttpPost]
        public async Task Post(CommandDto commandDto)
        {
            await _commandInvoker.InvokeAsync(commandDto.CommandName, commandDto.DeviceName);
        }
    }
}
