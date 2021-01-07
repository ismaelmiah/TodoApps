using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Todo.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace API.Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        
        
        [HttpGet]
        public IEnumerable<TodoItemModel> FilteredByDate([FromQuery] DateTime datetime)
        {
            var model = new TodoItemModel();
            var data =  model.GetByDate(datetime);
            return data.OrderBy(x=>x.Title);
        }

        [HttpGet("all")]
        public async Task<IEnumerable<TodoItemModel>> Get()
        {
            var model = new TodoItemModel();
            var data = await model.GetAll();
            return data;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemModel>> Get(int id)
        {
            var model = new TodoItemModel();
            var todoItem = await model.Get(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }
        
        [HttpPost]
        public ActionResult<TodoItemModel> Post(TodoItemModel item)
        {
            var data = item.Add(item);
            return CreatedAtAction(nameof(Get), new {id = data.Id}, data);
        }
        
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, JsonPatchDocument<TodoItemModel> patchDoc)
        {
            var model = new TodoItemModel();
            patchDoc.ApplyTo(model, ModelState);
            var todoItem = await model.Get(id);
            if (todoItem == null)
            {
                return NotFound();
            }
            
            try
            {
                await model.PatchConfigure(id, model);
            }
            catch (DbUpdateConcurrencyException) when (!model.Exits(id))
            {
                return NotFound();
            }

            return NoContent();
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, TodoItemModel item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            var todoItem = await item.Get(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            try
            {
                await item.Update(id, item);
            }
            catch (DbUpdateConcurrencyException) when (!item.Exits(id))
            {
                return NotFound();
            }

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var model = new TodoItemModel();
            var todoItem = await model.Get(id);

            if (todoItem == null)
            {
                return NotFound();
            }
            await model.Remove(id);
            return NoContent();
        }
    }
}
