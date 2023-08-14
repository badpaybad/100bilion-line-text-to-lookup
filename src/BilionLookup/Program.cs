// See https://aka.ms/new-console-template for more information
using System.Text;

Console.WriteLine("Hello, World!");

IList<T> Shuffle<T>(IList<T> ts)
{
    return ts.OrderBy(a => Guid.NewGuid()).ToList();
}
Random _random = new Random();
string _alphabet = "qwertyuiopasdfghjklzxcvbnm";
string _number = "1234567890";
string _special = "~!@#$%^&*()=+_-{}[]<>,.?|`:;";
string RandomPassword(short lenghtOfLowerCase = 3, short lenghtOfUpperCase = 1, short lenghtOfNumber = 2, short lenghtOfSpecial = 0)
{
    var alphabet = new List<char>();
    for (var i = 0; i < lenghtOfLowerCase; i++)
    {
        alphabet.Add(_alphabet[_random.Next(0, _alphabet.Length)]);
    }
    for (var i = 0; i < lenghtOfUpperCase; i++)
    {
        //var irnd = _random.Next(0, alphabet.Count);
        //alphabet[irnd] = alphabet[irnd].ToString().ToUpper()[0];
        alphabet.Add(_alphabet[_random.Next(0, _alphabet.Length)].ToString().ToUpper()[0]);
    }

    var number = new List<char>();
    for (var i = 0; i < lenghtOfNumber; i++)
    {
        number.Add(_number[_random.Next(0, _number.Length)]);
    }
    var special = new List<char>();
    for (var i = 0; i < lenghtOfSpecial; i++)
    {
        special.Add(_special[_random.Next(0, _special.Length)]);
    }

    var temp = new List<char>();
    temp.AddRange(alphabet);
    temp.AddRange(number);
    temp.AddRange(special);

    temp = Shuffle(temp).ToList();

    return string.Join("", temp);
}

/*

100k file txt , each file with 1milion line email random

to solve: index and distinc to lookup

*/
var dirTemp = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
if (Directory.Exists(dirTemp) == false)
{
    Directory.CreateDirectory(dirTemp);
}
var count = 0;

int batchwrite = 50;
int maxCount = 100000/50;
int maxLine = 1000000;
List<Task> tasksWrite = new List<Task>();

async Task Write1MilionLine(int idx, int ccCount)
{
    var pathfile = Path.Combine(dirTemp, $"{ccCount}-{idx}-{DateTime.Now.Ticks}.txt");
    using (var sw = new StreamWriter(pathfile, false))
    {
        for (var l = 0; l < maxLine; l++)
        {
            await sw.WriteLineAsync(RandomPassword(3, 3, 3, 0) + "@" + RandomPassword(1, 1, 1, 0) + "." + RandomPassword(1, 0, 1, 0));
        }
    }

}

while (true)
{
    count++;
    if (count > maxCount)
    {
        break;
    }
    Console.WriteLine("Batch: " + count+ " maxCount: "+ maxCount + " batch size: " + batchwrite);
    tasksWrite = new List<Task>();
    for (var i = 0; i < batchwrite; i++)
    {
        tasksWrite.Add(Write1MilionLine(i,count));
    }
    await Task.WhenAll(tasksWrite);
    tasksWrite.Clear();
}

Console.WriteLine("Done all");

