using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SunOpal.MicroCQRS
{
  /// <inheritdoc />
  public class CommandValidator : ICommandValidator
  {
    private readonly IDictionary<Type, Func<object, ValidationResult>> _validationHandlers 
      = new Dictionary<Type, Func<object, ValidationResult>>();

    public void RegisterValidationHandler<T>(Func<T,ValidationResult> handler) where T : Command
    {
      _validationHandlers.Add(typeof(T), o => handler((T) o));
    }

    public void WireUpValidate(object o)
    {
      var infos = o.GetType()
        .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
        .Where(m => m.Name == "Validate")
        .Where(m => m.GetParameters().Length == 1);

      foreach (var methodInfo in infos)
      {
        var type = methodInfo.GetParameters().First().ParameterType;
        var info = methodInfo;
        _validationHandlers.Add(type, command => (ValidationResult)info.Invoke(o, new []{ command }));
      }
    }

    public ValidationResult Validate<T>(T command) where T : Command
    {
      if (!_validationHandlers.TryGetValue(command.GetType(), out Func<object, ValidationResult> handler))
        return new ValidationResult();
      try
      {
        return handler(command);
      }
      catch(TargetInvocationException ex)
      {
        throw ex.InnerException;
      }
    }
  }
}