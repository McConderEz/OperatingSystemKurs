using System;
using System.Collections.Generic;
using System.Linq;

public class Message
{
    public string Sender { get; set; }
    public string Content { get; set; }
}

public class Channel
{
    private List<Message> messages = new List<Message>();

    public void SendMessage(string sender, string content)
    {
        Message message = new Message { Sender = sender, Content = content };
        messages.Add(message);
    }

    public Message ReceiveMessage()
    {
        if (messages.Any())
        {
            Message message = messages.First();
            messages.RemoveAt(0);
            return message;
        }
        else
        {
            return null;
        }
    }
}

public class SharedMemory
{
    private Dictionary<string, object> data = new Dictionary<string, object>();

    public void Write(string key, object value)
    {
        data[key] = value;
    }

    public object Read(string key)
    {
        if (data.ContainsKey(key))
        {
            return data[key];
        }
        else
        {
            return null;
        }
    }
}

public class ProcessScheduler
{
    private int currentTime;
    private int timeQuantum;
    private int memorySize;
    private List<Process> processes;
    private List<Process> diskProcesses;
    private List<Process> completedProcesses;
    private List<Process> swappedProcesses;
    private Channel channel;
    private SharedMemory sharedMemory;

    public ProcessScheduler(int timeQuantum, int memorySize, Channel channel, SharedMemory sharedMemory)
    {
        currentTime = 0;
        this.timeQuantum = timeQuantum;
        this.memorySize = memorySize;
        processes = new List<Process>();
        diskProcesses = new List<Process>();
        completedProcesses = new List<Process>();
        swappedProcesses = new List<Process>();
        this.channel = channel;
        this.sharedMemory = sharedMemory;
    }

    public void AddProcess(Process process)
    {
        if (processes.Count < memorySize)
        {
            processes.Add(process);
        }
        else
        {
            diskProcesses.Add(process);
        }
        SendMessage("ProcessScheduler", $"Добавлен новый процесс: {process.Name}");
    }

    public void SwapProcess(Process process)
    {
        Console.WriteLine($"Свопинг {process.Name} во времени {currentTime}");
        swappedProcesses.Add(process);
        if (processes.Count < memorySize)
        {
            processes.Add(process);
        }
        else
        {
            Console.WriteLine("Основная память полная, процесс отправлен на диск.");
            SendMessage("ProcessScheduler", $"Процесс {process.Name} отправлен на диск.");
        }
    }

    public void Run()
    {
        while (processes.Any())
        {
            Process process = processes.First();
            processes.RemoveAt(0);
            Console.WriteLine($"Выполнение {process.Name} во времени {currentTime}");
            if (process.BurstTime <= timeQuantum)
            {
                currentTime += process.BurstTime;
                process.BurstTime = 0;
            }
            else
            {
                currentTime += timeQuantum;
                process.BurstTime -= timeQuantum;
                SwapProcess(process);
            }

            if (process.BurstTime == 0)
            {
                process.CompletionTime = currentTime;
                completedProcesses.Add(process);
            }

            Message message = process.ReceiveMessage();
            if (message != null)
            {
                Console.WriteLine($"Процесс {process.Name} получил сообщение: {message.Content}");
            }

            while (diskProcesses.Any() && processes.Count < memorySize)
            {
                Process diskProcess = diskProcesses.First();
                diskProcesses.RemoveAt(0);
                Console.WriteLine($"Процесс {diskProcess.Name} возвращен из дискового пространства.");
                processes.Add(diskProcess);
            }

            WriteToSharedMemory("current_time", currentTime);
        }
    }

    public void SendMessage(string recipient, string content)
    {
        channel.SendMessage(recipient, content);
    }

    public Message ReceiveMessage()
    {
        return channel.ReceiveMessage();
    }

    public void WriteToSharedMemory(string key, object value)
    {
        sharedMemory.Write(key, value);
    }

    public object ReadFromSharedMemory(string key)
    {
        return sharedMemory.Read(key);
    }

    public void PrintCompletedProcesses()
    {
        Console.WriteLine("Завершенные процессы:");
        foreach (Process process in completedProcesses)
        {
            Console.WriteLine($"{process.Name} завершился во времени {process.CompletionTime}");
        }
    }

    public void GenerateProcesses(int numProcesses)
    {
        Random random = new Random();
        for (int i = 0; i < numProcesses; i++)
        {
            string name = "Process" + (i + 1);
            int burstTime = random.Next(1, 10);
            Process process = new Process(name, burstTime);
            AddProcess(process);
        }
    }
}

public class Process
{
    public string Name { get; set; }
    public int BurstTime { get; set; }
    public int CompletionTime { get; set; }
    private Channel channel;

    public Process(string name, int burstTime)
    {
        Name = name;
        BurstTime = burstTime;
        channel = new Channel();
    }

    public void SendMessage(string recipient, string content)
    {
        channel.SendMessage(Name, content);
    }

    public Message ReceiveMessage()
    {
        return channel.ReceiveMessage();
    }
}
