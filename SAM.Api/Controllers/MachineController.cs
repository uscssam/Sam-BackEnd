using Microsoft.AspNetCore.Mvc;
using SAM.Entities;
using SAM.Service;
using SAM.Services.Interfaces;

namespace SAM.Api.Controllers
{
    public class MachineController : BaseController<Machine>
    {
        public MachineController(IService<Machine> service) : base(service) { }

        [HttpGet("unit/{unitId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ListByUnit(int unitId)
        {
            var machines = ((MachineService)service).ListByUnit(unitId);
            if (machines != null && machines.Count > 0)
                return Ok(machines);
            else return NotFound(null);
        }
    }
}
