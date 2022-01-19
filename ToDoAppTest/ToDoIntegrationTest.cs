using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using ToDoApp;
using ToDoApp.Data;
using ToDoApp.Models;
using Xunit;

namespace ToDoAppTest
{
    public class TODOIntegrationTest
    {
        protected readonly HttpClient TestClient;

        public TODOIntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
             .WithWebHostBuilder(builder =>
             {
                 builder.ConfigureServices(services =>
                 {
                     services.RemoveAll(typeof(AppDbContext));
                     services.AddDbContext<AppDbContext>(options => { options.UseSqlServer("TestDb"); });
                 });
             });

            TestClient = appFactory.CreateClient();
        }

        [Fact]
        public async Task Test_Search()
        {
            //ToDos which contains the word "Integration"
            var response = await TestClient.GetAsync("api/ToDo/Search?description=Integration");
            var jsonString = await response.Content.ReadAsStringAsync();
            var toDos = JsonConvert.DeserializeObject<List<ToDo>>(jsonString);
            var initialCount = toDos.Count;

            var toDo = new ToDo()
            {
                Description = "Integration test ",
                IsDone = false
            };

            //Add a new ToDo where Description contains the word "Integration"
            await TestClient.PostAsync("api/ToDo/Add", new StringContent(JsonConvert.SerializeObject(toDo), Encoding.UTF8, "application/json"));

            //Check if the number of ToDos which contains the word "Integration" increased by one
            response = await TestClient.GetAsync("api/ToDo/Search?description=Integration");
            jsonString = await response.Content.ReadAsStringAsync();
            toDos = JsonConvert.DeserializeObject<List<ToDo>>(jsonString);

            Assert.Equal(initialCount + 1, toDos.Count);
        }


        [Fact]
        public async Task Test_MarkAsDone()
        {

            var toDo = new ToDo()
            {
                Description = "MarkAsDoneTesting",
                IsDone = false
            };

            //Adding a new ToDo"
            var addResponse = await TestClient.PostAsync("api/ToDo/Add", new StringContent(JsonConvert.SerializeObject(toDo), Encoding.UTF8, "application/json"));
            var jsonString = await addResponse.Content.ReadAsStringAsync();
            var addedToDo = JsonConvert.DeserializeObject<ToDo>(jsonString);
            Assert.False(addedToDo.IsDone);

            //Call MarkAsDone
            await TestClient.GetAsync($"api/ToDo/MarkAsDone/{addedToDo.Id}");

            var response1 = await TestClient.GetAsync("api/ToDo/Get");
            var sas = await response1.Content.ReadAsStringAsync();
            var toDos = JsonConvert.DeserializeObject<List<ToDo>>(sas);

            //Check if its Done
            var response = await TestClient.GetAsync($"api/ToDo/GetById/{addedToDo.Id}");
            jsonString = await response.Content.ReadAsStringAsync();
            addedToDo = JsonConvert.DeserializeObject<ToDo>(jsonString);

            Assert.True(addedToDo.IsDone);
        }


    }
}
