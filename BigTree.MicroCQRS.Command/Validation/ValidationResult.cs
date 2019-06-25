using System.Collections.Generic;
using System.Linq;

namespace BigTree.MicroCQRS
{
  public class ValidationResult {
    private readonly List<ValidationFailure> errors = new List<ValidationFailure>();

    public bool IsValid {
      get { return Errors.Count == 0; }
    }

    public IList<ValidationFailure> Errors {
      get { return errors; }
    }

    public ValidationResult() { }

    public ValidationResult(IEnumerable<ValidationFailure> failures) {
      errors.AddRange(failures.Where(failure => failure != null));
    }
  }
}