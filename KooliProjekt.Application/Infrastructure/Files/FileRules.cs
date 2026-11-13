using System;
using System.IO;
using System.Linq;
namespace KooliProjekt.Application.Infrastructure.Files
{
 public static class FileRules
 {
  public const int MaxBytes = 5 * 1024 * 1024;
  public static bool ValidName(string name) => !string.IsNullOrWhiteSpace(name) && name.Length <= 150 && name != "." && name != ".." && name == Path.GetFileName(name) && name.IndexOfAny(Path.GetInvalidFileNameChars()) < 0 && !name.Contains('/') && !name.Contains('\\') && !name.EndsWith('.') && !name.EndsWith(' ') && !new[]{"CON","PRN","AUX","NUL","COM1","COM2","COM3","COM4","COM5","COM6","COM7","COM8","COM9","LPT1","LPT2","LPT3","LPT4","LPT5","LPT6","LPT7","LPT8","LPT9"}.Contains(name.Split('.')[0], StringComparer.OrdinalIgnoreCase);
  public static bool IsImage(string name, byte[] bytes)
  {
   if (bytes == null || bytes.Length < 12 || name == null) return false;
   var ext = Path.GetExtension(name).ToLowerInvariant();
   return ext == ".png" && bytes.Take(8).SequenceEqual(new byte[]{137,80,78,71,13,10,26,10}) ||
    (ext == ".jpg" || ext == ".jpeg") && bytes[0] == 255 && bytes[1] == 216 && bytes[2] == 255 ||
    ext == ".webp" && System.Text.Encoding.ASCII.GetString(bytes,0,4) == "RIFF" && System.Text.Encoding.ASCII.GetString(bytes,8,4) == "WEBP";
  }
  public static string ContentType(string name) => Path.GetExtension(name).ToLowerInvariant() switch { ".png" => "image/png", ".webp" => "image/webp", _ => "image/jpeg" };
 }
}
