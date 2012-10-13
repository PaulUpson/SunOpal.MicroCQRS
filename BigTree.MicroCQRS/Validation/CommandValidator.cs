using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BigTree.MicroCQRS.Validation {
  public interface IValidationHandler<in T> where T : Command {
    ValidationResult Validate(T cmd);
  }

  public interface ICommandValidator {
    ValidationResult Validate<T>(T command) where T : Command;
  }

  public class CommandValidator : ICommandValidator {
    private readonly IDictionary<Type, Func<object, ValidationResult>> _validationHandlers 
      = new Dictionary<Type, Func<object, ValidationResult>>();

    public void RegisterValidationHandler<T>(Func<T,ValidationResult> handler) where T : Command {
      _validationHandlers.Add(typeof(T), o => handler((T) o));
    }

    public void WireUpValidate(object o) {
      var infos = o.GetType()
        .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
        .Where(m => m.Name == "Validate")
        .Where(m => m.GetParameters().Length == 1);

      foreach (var methodInfo in infos) {
        var type = methodInfo.GetParameters().First().ParameterType;
        var info = methodInfo;
        _validationHandlers.Add(type, command => (ValidationResult)info.Invoke(o, new []{ command }));
      }
    }

    public ValidationResult Validate<T>(T command) where T : Command {
      Func<object, ValidationResult> handler;
      if (!_validationHandlers.TryGetValue(command.GetType(), out handler)) 
        return new ValidationResult();
      try {
        return handler(command);
      }
      catch(TargetInvocationException ex) {
        throw ex.InnerException;
      }
    }
  }

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

  public class ValidationFailure {
    public ValidationFailure(string propertyName, string error)
      : this(propertyName, error, null) {}

    public ValidationFailure(string propertyName, string error, object attemptedValue) {
      PropertyName = propertyName;
      ErrorMessage = error;
      AttemptedValue = attemptedValue;
    }

    public string PropertyName { get; private set; }
    public string ErrorMessage { get; private set; }
    public object AttemptedValue { get; private set; }
    public object CustomState { get; set; }

    public override string ToString() {
      return ErrorMessage;
    }
  }

  /*  public class ValidationHandler {
    private readonly ICommandValidator _validation;
    private readonly Action<object> _next;
    
    public ValidationHandler(ICommandValidator validation, Action<object> next) {
      _validation = validation;
      _next = next;
    }

    public void Handle(Command cmd) {
      if(_validation.Validate(cmd)) {
        _next(cmd);
      }
    }
  }
 
 This is how you might do it in a composition fashion i.e. register<Command>(new ValidationHandler(Validator, commandHandler.Handle))
 */

  public static class ValidationExtensions {
    public static void Add(this IList<ValidationFailure> collection, string propertyName, string message) {
      collection.Add(new ValidationFailure(propertyName, message));
    }
  }
}