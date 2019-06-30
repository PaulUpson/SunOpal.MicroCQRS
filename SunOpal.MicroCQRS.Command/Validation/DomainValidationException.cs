using System;
using System.Collections.Generic;
using System.Linq;

namespace SunOpal.MicroCQRS
{
  public class DomainValidationException : Exception
  {
    private readonly IList<ValidationFailure> _errors = new List<ValidationFailure>();

    public DomainValidationException() { }

    public DomainValidationException(string paramName, string message)
    {
      AddError(paramName, message);
    }

    public bool HasErrors { get { return _errors.Any(); } }

    public void AddError(string paramName, string message)
    {
      _errors.Add(new ValidationFailure(paramName, message));
    }

    public IEnumerable<ValidationFailure> Errors { get { return _errors; } }
  }
}