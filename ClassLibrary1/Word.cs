using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class Word
{
    public HashSet<string> AllWords { get; private set; } = new();
    public string TargetWord { get; private set; }

    private Random random = new();

    public Word(string allWordsPath)
    {
        LoadWords(allWordsPath);
    }

    public void LoadWords(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"Файл не знайдено: {path}");

        var lines = File.ReadAllLines(path, Encoding.UTF8)
                        .Where(l => !string.IsNullOrWhiteSpace(l))
                        .Select(l => l.Replace("\uFEFF", "")
                                      .Trim()
                                      .ToUpperInvariant());

        AllWords = new HashSet<string>(lines);
    }

    public string GenerateTargetWord()
    {
        if (AllWords == null || AllWords.Count == 0)
            throw new InvalidOperationException("Список слів порожній.");

        TargetWord = AllWords.ElementAt(random.Next(AllWords.Count));
        return TargetWord;
    }

    public bool IsValidWord(string word)
    {
        if (string.IsNullOrWhiteSpace(word)) return false;
        var norm = word.Replace("\uFEFF", "").Trim().ToUpperInvariant();
        return AllWords.Contains(norm);
    }
}