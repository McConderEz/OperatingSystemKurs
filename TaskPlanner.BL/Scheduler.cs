using System;
using System.Collections.Generic;
using Spectre.Console;
using System.Threading;

// Класс, представляющий процесс
public class MyProcess
{
    public int PID { get; set; }
    public string Name { get; set; }
    public int ExecutionTime { get; set; }
    public int RemainingTime { get; set; }
    public int MemorySize { get; set; }
    public ProcessState State { get; set; }
    public ProcessPriority Priority { get; set; }
}

// Перечисление для состояния процесса
public enum ProcessState
{
    Waiting,
    Running,
    Finished
}

// Перечисление для приоритета процесса
public enum ProcessPriority
{
    Low,
    Medium,
    High
}

// Класс, представляющий планировщик задач
public class Scheduler
{
    private List<MyProcess> processes;
    private int quantum;
    private int maxMemory;
    private Table table;
    private List<MyProcess> waitingQueue = new List<MyProcess>(); // Ожидающие процессы
    private int currentMemoryUsage = 0;

    public Scheduler(int quantum, int maxMemory)
    {
        processes = new List<MyProcess>();
        this.quantum = quantum;
        this.maxMemory = maxMemory;
        table = new Table().Border(TableBorder.Rounded);
        table.AddColumn("PID").Centered();
        table.AddColumn("Name").Centered();
        table.AddColumn("Execution Time").Centered();
        table.AddColumn("Remaining Time").Centered(); // Добавляем колонку для оставшегося времени
        table.AddColumn("Memory Size").Centered();
        table.AddColumn("State").Centered();
        table.AddColumn("Priority").Centered();
    }

    public void AddProcess(MyProcess process)
    {
        processes.Add(process);
    }

    public void Run()
    {
        // Выполняем процессы в порядке их приоритета
        foreach (var process in processes.OrderByDescending(p => p.Priority).ThenBy(p => p.ExecutionTime))
        {
            if (process.MemorySize <= maxMemory - currentMemoryUsage)
            {
                RunProcess(process);
            }
            else
            {
                // Если для процесса недостаточно памяти, вызываем метод SwapProcess()
                SwapProcess();

                // Проверяем, если после переноса процесса есть достаточно памяти для его выполнения
                if (process.MemorySize <= maxMemory - currentMemoryUsage)
                {
                    RunProcess(process);
                }
                else
                {
                    Console.WriteLine($"Process {process.PID} cannot be executed due to insufficient memory.");
                }
            }
        }
    }


    private void RunProcess(MyProcess process)
    {
        process.State = ProcessState.Running;
        process.RemainingTime -= quantum;

        if (process.RemainingTime <= 0)
        {
            process.State = ProcessState.Finished; // Процесс завершен
        }
        else
        {
            process.State = ProcessState.Waiting; // Процесс в ожидании следующего кванта времени
        }
    }

    private MyProcess GetNextProcess()
    {
        // Сначала выбираем процессы, которые помещаются в доступную память
        var availableProcesses = processes
            .Where(p => p.MemorySize <= maxMemory - currentMemoryUsage)
            .ToList();

        // Если нет доступных процессов, возвращаем null
        if (!availableProcesses.Any())
            return null;

        // Выбираем следующий процесс с наивысшим приоритетом и наименьшим оставшимся временем выполнения
        return availableProcesses
            .OrderByDescending(p => p.Priority)
            .ThenBy(p => p.RemainingTime)
            .FirstOrDefault();
    }


    private void SwapProcess()
    {
        // Переносим процессы из списка processes в waitingQueue, если для них недостаточно памяти
        foreach (var process in processes.ToList())
        {
            if (process.MemorySize > maxMemory - currentMemoryUsage)
            {
                processes.Remove(process);
                waitingQueue.Add(process);
                currentMemoryUsage -= process.MemorySize; // Уменьшаем использование памяти
                Console.WriteLine($"Process {process.PID} moved to waiting queue due to insufficient memory.");
            }
        }

        // Переносим процессы из списка waitingQueue в processes, если для них стало доступно достаточно памяти
        foreach (var process in waitingQueue.ToList())
        {
            if (process.MemorySize <= maxMemory - currentMemoryUsage)
            {
                waitingQueue.Remove(process);
                processes.Add(process);
                currentMemoryUsage += process.MemorySize; // Увеличиваем использование памяти
                Console.WriteLine($"Process {process.PID} moved to active processes from waiting queue.");
            }
        }
    }

    private void UpdateTable()
    {
        Console.Clear();
        table.Rows.Clear();
        foreach (var process in processes)
        {
            table.AddRow(process.PID.ToString(), process.Name, process.ExecutionTime.ToString(), process.RemainingTime.ToString(), process.MemorySize.ToString(), process.State.ToString(), process.Priority.ToString()).Centered();
        }
        AnsiConsole.Render(table);
    }
}
