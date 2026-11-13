using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace KooliProjekt.Application.Infrastructure.Files
{
 public class LocalFileClient : IFileClient
 {
  private readonly string _filesRoot;
  public LocalFileClient(string filesRoot) { _filesRoot = Path.GetFullPath(filesRoot); }
  private string Resolve(string store, string name = null)
  {
   if (string.IsNullOrWhiteSpace(store) || Path.IsPathRooted(store) || store.Split('/', '\\').Any(s => s == ".." || s == ".")) throw new ArgumentException("Invalid file store.");
   if (name != null && !FileRules.ValidName(name)) throw new ArgumentException("Invalid file name.");
   var result = Path.GetFullPath(Path.Combine(_filesRoot, store, name ?? ""));
   if (!result.StartsWith(_filesRoot.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)) throw new ArgumentException("Invalid path.");
   // Do not follow junctions or symbolic links outside the configured root.
   for (var dir = new DirectoryInfo(Path.GetDirectoryName(result)); dir != null; dir = dir.Parent)
   {
    if (dir.Exists && (dir.Attributes & FileAttributes.ReparsePoint) != 0) throw new IOException("Linked directories are not supported.");
   }
   if ((File.Exists(result) || Directory.Exists(result)) && (File.GetAttributes(result) & FileAttributes.ReparsePoint) != 0) throw new IOException("Linked files are not supported.");
   return result;
  }
  public async Task Save(string fileName, string fileStore, Stream inputStream, CancellationToken ct = default)
  {
   var target = Resolve(fileStore, fileName);
   Directory.CreateDirectory(Path.GetDirectoryName(target));
   // CreateNew prevents accidentally overwriting another upload.
   using var output = new FileStream(target, FileMode.CreateNew, FileAccess.Write, FileShare.None, 81920, true);
   try { await inputStream.CopyToAsync(output, ct); }
   catch { output.Close(); File.Delete(target); throw; }
  }
  public Task<bool> Delete(string fileName, string fileStore, CancellationToken ct = default)
  {
   ct.ThrowIfCancellationRequested(); var target = Resolve(fileStore, fileName);
   if (!File.Exists(target)) return Task.FromResult(false);
   File.Delete(target); return Task.FromResult(true);
  }
  public async Task<byte[]> Get(string fileName, string fileStore, CancellationToken ct = default)
  {
   var target = Resolve(fileStore, fileName);
   try { return await File.ReadAllBytesAsync(target, ct); }
   catch (FileNotFoundException) { return null; }
   catch (DirectoryNotFoundException) { return null; }
  }
  public Task<IReadOnlyList<string>> List(string fileStore, CancellationToken ct = default)
  {
   ct.ThrowIfCancellationRequested(); var folder = Resolve(fileStore);
   IReadOnlyList<string> files = Directory.Exists(folder) ? Directory.GetFiles(folder).Where(p => (File.GetAttributes(p) & FileAttributes.ReparsePoint) == 0).Select(Path.GetFileName).OrderBy(n => n, StringComparer.Ordinal).ToArray() : Array.Empty<string>();
   return Task.FromResult(files);
  }
 }
}

