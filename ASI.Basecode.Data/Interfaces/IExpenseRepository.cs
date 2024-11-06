using ASI.Basecode.Data.Models;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Interfaces
{
    public interface IExpenseRepository
    {
        IEnumerable<Expense> ViewExpenses();
        void AddExpense(Expense expense);
        void UpdateExpense(Expense expense);
        void DeleteExpense(Expense expense);

        // Method to retrieve categories
        IEnumerable<Category> GetCategories();
    }
}
