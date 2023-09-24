using Microsoft.AspNetCore.Mvc;
using SAM.Repositories.Interfaces;

namespace SAM.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseController<T> : Controller
        where T : class
    {
        private readonly IRepositoryDatabase<T> repository;

        public BaseController(IRepositoryDatabase<T> repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public T Get(int id)
        {
            return repository.Read(id);
        }

    }
}
