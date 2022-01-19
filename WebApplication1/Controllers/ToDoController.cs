using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.Data;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoRepository _repository;
        public ToDoController(IToDoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<ActionResult<IEnumerable<ToDo>>> Get()
        {
            return  Ok(await _repository.Get());
        }

        [HttpGet]
        [Route("Paging")]
        public async Task<ActionResult<IEnumerable<ToDo>>> Paging(int pageNumber)
        {
            return Ok(await _repository.Paging(pageNumber));
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<ActionResult<IEnumerable<ToDo>>> GetById([FromRoute] int id)
        {
            var result = await _repository.Get(id);

            if(result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByDoneStatus")]
        public async Task<ActionResult<IEnumerable<ToDo>>> GetByDoneStatus([FromQuery] bool status)
        {
            return Ok(await _repository.Get(status));
        }

        [HttpGet]
        [Route("Search")]
        public async Task<ActionResult<IEnumerable<ToDo>>> Search([FromQuery] string description)
        {
            return Ok(await _repository.Search(description));
        }

        [HttpGet]
        [Route("MarkAsDone/{id}")]
        public async Task<ActionResult> MarkAsDone([FromRoute] int id)
        {
            var toDoObject = await _repository.Get(id);
            if (toDoObject == null)
            {
                return NotFound();
            }
            await _repository.MarkAsDone(id);
            return Ok();
        }


        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult<ToDo>> Add([FromBody] ToDo toDoObject)
        {
            var createdToDo = await _repository.Add(toDoObject);
            return Created($"api/TODO/Get/{createdToDo.Id}", createdToDo);
        }

        [HttpPut]
        [Route("UpdateDescription/{id}")]
        public async Task<ActionResult> UpdateDescription([FromRoute] int id, [FromBody] string description)
        {
            var toDoObject = await _repository.Get(id);
            if (toDoObject == null)
            {
                return NotFound();
            }
            await _repository.UpdateDescription(toDoObject,description);
            return Ok();

        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var toDoObject = await _repository.Get(id);
            if (toDoObject == null)
            {
                return NotFound();
            }
            await _repository.Delete(id);
            return NoContent();
        }
    }
}
