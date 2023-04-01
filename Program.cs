public class Program
{


    public static void Main()
    {
        while (true)
        {
            for (int i = 0; i < 10; ++i) Console.WriteLine(Namer.MakeName(4));
            if (Console.ReadKey().Key == ConsoleKey.Escape) break;
        }
    }


}
