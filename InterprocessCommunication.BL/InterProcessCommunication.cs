using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace InterprocessCommunication.BL
{
    public class InterProcessCommunication
    {
        public void Start()
        {
            ExampleChannel();

            ExampleNamedPipe();

            ExampleMemoryMapped();

            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        static void ExampleChannel()
        {
            Console.WriteLine("Пример работы с обычным каналом");

            var channel = new Channel<int>();

            Task producerTask = Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Отправка данных в канал: {i}");
                    channel.Send(i);
                    Thread.Sleep(100);
                }
                Console.ResetColor();
                channel.Close();
            });

            Task consumerTask = Task.Run(() =>
            {
                foreach (var item in channel)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Получено из канала: {item}");
                }
                Console.ResetColor();
            });

            Task.WaitAll(producerTask, consumerTask);

            Console.WriteLine();
        }
        static void ExampleNamedPipe()
        {
            Console.WriteLine("Пример работы с именованным каналом");

            string pipeName = "MyNamedPipe";

            Task producerTask = Task.Run(() =>
            {
                using (var namedPipeServer = new NamedPipeServer(pipeName))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Отправка данных в именнованный канал: {i}");
                        namedPipeServer.Send(i);
                        Thread.Sleep(100);
                    }
                    Console.ResetColor();
                }
            });
            Task consumerTask = Task.Run(() =>
            {
                using (var namedPipeClient = new NamedPipeClient(pipeName))
                {
                    int data;
                    while ((data = namedPipeClient.Receive()) != -1)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Получено из именованного канала: {data}");
                    }
                    Console.ResetColor();
                }
            });

            Task.WaitAll(producerTask, consumerTask);

            Console.WriteLine();
        }

        static void ExampleMemoryMapped() //TODO: Добавить синхронизацию
        {
            Console.WriteLine("Пример работы с отображаемой памятью");

            using (var memoryMappedFile = new MemoryMappedFile<int>("MyMemoryMappedFile", 10))
            {
                var producerSemaphore = new SemaphoreSlim(1);
                var consumerSemaphore = new SemaphoreSlim(0);

                Task producerTask = Task.Run(() =>
                {
                    for (int i = 0; i < 10; i++)
                    {
                        producerSemaphore.Wait();
                        memoryMappedFile.Write(i);
                        consumerSemaphore.Release();
                        Thread.Sleep(100);
                    }
                });

                Task consumerTask = Task.Run(() =>
                {
                    for (int i = 0; i < 10; i++)
                    {
                        consumerSemaphore.Wait();
                        int data = memoryMappedFile.Read();
                        Console.WriteLine($"Прочитано из отображаемой памяти: {data}");
                        producerSemaphore.Release();
                        Thread.Sleep(100);
                    }
                });

                Task.WaitAll(producerTask, consumerTask);
            }

            Console.WriteLine();
        }
    }
}
