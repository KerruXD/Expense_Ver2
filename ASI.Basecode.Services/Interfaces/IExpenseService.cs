using ASI.Basecode.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASI.Basecode.Data.Repositories;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IExpenseService
    {
        List<Expense> GetExpenses();

        void AddExpense(Expense expense);

        void UpdateExpense(Expense expense);

        void DeleteExpense(Expense expense);
    }
}
