namespace SunOpal.MicroCQRS
{
  /// <summary>
  /// A global command validator
  /// </summary>
  public interface ICommandValidator {
    ValidationResult Validate<T>(T command) where T : Command;
  }
}