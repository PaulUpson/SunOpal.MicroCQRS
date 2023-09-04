using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace SunOpal.DocumentStore;

public class FileDocumentReaderWriter<TKey, TEntity> : IDocumentReader<TKey, TEntity>, IDocumentWriter<TKey, TEntity>
{
  private readonly string _folder;
  private readonly IDocumentStrategy _strategy;
  private readonly TextWriter _log;

  public FileDocumentReaderWriter(string directoryPath, IDocumentStrategy strategy, TextWriter log)
  {
    _folder = Path.Combine(directoryPath, strategy.GetEntityBucket<TEntity>());//.Replace('/', Path.DirectorySeparatorChar);
    _strategy = strategy;
    _log = log;
  }

  public void InitIfNeeded()
  {
    Directory.CreateDirectory(_folder);
  }

  public bool TryGet(TKey key, out TEntity view)
  {
    view = default;
    var name = GetName(key);

    try
    {
      if(!File.Exists(name))
        return false;

      using var stream = File.Open(name, FileMode.Open, FileAccess.Read, FileShare.Read);
      if (stream.Length == 0)
        return false;
      view = _strategy.Deserialize<TEntity>(stream);
      return true;
    }
    catch (FileNotFoundException)
    {
      return false;
    }
    catch(DirectoryNotFoundException)
    {
      return false;
    }
    catch(IOException) // fix for fast read / slow write concurrency
    {
      _log.WriteLine($"File read failed for {_folder} - {name} with key {key}. Will retry.");
      Thread.Sleep(2);
      TryGet(key, out view);
      return false;
    }
  }

  string GetName(TKey key) => Path.Combine(_folder, _strategy.GetEntityLocation<TEntity>(key));

  public TEntity AddOrUpdate(TKey key, Func<TEntity> addFactory, Func<TEntity, TEntity> update, AddOrUpdateHint hint)
  {
    var name = GetName(key);

    try
    {
      var subfolder = Path.GetDirectoryName(name);
      if (subfolder != null && !Directory.Exists(subfolder))
        Directory.CreateDirectory(subfolder);

      // we are locking the file.
      using var file = File.Open(name, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
      byte[] initial = Array.Empty<byte>();
      TEntity result;
      if (file.Length == 0)
      {
        result = addFactory();
      }
      else
      {
        using var mem = new MemoryStream();
        file.CopyTo(mem);
        mem.Seek(0, SeekOrigin.Begin);
        var entity = _strategy.Deserialize<TEntity>(mem);
        initial = mem.ToArray();
        result = update(entity);
      }

      using (var mem = new MemoryStream())
      {
        _strategy.Serialize(result, mem);
        var data = mem.ToArray();

        if (!data.SequenceEqual(initial))
        {
          // upload only if we changed
          file.Seek(0, SeekOrigin.Begin);
          file.Write(data, 0, data.Length);
          // truncate this file
          file.SetLength(data.Length);
        }
      }

      return result;
    }
    catch (FileNotFoundException)
    {
      throw new InvalidOperationException($"File '{name}' does not exist.");
    }
    catch (DirectoryNotFoundException)
    {
      throw new InvalidOperationException($"Container '{_folder}' does not exist.");
    }
    catch (IOException) // fix for fast read / slow write concurrency
    {
      _log.WriteLine($"File write failed for {_folder} - {name} with key {key}. Will retry.");
      Thread.Sleep(120);
      return AddOrUpdate(key, addFactory, update, hint);
    }
  }

  public bool TryDelete(TKey key)
  {
    var name = GetName(key);
    if(File.Exists(name))
    {
      File.Delete(name);
      return true;
    }
    return false;
  }
}