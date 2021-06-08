using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Text;

namespace Repassage
{
    public class FileReader
    {
        public string GetFilePath(string target)
        {
            var localFolder = Path.GetDirectoryName(Application.ExecutablePath);
            var index = localFolder.LastIndexOf("bin");
            localFolder = localFolder.Substring(0, index);
            var path = Path.GetFullPath(Path.Combine(localFolder, @"..\Repassage\" + target));

            return path;
        }

        public Dictionary<string, string> GetTexts(string scenarioPath)
        {
            var text = File.ReadAllText(scenarioPath).Split(' ');
            var texts = new Dictionary<string, string>();
            var isLetterFirst = true;
            var newAuthor = "";
            var newText = new StringBuilder();

            foreach (var word in text)
            {
                if (word.Length > 0 && word[0].Equals('('))
                {
                    if (isLetterFirst)
                    {
                        newAuthor = word;
                        isLetterFirst = false;
                        continue;
                    }

                    texts.Add(newAuthor, newText.ToString());
                    newText = new StringBuilder();
                    newAuthor = word;
                }

                else newText.Append(word + " ");
            }

            return texts;
        }

        public Dictionary<int, int[]> GetLevels(string levelsPath)
        {
            var rawLevels = File.ReadAllText(levelsPath).Split(' ');
            var allLevels = new Dictionary<int, int[]>();
            var isFirstLevel = true;
            var army = new List<int>();
            var route = new StringBuilder();

            foreach (var word in rawLevels)
            {
                if (word.Length > 0 && word[0].Equals('('))
                {
                    if (isFirstLevel)
                    {
                        foreach (var letter in word)
                            if (Char.IsDigit(letter)) route.Append(letter);
                        isFirstLevel = false;
                        continue;
                    }

                    if (route.Length > 0 && Int32.Parse(route.ToString()) > 0)
                        allLevels.Add(Int32.Parse(route.ToString()), army.ToArray());

                    route = new StringBuilder();
                    foreach (var letter in word)
                        if (Char.IsDigit(letter)) route.Append(letter);
                    army = new List<int>();
                }

                else if (word.Length > 0 && Char.IsDigit(word[0]))
                    army.Add(Int32.Parse(word));
            }

            return allLevels;
        }
    }
}