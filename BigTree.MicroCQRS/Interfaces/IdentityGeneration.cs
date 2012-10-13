using System;

namespace BigTree.MicroCQRS 
{
  public interface IIdentityGenerator<T>
  {
    T GetId();
    Type GetGenericType();
    void Reseed(T seed);
  }
}