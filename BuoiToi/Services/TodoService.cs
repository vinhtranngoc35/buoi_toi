using BuoiToi.Data;
using BuoiToi.Models;
using BuoiToi.Models.Enums;

namespace BuoiToi.Services
{
    public class TodoService
    {
        private readonly SetupDatabase _db;
        public TodoService(SetupDatabase db) {
            _db = db;
        }

        private static List<Todo> todos = new List<Todo>()
        {
            new Todo {Id = 1, Title = "Study C#", Description = "For, While, ...", CategoryId = 1, StartTime = new DateTime(2023, 10, 28, 18, 0, 0), EndTime = new DateTime(2023, 10, 28, 20, 0, 0)},
            new Todo {Id = 2, Title = "Study SQL", Description = "Index, Store Procedure", CategoryId = 1, StartTime = new DateTime(2023, 10, 28, 20, 0, 0), EndTime = new DateTime(2023, 10, 28, 21, 0, 0)}
        };

        private static readonly List<Category> categories = new List<Category>() 
        {
            new Category {Id = 1, Name = "Study"},
            new Category {Id = 2, Name = "Work"}
        };
        private static int CurrentId = 2;

        public List<Category> GetCategories()
        {
            return categories;
        }

        public List<Todo> FindAll()
        {
             return (from t in todos
                    join c in categories on t.CategoryId equals c.Id
                    orderby t.StartTime
                    select new Todo
                    {
                        Id = t.Id, 
                        Title = t.Title,
                        Description = t.Description,
                        CategoryId = t.CategoryId, 
                        StartTime = t.StartTime, 
                        EndTime = t.EndTime,
                        Link = t.Link,
                        Status = t.Status,
                        Category = c
                    }).ToList();
        }

        public Todo? FindById(int id) => todos.FirstOrDefault(t => t.Id == id);

        public void AddTodo(Todo todo)
        {
            todo.Id = ++CurrentId;
            todos.Add(todo);
        }



        public void EditTodo(int id, Todo todo)
        {
            foreach (var item in todos)
            {
                if(item.Id == id)
                {
                    item.Title = todo.Title;
                    item.Description = todo.Description;
                    item.CategoryId = todo.CategoryId;
                    item.Status = todo.Status;
                    item.StartTime = todo.StartTime;
                    item.EndTime = todo.EndTime;
                    item.Link = todo.Link;
                }
            }
        }

        public void DeleteTodo(int id)
        {
            todos = todos.Where(e => e.Id != id).ToList();
        }
        public void UpdateStatus(int id)
        {
            foreach(var item in todos)
            {
                if (item.Id == id)
                {
                    //[TODO, INPROGRESS, PENDING, COMPLETE]
                    // TODO
                    var todoStatuses = Enum.GetValues(typeof(TodoStatus));
                    
                   for (int i = 0; i < todoStatuses.Length - 1; i++) 
                    {
                        if (todoStatuses.GetValue(i)?.ToString() == item.Status.ToString())
                        {
                            item.Status = (TodoStatus)todoStatuses.GetValue(i + 1);
                            return;
                        }
                    }
                   item.Status = (TodoStatus)todoStatuses.GetValue(0);
                }
            }
        }
    }
}
