using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace KooliProjekt.Application.Infrastructure.Files
{
 public interface IFileClient
 {
  Task Save(string fileName, string fileStore, Stream inputStream, CancellationToken ct = default);
  Task<bool> Delete(string fileName, string fileStore, CancellationToken ct = default);
  Task<byte[]> Get(string fileName, string fileStore, CancellationToken ct = default);
  Task<IReadOnlyList<string>> List(string fileStore, CancellationToken ct = default);
 }
}
