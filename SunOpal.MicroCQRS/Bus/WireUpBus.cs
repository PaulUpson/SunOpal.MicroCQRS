using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace SunOpal.MicroCQRS {
  public class WireUpBus : ICommandSender, IEventPublisher
  {
    private readonly IDictionary<Type, List<Wire>> _eventHandlers = new Dictionary<Type, List<Wire>>();
    private readonly IDictionary<Type, Action<object>> _commandHandlers = new Dictionary<Type, Action<object>>();
    private readonly ILog _log;

    public sealed class Wire
    {
      public MethodInfo Method;
      public object Instance;
    }

    public WireUpBus(ILog log)
    {
      _log = log;
    }

    public void RegisterHandler<T>(Action<T> handler) where T : IMessage
    {
      if (typeof(Command).IsAssignableFrom(typeof(T)))
      {
        _commandHandlers.Add(typeof(T), o => handler((T)o));
      }
    }

    public void WireToHandle(object o)
    {
      var infos = o.GetType()
        .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
        .Where(m => m.Name == "Handle")
        .Where(m => m.GetParameters().Length == 1);

      foreach (var methodInfo in infos)
      {
        var type = methodInfo.GetParameters().First().ParameterType;

        if (typeof(Event).IsAssignableFrom(type))
        {

          if (!_eventHandlers.TryGetValue(type, out List<Wire> list))
          {
            list = new List<Wire>();
            _eventHandlers.Add(type, list);
          }

          list.Add(new Wire { Instance = o, Method = methodInfo });
        }
        if (typeof(Command).IsAssignableFrom(type))
        {
          var info = methodInfo;
          _commandHandlers.Add(type, command => info.Invoke(o, new[] { command }));
        }
      }
    }

    public void Send<T>(T command) where T : Command
    {
      if (!_commandHandlers.TryGetValue(command.GetType(), out Action<object> handler))
      {
        throw new InvalidOperationException("no handler registered");
      }
      try
      {
        handler(command);
      }
      catch (TargetInvocationException ex)
      {
        throw ex.InnerException;
      }
    }

    public void Publish<T>(T @event) where T : Event {
      if (!_eventHandlers.TryGetValue(@event.GetType(), out List<Wire> info)) return;
      foreach (var wire in info) {
        //dispatch on thread pool for added awesomeness
        var wire1 = wire;
        ThreadPool.QueueUserWorkItem(x => WithLogging(() => wire1.Method.Invoke(wire1.Instance, new[] {(object) @event})));
      }
    }

    public void PublishSync<T>(T @event) where T : Event {
      List<Wire> info;
      if (!_eventHandlers.TryGetValue(@event.GetType(), out info)) return;
      try
      {
        foreach (var wire in info)
        {
          wire.Method.Invoke(wire.Instance, new[] { (object)@event });
        }
      }
      catch (TargetInvocationException ex)
      {
        _log.Exception(ex.InnerException);
        throw new MessageSendingException(string.Format("Error publishing event of type {0} - {1}", @event.GetType().Name, @event), ex.InnerException);
      }
    }

    private void WithLogging(Action action) {
      try {
        action();
      }
      catch (TargetInvocationException ex) {
        _log.Exception(ex.InnerException);
      }
    }
  }
}
