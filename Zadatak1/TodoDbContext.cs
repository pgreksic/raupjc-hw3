using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zadatak1.Models;

namespace Zadatak1
{
    public class TodoDbContext : DbContext
    {

        public TodoDbContext(string cnnstr) : base(cnnstr)
        {

        }


        public IDbSet<TodoItem> TodoItems { get; set; }
        public IDbSet<TodoItemLabel> TodoItemLabels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /// TodoItem 
            modelBuilder.Entity<TodoItem>().HasKey(item => item.Id);
            modelBuilder.Entity<TodoItem>().Property(item => item.Text).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(item => item.IsCompleted).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(item => item.DateCompleted).IsOptional();
            modelBuilder.Entity<TodoItem>().Property(item => item.DateCreated).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(item => item.UserId).IsRequired();
            modelBuilder.Entity<TodoItem>().HasMany(item => item.Labels).WithMany(labels => labels.LabelTodoItems);
            modelBuilder.Entity<TodoItem>().Property(item => item.DateDue).IsOptional();


            /// TodoItemLabel
            modelBuilder.Entity<TodoItemLabel>().HasKey(label => label.Id);
            modelBuilder.Entity<TodoItemLabel>().Property(label => label.Value).IsRequired();
            modelBuilder.Entity<TodoItemLabel>().HasMany(label => label.LabelTodoItems).WithMany(item => item.Labels);

        }
    }
}
