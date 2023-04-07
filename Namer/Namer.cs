/*
Namer: a lib for generating japanese names, toponyms etc.
Vasya Ryzhkoff, 2023
*/


using System.Text.Json;
using Ieroglyphs;
public static class Namer
{

    private static string _jsonFile = "Namer/ieroglyph_list.json";
    private static string _jsonText = File.ReadAllText(_jsonFile);
    private static List<Ieroglyph>? _ieroglyphs = JsonSerializer.Deserialize<List<Ieroglyph>>(_jsonText);

    private static bool NextIsVowel(string name, int index)
    {
        char[] vowels = { 'а', 'и', 'у', 'э', 'о', 'я', 'ю', 'ё' };
        foreach (var vovel in vowels)
        {
            if (name[index + 1] == vovel) return true;
        }
        return false;
    }

    public static (string name, string meaning) MakeName(int max_len)
    {
        if (_ieroglyphs == null) throw (new NullReferenceException($"Json data was not found. Check {_jsonFile}"));

        var rndGen = new Random();
        int nameLen = rndGen.Next(2, max_len);
        int ieroglyphArrayLen = _ieroglyphs.Count;

        string name = "";
        string meaning = "";


        for (int i = 0; i < nameLen; ++i)
        {
            Ieroglyph currentIeroglyph = _ieroglyphs[rndGen.Next(ieroglyphArrayLen)];
            int spellingArrayLen = currentIeroglyph.spelling.Count; //У одного иероглифа есть разные варианты произношения

            meaning += currentIeroglyph.meaning + " ";

            name += currentIeroglyph.spelling[rndGen.Next(spellingArrayLen)];
        }


        var sokuonIndex = name.IndexOf('@');
        if (sokuonIndex != -1)
        {
            //Если сокуон не в конце слова и не перед гласной....
            if (sokuonIndex != name.Length - 1 && !NextIsVowel(name, sokuonIndex)) name = name.Replace('@', name[sokuonIndex + 1]);
            else return MakeName(max_len); //Иначе генери новое слово...
        }
        return (meaning, name);
    }

}