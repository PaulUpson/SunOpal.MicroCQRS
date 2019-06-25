# BigTree.MicroCQRS

_A lightweight CQRS framework_

## Concepts

### Basics
- issue commands
  - validate 
  - mutate state of domain model
  - raise events
- query for data
  - materialized views
  - built from event stream
### Advanced
 - sagas for long running operations
 - event converters for upgrading events from vPrevious to vNext
 - 

### Document Strategy

The mechanism used for fetching documents as well as serialising data into the document entities.

```c#
public interface IDocumentStrategy
  {
    string GetEntityBucket<TEntity>();
    string GetEntityLocation<TEntity>(object key);

    void Serialize<TEntity>(TEntity entity, Stream stream);
    TEntity Deserialize<TEntity>(Stream stream);
  }
```
 



