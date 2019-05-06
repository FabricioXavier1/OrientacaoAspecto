using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrientacaoAspecto
{
    internal class Program
    {
        private static readonly Stopwatch stopwatch = Stopwatch.StartNew();

        private static void Main(string[] args)
        {
            // Primeira chamada do método, nesse ponto, será lento.
            WriteMessage(Hello("world"));
            WriteMessage(Hello("universe"));

            // Segunda chamada do método. O resultado já estará em cache
            WriteMessage(Hello("world"));
            WriteMessage(Hello("universe"));
            Console.ReadKey();
        }

        // Escreve uma mensagem no console contando o tempo
        private static void WriteMessage(string message)
        {
            Console.WriteLine($"{stopwatch.ElapsedMilliseconds} ms - {message}");
        }

        [Cache]
        private static string Hello(string who)
        {
            // Write something to the console and wait to show that the method is actually being executed.
            WriteMessage(string.Format($"fazendo alguma coisa {who}."));
            Thread.Sleep(5000);
            
            return string.Format($"Hello, {who}");
        }
    }
}
