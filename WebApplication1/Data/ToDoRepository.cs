using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace ToDoApp.Data
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly AppDbContext _context;
        public ToDoRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ToDo> Add(ToDo toDoObject)
        {
            _context.TODOs.Add(toDoObject);
           await  _context.SaveChangesAsync();

            return toDoObject;
        }

        public async Task Delete(int id)
        {
            var toDoToDelete = await _context.TODOs.FindAsync(id);
            _context.TODOs.Remove(toDoToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task MarkAsDone(int id)
        {
            var toDoToObject = await _context.TODOs.FindAsync(id);
            toDoToObject.IsDone = true;
            await _context.SaveChangesAsync();         
        }

        public async Task<IEnumerable<ToDo>> Get()
        {
            return await _context.TODOs.ToListAsync();
        }

        public async Task<IEnumerable<ToDo>> Get(bool doneStatus)
        {
            return await _context.TODOs.Where(t => t.IsDone == doneStatus).ToListAsync();
        }

        public async Task<ToDo> Get(int id)
        {
            return await  _context.TODOs.FindAsync(id);
        }

        public async Task<IEnumerable<ToDo>> Search(string taskText)
        {
            return await _context.TODOs.Where(t => t.Description.Contains(taskText)).ToListAsync();
        }

        public async Task UpdateDescription(ToDo toDoObject, string description)
        {

            toDoObject.Description = description;
            await _context.SaveChangesAsync();
            
        }

        public async Task<IEnumerable<ToDo>> Paging(int pageNumber)
        {
            return await _context.TODOs.Skip((pageNumber - 1) * 25 ).Take(25).ToListAsync();
        }
    }
}
