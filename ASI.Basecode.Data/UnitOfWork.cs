using System;
using ASI.Basecode.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Data
{
    /// <summary>
    /// Unit of Work Implementation
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        /// <summary>
        /// Gets the database context
        /// </summary>
        public DbContext Database { get; private set; }

        /// <summary>
        /// Initializes a new instance of the UnitOfWork class.
        /// </summary>
        /// <param name="serviceContext">The service context.</param>
        public UnitOfWork(AsiBasecodeDBContext serviceContext)
        {
            Database = serviceContext;
        }

        /// <summary>
        /// Saves the changes to database with concurrency handling
        /// </summary>
        public void SaveChanges()
        {
            using (var transaction = Database.Database.BeginTransaction())
            {
                bool saveFailed;
                do
                {
                    saveFailed = false;
                    try
                    {
                        Database.SaveChanges();
                        transaction.Commit();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        saveFailed = true;
                        HandleConcurrencyConflict(ex);
                    }
                    catch (Exception) // Changed catch to catch general exceptions
                    {
                        transaction.Rollback();
                        throw;
                    }
                } while (saveFailed);
            }
        }

        /// <summary>
        /// Handles concurrency conflicts by reloading the conflicting entities.
        /// </summary>
        /// <param name="ex">The concurrency exception.</param>
        private void HandleConcurrencyConflict(DbUpdateConcurrencyException ex)
        {
            foreach (var entry in ex.Entries)
            {
                if (entry.Entity is Expense expense)
                {
                    // Reload the values from the database
                    var databaseEntry = Database.Set<Expense>().AsNoTracking().SingleOrDefault(e => e.ExpenseID == expense.ExpenseID);
                    if (databaseEntry == null)
                    {
                        throw new NotSupportedException("The entity has been deleted by another user.");
                    }
                    else
                    {
                        // Replace the values of the entity with the database values
                        entry.OriginalValues.SetValues(databaseEntry);
                    }
                }
                else if (entry.Entity is User)
                {
                    entry.Reload(); // Handle User as well
                }
                else
                {
                    throw new NotSupportedException("Concurrency conflict for type " + entry.Metadata.Name);
                }
            }
        }

        /// <summary>
        /// Disposes the database context
        /// </summary>
        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
