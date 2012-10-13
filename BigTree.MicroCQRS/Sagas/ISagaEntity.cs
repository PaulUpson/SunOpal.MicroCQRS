using System;

namespace BigTree.MicroCQRS.Sagas {
  public interface ISagaEntity
  {
    Guid Id { get; set; }
    int OriginatorId { get; set; }
  }
}