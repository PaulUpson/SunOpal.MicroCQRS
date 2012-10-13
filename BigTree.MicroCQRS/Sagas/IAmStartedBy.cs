namespace BigTree.MicroCQRS.Sagas {

  /// <summary>
  /// use this interface to signify that when a message if the given type
  /// is received, if a saga connot be found by an <see cref="IFindSagas{T}"/>
  /// the saga will be created.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public interface IAmStartedBy<T> : Handles<T> where T : Event { }
}