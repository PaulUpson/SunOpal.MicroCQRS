using System;
using System.Collections.Generic;
using System.Threading;

namespace SunOpal.MicroCQRS {
  /// <summary>
  /// Simple example in memory bus, treating Commands and Events mostly the same
  /// </summary>
  public class SimpleBus : ICommandSender, IEventPublisher
  {
    private readonly Dictionary<Type, List<Action<IMessage>>> _routes = new Dictionary<Type, List<Action<IMessage>>>();

    public void RegisterHandler<T>(Action<T> handler) where T : IMessage
    {
      List<Action<IMessage>> handlers;
      if (!_routes.TryGetValue(typeof(T), out handlers))
      {
        handlers = new List<Action<IMessage>>();
        _routes.Add(typeof(T), handlers);
      }
      handlers.Add(DelegateAdjuster.CastArgument<IMessage, T>(x => handler(x)));
    }

    public void Send<T>(T command) where T : Command
    {
      List<Action<IMessage>> handlers;
      if (_routes.TryGetValue(typeof(T), out handlers))
      {
        if (handlers.Count != 1) throw new InvalidOperationException("cannot send to more than one handler");
        handlers[0](command);
      }
      else
      {
        throw new InvalidOperationException("no handler registered");
      }
    }

    public void Publish<T>(T @event) where T : Event
    {
      List<Action<IMessage>> handlers;
      if (!_routes.TryGetValue(@event.GetType(), out handlers)) return;
      foreach (var handler in handlers)
      {
        //dispatch on thread pool for added awesomeness
        var handler1 = handler;
        ThreadPool.QueueUserWorkItem(x => handler1(@event));
      }
    }

    public void PublishSync<T>(T @event) where T : Event
    {
      List<Action<IMessage>> handlers;
      if (!_routes.TryGetValue(@event.GetType(), out handlers)) return;
      foreach (var handler in handlers)
      {
        handler(@event);
      }
    }
  }
}