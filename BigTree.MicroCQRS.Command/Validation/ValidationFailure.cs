namespace BigTree.MicroCQRS
{
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
}