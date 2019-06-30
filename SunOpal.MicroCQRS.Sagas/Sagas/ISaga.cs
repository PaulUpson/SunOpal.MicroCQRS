using System;
using System.Collections.Generic;

namespace SunOpal.MicroCQRS.Sagas {
  public interface ISaga : HasCompleted {
    ISagaEntity GetEntity(Guid id);
    ICommandSender Bus { get; set; }
  }

  public interface ISaga<T> : ISaga where T : ISagaEntity
  {
    IDictionary<Guid,T> Data { get; set; }
  }
}