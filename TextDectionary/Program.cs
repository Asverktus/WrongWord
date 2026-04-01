using System;
using System.IO;

namespace TextCorrector
{
  public enum MenuCommand
  {
    Exit = 0,
    CorrectDirectory = 1,
    CorrectSingleFile = 2
  }

  class Program
  {
    private static DictionaryLoader _loader;
    private static TextCorrector _corrector;

    static void Main()
    {
      Console.WriteLine("=== TEXT CORRECTOR ===\n");

      try
      {
        _loader = new DictionaryLoader();
        _corrector = new TextCorrector(_loader);
      }

      catch (Exception ex)
      {
        Console.WriteLine(
          $"Error initializing: {ex.Message}" +
          "\nPress any key to exit..."
          );
        Console.ReadKey();

        return;
      }

      while (true)
      {
        Console.WriteLine(
          "\n=== MENU ===\n" +
          "1. Correct all files in directory (spelling + phones)\n" +
          "2. Correct single file (spelling + phones)\n" +
          "0. Exit"
        );

        Console.Write("Choose: ");
        string input;
        input = Console.ReadLine();

        int choice;

        bool parseResult;
        parseResult = int.TryParse(input, out choice);

        if (!parseResult)
        {
          Console.WriteLine("Invalid input");

          continue;
        }

        if (choice == (int)MenuCommand.Exit)
        {
          Console.WriteLine("Goodbye!");

          return;
        }

        try
        {
          switch ((MenuCommand)choice)
          {
            case MenuCommand.CorrectDirectory:
              Console.Write("Enter directory path: ");
              string dirPath;
              dirPath = Console.ReadLine();

              if (string.IsNullOrWhiteSpace(dirPath))
              {
                dirPath = Directory.GetCurrentDirectory();
                Console.WriteLine($"Using current directory: {dirPath}");
              }

              _corrector.CorrectDirectory(dirPath);

              break;

            case MenuCommand.CorrectSingleFile:
              Console.Write("Enter file path: ");
              string filePath;
              filePath = Console.ReadLine();

              _corrector.CorrectFile(filePath);

              break;

            default:
              Console.WriteLine("Invalid choice");

              break;
          }
        }

        catch (Exception ex)
        {
          Console.WriteLine($"Error: {ex.Message}");
        }
      }
    }
  }
}