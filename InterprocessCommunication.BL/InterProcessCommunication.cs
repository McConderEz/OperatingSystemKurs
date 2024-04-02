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
            // Пример работы с обычным каналом
            ExampleChannel();

            // Пример работы с именованным каналом
            ExampleNamedPipe();

            // Пример работы с отображаемой памятью
            ExampleMemoryMapped();

            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        static void ExampleChannel()
        {
            Console.WriteLine("Пример работы с обычным каналом");

            // Создание канала
            var channel = new Channel<int>();

            // Создание и запуск производителя для канала
            Task producerTask = Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    // Отправка данных в канал
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Отправка данных в канал: {i}");
                    channel.Send(i);
                    Thread.Sleep(100);
                }
                Console.ResetColor();
                // Закрытие канала после отправки всех данных
                channel.Close();
            });

            // Создание и запуск потребителя для канала
            Task consumerTask = Task.Run(() =>
            {
                // Получение данных из канала
                foreach (var item in channel)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Получено из канала: {item}");
                }
                Console.ResetColor();
            });

            // Ожидание завершения работы производителя и потребителя
            Task.WaitAll(producerTask, consumerTask);

            Console.WriteLine();
        }

        // Пример работы с именованным каналом
        static void ExampleNamedPipe()
        {
            Console.WriteLine("Пример работы с именованным каналом");

            // Имя именованного канала
            string pipeName = "MyNamedPipe";

            // Создание и запуск производителя для именованного канала
            Task producerTask = Task.Run(() =>
            {
                using (var namedPipeServer = new NamedPipeServer(pipeName))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        // Отправка данных через именованный канал
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Отправка данных в именнованный канал: {i}");
                        namedPipeServer.Send(i);
                        Thread.Sleep(100);
                    }
                    Console.ResetColor();
                }
            });

            // Создание и запуск потребителя для именованного канала
            Task consumerTask = Task.Run(() =>
            {
                using (var namedPipeClient = new NamedPipeClient(pipeName))
                {
                    // Получение данных через именованный канал
                    int data;
                    while ((data = namedPipeClient.Receive()) != -1)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Получено из именованного канала: {data}");
                    }
                    Console.ResetColor();
                }
            });

            // Ожидание завершения работы производителя и потребителя
            Task.WaitAll(producerTask, consumerTask);

            Console.WriteLine();
        }

        // Пример работы с отображаемой памятью
        static void ExampleMemoryMapped()
        {
            Console.WriteLine("Пример работы с отображаемой памятью");

            // Создание отображаемой памяти
            using (var memoryMappedFile = new MemoryMappedFile<int>("MyMemoryMappedFile", 10))
            {
                // Создание и запуск производителя для отображаемой памяти
                Task producerTask = Task.Run(() =>
                {
                    for (int i = 0; i < 10; i++)
                    {
                        // Запись данных в отображаемую память
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Запись данных в отображаемую память: {i}");
                        memoryMappedFile.Write(i);
                        Thread.Sleep(100);
                    }
                    Console.ResetColor();
                });

                // Создание и запуск потребителя для отображаемой памяти
                Task consumerTask = Task.Run(() =>
                {
                    // Чтение данных из отображаемой памяти
                    for (int i = 0; i < 10; i++)
                    {
                        int data = memoryMappedFile.Read();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Прочитано из отображаемой памяти: {data}");
                        Thread.Sleep(120);
                    }
                    Console.ResetColor();
                });

                // Ожидание завершения работы производителя и потребителя
                Task.WaitAll(producerTask, consumerTask);
            }

            Console.WriteLine();
        }
    }
}
