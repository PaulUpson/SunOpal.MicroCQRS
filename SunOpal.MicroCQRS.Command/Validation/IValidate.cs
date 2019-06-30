namespace SunOpal.MicroCQRS
{
  /// <summary>
  /// Handling of a single command validation.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public interface IValidate<in T> where T : Command {
    ValidationResult Validate(T cmd);
  }
}