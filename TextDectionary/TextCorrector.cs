using System;
using System.IO;

namespace TextCorrector
{
  public class TextCorrector
  {
    private DictionaryLoader _loader;

    public TextCorrector(DictionaryLoader loader)
    {
      _loader = loader;
    }

    public void CorrectFile(string filePath)
    {
      if (!File.Exists(filePath))
      {
        Console.WriteLine($"File not found: {filePath}");

        return;
      }

      string originalContent;
      originalContent = File.ReadAllText(filePath, System.Text.Encoding.UTF8);

      string correctedContent;
      correctedContent = _loader.CorrectText(originalContent);

      if (originalContent != correctedContent)
      {
        File.WriteAllText(filePath, correctedContent, System.Text.Encoding.UTF8);
        Console.WriteLine($"Fixed: {Path.GetFileName(filePath)}");
      }
    }

    public void CorrectDirectory(string directoryPath, bool recursive = true)
    {
      if (!Directory.Exists(directoryPath))
      {
        Console.WriteLine($"Directory not found: {directoryPath}");

        return;
      }

      string[] files;
      files = Directory.GetFiles(directoryPath, "*.txt", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

      Console.WriteLine($"Found {files.Length} .txt files");

      foreach (string file in files)
      {
        CorrectFile(file);
      }
    }
  }
}