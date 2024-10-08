using System.Xml.Linq;

namespace ChapterConverter;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        if (args.Length != 2)
        {
            Console.WriteLine("братик, первый аргумент до .xml, второй аргумент до пути куда положить");
            return;
        }

        string xmlPath = args[0];
        string targetPath = args[1];

        if (!Path.Exists(xmlPath))
        {
            Console.WriteLine("нет такова иксмл");
            return;
        }

        if (Path.Exists(targetPath))
        {
            Console.WriteLine("такой таргет уже есть...");
            return;
        }

        string xmlContent = File.ReadAllText(xmlPath);

        XDocument doc = XDocument.Parse(xmlContent);

        var chapters = (doc.Root!.FirstNode as XElement).Descendants("ChapterAtom").Select(a => new
        {
            // таймспан не любит слишком точные значения
            ChapterTimeStart = TimeSpan.Parse(a.Element("ChapterTimeStart").Value.Substring(0, 13)),
            ChapterString = a.Element("ChapterDisplay").Element("ChapterString").Value
        }).ToArray();

        var txtContent = chapters
            .Select(ch => $"{ch.ChapterTimeStart:g} {ch.ChapterString}")
            .ToArray();

        File.WriteAllLines(targetPath, txtContent);
        
        Console.WriteLine($"готово, братик. всего глав получилось {chapters.Length}");
    }
}