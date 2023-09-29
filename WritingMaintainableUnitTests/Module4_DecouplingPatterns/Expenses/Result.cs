using System.Collections.Generic;
using System.Linq;

namespace WritingMaintainableUnitTests.Module4_DecouplingPatterns.Expenses
{
    public class Result
    {
        public bool IsSuccessful { get; private set; }
        public IEnumerable<DomainViolation> DomainViolations { get; }

        public Result(IEnumerable<DomainViolation> domainViolations)
        {
            DomainViolations = domainViolations;
            IsSuccessful = ! domainViolations.Any();
        }
        
        public static Result Success()
        {
            return new Result(Enumerable.Empty<DomainViolation>());
        }

        public static Result Failure(params DomainViolation[] domainViolations)
        {
            return new Result(domainViolations);
        }
    }
}