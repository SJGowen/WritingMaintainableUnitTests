using System;
using Microsoft.AspNetCore.Mvc;
using WritingMaintainableUnitTests.Module4_DecouplingPatterns.Expenses;

namespace WritingMaintainableUnitTests.Module5_AssertionsAndObservations
{
    public class ExpenseSheetController : Controller
    {
        private readonly ICommandHandler<CreateExpenseSheet> _commandHandler;

        public ExpenseSheetController(ICommandHandler<CreateExpenseSheet> commandHandler)
        {
            _commandHandler = commandHandler;
        }
        
        [HttpPost]
        public IActionResult Create(CreateExpenseSheetFormModel formModel)
        {
            var command = new CreateExpenseSheet(Guid.NewGuid(), formModel.EmployeeId, formModel.SubmissionDate);
            var result = _commandHandler.Handle(command);

            if(! result.IsSuccessful)
                return BadRequest();

            return Ok();
        }       
    }

    public class CreateExpenseSheetFormModel
    {
        public Guid EmployeeId { get; set; }
        public DateTime SubmissionDate { get; set; }
    }
}