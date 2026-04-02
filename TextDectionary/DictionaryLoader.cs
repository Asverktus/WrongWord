using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TextCorrector
{
  public class DictionaryLoader
  {
    private Dictionary<string, string> _corrections;
    private Regex _phoneRegex;

    public DictionaryLoader()
    {
      _corrections = new Dictionary<string, string>();
      _phoneRegex = new Regex(@"\((\d{3})\)\s*(\d{3})-(\d{2})-(\d{2})", RegexOptions.Compiled);
      LoadDefaultDictionary();
    }

    private void LoadDefaultDictionary()
    {
      _corrections.Clear();

      AddCorrection("привет", new string[] { "првиет", "пирвет", "привеит" });
      AddCorrection("спасибо", new string[] { "спасиба", "спосибо" });
      AddCorrection("пожалуйста", new string[] { "пожалуста", "пожайлуста" });
      AddCorrection("прости", new string[] { "пирсти", "присто" });

      Console.WriteLine($"Loaded {_corrections.Count} error corrections from built-in dictionary");
    }

    private void AddCorrection(string correctWord, string[] wrongWords)
    {
      string trimmedWrongWord;

      foreach (string wrongWord in wrongWords)
      {
        trimmedWrongWord = wrongWord.Trim();

        if (!string.IsNullOrEmpty(trimmedWrongWord))
        {
          _corrections[trimmedWrongWord.ToLower()] = correctWord;
        }
      }
    }

    public string CorrectText(string text)
    {
      if (string.IsNullOrEmpty(text))
      {
        return text;
      }

      string result;
      result = text;

      result = CorrectPhoneNumbers(result);

      int notFound;
      notFound = -1;

      string wrongWordLower;
      string correctWord;
      int index;
      string foundWord;
      string replacement;

      foreach (KeyValuePair<string, string> entry in _corrections)
      {
        wrongWordLower = entry.Key;
        correctWord = entry.Value;
        index = 0;

        while (true)
        {
          index = result.IndexOf(wrongWordLower, index, StringComparison.OrdinalIgnoreCase);

          if (index == notFound)
          {
            break;
          }

          foundWord = result.Substring(index, wrongWordLower.Length);
          replacement = ApplyCasePreserving(correctWord, foundWord);

          result = result.Substring(0, index) + replacement + result.Substring(index + wrongWordLower.Length);

          index = index + replacement.Length;
        }
      }

      return result;
    }

    private string CorrectPhoneNumbers(string text)
    {
      if (string.IsNullOrEmpty(text))
      {
        return text;
      }

      string result;
      result = _phoneRegex.Replace(text, MatchPhoneNumber);

      return result;
    }

    private string MatchPhoneNumber(Match match)
    {
      int operatorCodeGroup;
      operatorCodeGroup = 1;

      int firstPartGroup;
      firstPartGroup = 2;

      int secondPartGroup;
      secondPartGroup = 3;

      int thirdPartGroup;
      thirdPartGroup = 4;

      return $"+380 {match.Groups[operatorCodeGroup].Value} {match.Groups[firstPartGroup].Value} {match.Groups[secondPartGroup].Value} {match.Groups[thirdPartGroup].Value}";
    }

    private string ApplyCasePreserving(string target, string source)
    {
      if (string.IsNullOrEmpty(target) || string.IsNullOrEmpty(source))
      {
        return target;
      }

      bool allUpper;
      allUpper = true;

      foreach (char currentChar in source)
      {
        if (char.IsLetter(currentChar) && !char.IsUpper(currentChar))
        {
          allUpper = false;
          break;
        }
      }

      if (allUpper)
      {
        return target.ToUpper();
      }

      int firstIndex;
      firstIndex = 0;

      int singleLength;
      singleLength = 1;

      if (char.IsUpper(source[firstIndex]) && (source.Length == singleLength || !char.IsUpper(source[firstIndex + singleLength])))
      {
        if (target.Length > firstIndex)
        {
          return char.ToUpper(target[firstIndex]) + target.Substring(singleLength).ToLower();
        }
      }

      return target.ToLower();
    }
  }
}