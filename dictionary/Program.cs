//Створити додаток «Словники».
//Основне завдання проєкту: зберігати словники різними мовами і дозволяти користувачеві знаходити переклад потрібного слова або фрази.
//Інтерфейс додатку повинен надавати такі можливості:
//■ Створювати словник.Під час створення необхідно вказати тип словника. Наприклад, англо-російський або російсько-англійський.
//■ Додавати слово і його переклад до вже існуючого словника.Оскільки слово може
//мати декілька перекладів, необхідно дотримуватися можливості створення декількох варіантів перекладу.
//■ Замінювати слово або його переклад у словнику.
//■ Видаляти слово або переклад. Якщо слово видаляється, усі його переклади видаляються разом з ним. Не можна видалити переклад слова, якщо це останній
//варіант перекладу.
//■ Шукати переклад слова.
//■ Словники повинні зберігатися у файлах.
//■ Слово і варіанти його перекладів можна експортувати до окремого файлу результату.
//■ При старті програми потрібно показувати меню для роботи з програмою. Якщо
//вибір пункту меню відкриває підменю, тоді в ньому потрібно передбачити можливість повернення до попереднього меню





using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Translation
{
    public string Word { get; set; }
    public string TranslationText { get; set; }

    public Translation(string word, string translationText)
    {
        Word = word;
        TranslationText = translationText;
    }
}

class Dictionary
{
    private List<Translation> translations;

    public string Name { get; private set; }
    public string FileName { get; private set; }

    public Dictionary(string name, string fileName)
    {
        Name = name;
        FileName = fileName;
        translations = new List<Translation>();
    }

    public void AddTranslation(string word, string translationText)
    {
        Translation translation = new Translation(word, translationText);
        translations.Add(translation);
    }

    public void ReplaceTranslation(string word, string translationText)
    {
        Translation translation = translations.FirstOrDefault(t => t.Word == word);
        if (translation != null)
        {
            translation.TranslationText = translationText;
        }
    }

    public void RemoveTranslation(string word)
    {
        translations.RemoveAll(t => t.Word == word);
    }

    public Translation FindTranslation(string word)
    {
        return translations.FirstOrDefault(t => t.Word == word);
    }

    public void ExportToFile(string fileName)
    {
        using (StreamWriter writer = File.CreateText(fileName))
        {
            foreach (Translation translation in translations)
            {
                writer.WriteLine($"{translation.Word}: {translation.TranslationText}");
            }
        }
    }

    public void ImportFromFile(string fileName)
    {
        translations.Clear();
        using (StreamReader reader = File.OpenText(fileName))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(':');
                if (parts.Length == 2)
                {
                    string word = parts[0].Trim();
                    string translationText = parts[1].Trim();
                    AddTranslation(word, translationText);
                }
            }
        }
    }
}

class Program
{
    private static List<Dictionary> dictionaries;
    private static Dictionary currentDictionary;

    static void Main(string[] args)
    {
        dictionaries = new List<Dictionary>();
        currentDictionary = null;

        while (true)
        {
            Console.WriteLine("1. Create Dictionary");
            Console.WriteLine("2. Select Dictionary");
            Console.WriteLine("3. Add Translation");
            Console.WriteLine("4. Replace Translation");
            Console.WriteLine("5. Remove Translation");
            Console.WriteLine("6. Find Translation");
            Console.WriteLine("7. Export Dictionary");
            Console.WriteLine("8. Import Dictionary");
            Console.WriteLine("9. Exit");
            Console.WriteLine();

            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    CreateDictionary();
                    break;
                case "2":
                    SelectDictionary();
                    break;
                case "3":
                    AddTranslation();
                    break;
                case "4":
                    ReplaceTranslation();
                    break;
                case "5":
                    RemoveTranslation();
                    break;
                case "6":
                    FindTranslation();
                    break;
                case "7":
                    ExportDictionary();
                    break;
                case "8":
                    ImportDictionary();
                    break;
                case "9":
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            Console.WriteLine();
        }
    }

    static void ReplaceTranslation()
    {
        if (currentDictionary == null)
        {
            Console.WriteLine("No dictionary selected. Please select a dictionary first.");
            return;
        }

        Console.Write("Enter word: ");
        string word = Console.ReadLine();
        Console.Write("Enter new translation: ");
        string translation = Console.ReadLine();

        currentDictionary.ReplaceTranslation(word, translation);

        Console.WriteLine("Translation replaced.");
    }

    static void CreateDictionary()
    {
        Console.Write("Enter dictionary name: ");
        string name = Console.ReadLine();
        Console.Write("Enter file name for the dictionary: ");
        string fileName = Console.ReadLine();

        Dictionary dictionary = new Dictionary(name, fileName);
        dictionaries.Add(dictionary);

        Console.WriteLine($"Dictionary '{name}' created.");
    }

    static void SelectDictionary()
    {
        Console.WriteLine("Available dictionaries:");
        for (int i = 0; i < dictionaries.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {dictionaries[i].Name}");
        }

        Console.Write("Enter the number of the dictionary: ");
        string input = Console.ReadLine();
        if (int.TryParse(input, out int index) && index >= 1 && index <= dictionaries.Count)
        {
            currentDictionary = dictionaries[index - 1];
            Console.WriteLine($"Dictionary '{currentDictionary.Name}' selected.");
        }
        else
        {
            Console.WriteLine("Invalid input. Please try again.");
        }
    }


    static void ImportDictionary()
    {
        Console.Write("Enter file name to import from: ");
        string fileName = Console.ReadLine();

        Dictionary dictionary = new Dictionary("Imported Dictionary", fileName);
        dictionary.ImportFromFile(fileName);

        dictionaries.Add(dictionary);

        Console.WriteLine("Dictionary imported.");
    }


    static void FindTranslation()
    {
        if (currentDictionary == null)
        {
            Console.WriteLine("No dictionary selected. Please select a dictionary first.");
            return;
        }

        Console.Write("Enter word: ");
        string word = Console.ReadLine();

        Translation translation = currentDictionary.FindTranslation(word);

        if (translation != null)
        {
            Console.WriteLine($"Translation: {translation.TranslationText}");
        }
        else
        {
            Console.WriteLine("Translation not found.");
        }
    }



    static void AddTranslation()
    {
        if (currentDictionary == null)
        {
            Console.WriteLine("No dictionary selected. Please select a dictionary first.");
            return;
        }

        Console.Write("Enter word: ");
        string word = Console.ReadLine();
        Console.Write("Enter translation: ");
        string translation = Console.ReadLine();

        currentDictionary.AddTranslation(word, translation);

        Console.WriteLine("Translation added.");
    }

   

    static void RemoveTranslation()
    {
        if (currentDictionary == null)
        {
            Console.WriteLine("No dictionary selected. Please select a dictionary first.");
            return;
        }

        Console.Write("Enter word: ");
        string word = Console.ReadLine();

        currentDictionary.RemoveTranslation(word);

        Console.WriteLine("Translation removed.");
    }

  

    static void ExportDictionary()
    {
        if (currentDictionary == null)
        {
            Console.WriteLine("No dictionary selected. Please select a dictionary first.");
            return;
        }

        Console.Write("Enter file name to export to: ");
        string fileName = Console.ReadLine();

        currentDictionary.ExportToFile(fileName);

        Console.WriteLine("Dictionary exported.");
    }
  
}