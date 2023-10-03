using System.Collections.Generic;
using System.Linq;

namespace WritingMaintainableUnitTests.Module3AnatomyOfUnitTests.ArrangeActAssertPerTestMethodOrClass
{
    public class Result
    {
        public bool IsSuccessful { get; private set; }
        public IEnumerable<string> DomainViolations { get; }

        public Result(IEnumerable<string> domainViolations)
        {
            DomainViolations = domainViolations;
            IsSuccessful = !domainViolations.Any();
        }

        public static Result Success()
        {
            return new Result(Enumerable.Empty<string>());
        }

        public static Result Failure(params string[] domainViolations)
        {
            return new Result(domainViolations);
        }
    }
}