using System;

namespace WritingMaintainableUnitTests.Module4_DecouplingPatterns.Expenses
{
    public interface IApproverRepository
    {
        ICanApproveExpenses Get(Guid id);
    }
    
    public class ApproverRepository : IApproverRepository
    {
        private readonly IObjectRelationalMapper _objectRelationalMapper;

        public ApproverRepository(IObjectRelationalMapper objectRelationalMapper)
        {
            _objectRelationalMapper = objectRelationalMapper;
        }
        
        public ICanApproveExpenses Get(Guid id)
        {
            return _objectRelationalMapper.Get<ICanApproveExpenses>(id);
        }
    }

    public interface IExpenseSheetRepository
    {
        ExpenseSheet Get(Guid id);
        void Save(ExpenseSheet expenseSheet);
    }

    public class ExpenseSheetRepository : IExpenseSheetRepository
    {
        private readonly IObjectRelationalMapper _objectRelationalMapper;

        public ExpenseSheetRepository(IObjectRelationalMapper objectRelationalMapper)
        {
            _objectRelationalMapper = objectRelationalMapper;
        }
        
        public ExpenseSheet Get(Guid id)
        {
            return _objectRelationalMapper.Get<ExpenseSheet>(id);
        }

        public void Save(ExpenseSheet expenseSheet)
        {
            var current = Get(expenseSheet.Id);
            var currentVersion = current?.Version ?? 0;
            
            if(currentVersion != expenseSheet.Version)
                throw new OptimisticConcurrencyException<ExpenseSheet>(expenseSheet.Id, currentVersion, expenseSheet.Version);

            expenseSheet.Version += 1;
            _objectRelationalMapper.Save(expenseSheet.Id, expenseSheet);
        }
    }
    
    public interface IEmployeeRepository
    {
        Employee Get(Guid id);
    }
    
    public class OptimisticConcurrencyException<T> : Exception
    {
        public OptimisticConcurrencyException(Guid entityId, int expectedVersion, int actualVersion)
            : base($"Expected version '{expectedVersion}' of entity '{nameof(T)}' with ID '{entityId}', " +
                   $"but received version '{actualVersion}'.")
        {}
    }

    public interface IObjectRelationalMapper
    {
        TEntity Get<TEntity>(object id);
        void Save<TEntity>(object id, TEntity entity);
    }
}