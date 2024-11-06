using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ASI.Basecode.Data.Repositories
{
    public class ExpenseRepository : BaseRepository, IExpenseRepository
    {
        private readonly AsiBasecodeDBContext _dbContext;

        public ExpenseRepository(AsiBasecodeDBContext dbContext, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Expense> ViewExpenses()
        {
            // Retrieves all expenses from the database
            return this.GetDbSet<Expense>().ToList();
        }

        public void AddExpense(Expense expense)
        {
            // Adds a new expense to the database
            this.GetDbSet<Expense>().Add(expense);
            UnitOfWork.SaveChanges();
        }

        public void UpdateExpense(Expense expense)
        {
            // Updates an existing expense in the database
            this.GetDbSet<Expense>().Update(expense);
            UnitOfWork.SaveChanges();
        }

        public void DeleteExpense(Expense expense)
        {
            // Removes the specified expense from the database
            this.GetDbSet<Expense>().Remove(expense);
            UnitOfWork.SaveChanges();
        }

        // Implementation of GetCategories to retrieve all categories
        public IEnumerable<Category> GetCategories()
        {
            return _dbContext.Set<Category>().ToList();
        }
    }
}
