using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace TodoBackend
{
    public class TodoBuildData
    {
        [Required]
        [MaxLength(30)]
        public string Title { get; set; }
        [Required]
        [MaxLength(140)]
        public string Content { get; set; }

        public TodoBuildData()
        {
        }

        public TodoBuildData(string title, string content)
        {
            Title = title;
            Content = content;
        }
    }

    public class TodoModel
    {
        public string Id;
        public string Title;
        public string Content;
        public bool IsDone;

        public TodoModel(string title, string content)
        {
            Id = Guid.NewGuid().ToString();
            Title = title;
            Content = content;
            IsDone = false;
        }
    }

    public interface IDbConnector
    {
        public IEnumerable<TodoModel> Query(string query);
        public IEnumerable<TodoModel> All();
        public bool Insert(string Title, string Content);
        public bool Delete(string id);
        public int DeleteAllDone();
        public bool finish(string id);

    }

    public class MemDb : IDbConnector
    {
        private List<TodoModel> db;

        public MemDb()
        {
            db = new List<TodoModel>
            {
                new TodoModel("learn asp.net", "3 days project"),
                new TodoModel("get shit done", "right now")
            };
        }

        public IEnumerable<TodoModel> Query(string query)
        {
            return db.Where((x) => x.Id.ToString() == query);
        }

        public bool Insert(string Title, string Content)
        {
            var model = new TodoModel(Title, Content);
            if (db.Where((x) => x.Id == model.Id).Any()){
                return false;
            }
            else { 
                db.Add(model);
                return true;
            }
        }

        public IEnumerable<TodoModel> All()
        {
            return db;
        }

        public bool Delete(string id)
        {                
            return db.RemoveAll((x) => x.Id == id) > 0;
        }

        public int DeleteAllDone()
        {
            return db.RemoveAll((x) => x.IsDone = true);
        }

        public bool finish(string id)
        {
            if (!db.Where((x) => x.Id == id).Any())
            {
                return false;
            }
            else
            {
                db.ForEach(x => { x.IsDone = x.IsDone || x.Id == id;});
                return true;
            }
        }
    }
}
