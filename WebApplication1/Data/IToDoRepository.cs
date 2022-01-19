using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace ToDoApp.Data
{
    public interface IToDoRepository 
    {
        Task<ToDo> Add(ToDo toDoObject);
        Task UpdateDescription(ToDo toDoObject, string newDescription);
        Task MarkAsDone(int id);
        Task<IEnumerable<ToDo>> Get();
        /// <summary>
        /// Get by IsDone value
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<IEnumerable<ToDo>> Get(bool status);
        Task<ToDo> Get(int id);
        Task Delete(int id);
        Task<IEnumerable<ToDo>> Search(string text);
        Task<IEnumerable<ToDo>> Paging(int pageNumber);

    }
}
