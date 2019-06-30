using System.Collections.Generic;

namespace SunOpal.MicroCQRS
{
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