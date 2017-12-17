using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zadatak1.Models;

namespace Zadatak1
{
    public class TodoSqlRepository : ITodoRepository
    {

        private readonly TodoDbContext _context;
        public TodoSqlRepository(TodoDbContext context)
        {
            _context = context;
        }

        /// <summary >
        /// Gets TodoItem for a given id. Throw TodoAccessDeniedException
        ///with appropriate message if user is not the owner of the Todo item
        /// </ summary >
        /// <param name =" todoId "> Todo Id </ param >
        /// <param name =" userId ">Id of the user that is trying to fetch the data</ param >
        /// <returns > TodoItem if found , null otherwise </ returns >
        public TodoItem Get(Guid todoId, Guid userId)
        {
            TodoItem todoItem = _context.TodoItems.Include(item => item.Labels).Where(item => item.Id == todoId).FirstOrDefault();

            if (todoItem != null)
            {
                if (todoItem.UserId != userId)
                {
                    throw new TodoAccessDeniedException("User is not the owner of TodoItem");
                }
                return todoItem;
            }
            else
            {
                return null; 
            }
            


        }

        /// <summary >
        /// Adds new TodoItem object in database .
        /// If object with the same id already exists ,
        /// method should throw DuplicateTodoItemException with the message " duplicate id: {id }".
        /// </ summary >
        public void Add(TodoItem todoItem)
        {

            if (_context.TodoItems.Any(item => item.Id == todoItem.Id))
            {
                throw new DuplicateTodoItemException("Duplicate TodoItem with id:" + todoItem.Id.ToString());
            }

            _context.TodoItems.Add(todoItem);
            _context.SaveChanges();
        }


        /// <summary >
        /// Tries to remove a TodoItem with given id from the database . Throw TodoAccessDeniedException with appropriate message if user is not
        ///the owner of the Todo item
        /// </ summary >
        /// <param name =" todoId "> Todo Id </ param >
        /// <param name =" userId ">Id of the user that is trying to remove the data</param >
        /// <returns > True if success , false otherwise </returns >
        public bool Remove(Guid todoId, Guid userId)
        {
            TodoItem todoItem = _context.TodoItems.SingleOrDefault(item => item.Id == todoId);
            if (todoItem != null)
            {
                if (todoItem.UserId != userId)
                {
                    throw new TodoAccessDeniedException("User cannot remove TodoItem beacuse he isn't the owner!");
                }
                else
                {
                    _context.TodoItems.Remove(todoItem);
                    _context.SaveChanges();
                    return true;
                }

            }
            return false;
        }

        /// <summary >
        /// Updates given TodoItem in database .
        /// If TodoItem does not exist , method will add one . Throw
        ///TodoAccessDeniedException with appropriate message if user is not the owner of the Todo item
        /// </ summary >
        /// <param name =" todoItem "> Todo item </ param >
        /// <param name =" userId ">Id of the user that is trying to update the data</ param>
        public void Update(TodoItem todoItem, Guid userId)
        {
            if (todoItem.UserId != userId)
            {
                throw new TodoAccessDeniedException("User cannot update TodoItem beacuse he isn't the owner!");
            }

            if (_context.TodoItems.Any(item => item.Id == todoItem.Id))
            {
                Add(todoItem);
            }
            else
            {
                _context.Entry(todoItem).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

        /// <summary >
        /// Tries to mark a TodoItem as completed in database . Throw
        ///TodoAccessDeniedException with appropriate message if user is not the owner of the Todo item
        /// </ summary >
        /// <param name =" todoId "> Todo Id </ param >
        /// <param name =" userId ">Id of the user that is trying to mark as completed</ param >
        /// <returns > True if success , false otherwise </ returns >
        public bool MarkAsCompleted(Guid todoId, Guid userId)
        {
            TodoItem todoItem = _context.TodoItems.FirstOrDefault(item => item.Id == todoId);
            if (todoItem != null)
            {
                if (todoItem.UserId != userId)
                {
                    throw new TodoAccessDeniedException(
                        "User cannot mark TodoItem as completed beacuse he isn't the owner!");
                }

                bool marked = todoItem.MarkAsCompleted();
                _context.SaveChanges();
                return marked;
            }
            return false;
        }

        /// <summary >
        /// Gets all TodoItem objects in database for user , sorted by date created(descending )
        /// </ summary >
        public List<TodoItem> GetAll(Guid userId)
        {
            return _context.TodoItems.Include(item => item.Labels).Where(item => item.UserId == userId)
                .OrderByDescending(item => item.DateCreated).ToList();
        }

        /// <summary >
        /// Gets all incomplete TodoItem objects in database for user
        /// </ summary >
        public List<TodoItem> GetActive(Guid userId)
        {
            return _context.TodoItems.Include(item => item.Labels).Where(item => item.UserId == userId && !item.IsCompleted)
                .OrderByDescending(item => item.DateCreated).ToList();
        }

        /// <summary >
        /// Gets all completed TodoItem objects in database for user
        /// </ summary >
        public List<TodoItem> GetCompleted(Guid userId)
        {
            return _context.TodoItems.Include(item => item.Labels).Where(item => item.UserId == userId && item.IsCompleted)
                .OrderByDescending(item => item.DateCreated).ToList();
        }

        /// <summary >
        /// Gets all TodoItem objects in database for user that apply to thefilter
        /// </ summary >
        public List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId)
        {
            return _context.TodoItems.Include(item => item.Labels).Where(item => item.UserId == userId).AsEnumerable()
                .Where(filterFunction).ToList();
        }
    }
}
