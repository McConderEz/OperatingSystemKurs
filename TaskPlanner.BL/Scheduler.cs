using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console;
using System.Threading;

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

public enum ProcessState
{
    Waiting,
    Running,
    Finished
}

public enum ProcessPriority
{
    Low,
    Medium,
    High
}

public class Scheduler
{
    private List<MyProcess> processes = new List<MyProcess>();
    private readonly int quantum;
    private readonly int maxMemory;
    private Table table = new Table();
    private int currentMemoryUsage = 0;

    public Scheduler(int quantum, int maxMemory)
    {
        this.quantum = quantum;
        this.maxMemory = maxMemory;
        InitTable();
    }

    private void InitTable()
    {
        table.Border(TableBorder.Rounded);
        table.AddColumn("PID");
        table.AddColumn("Name");
        table.AddColumn("Execution Time");
        table.AddColumn("Remaining Time");
        table.AddColumn("State");
        table.AddColumn("Priority");
    }

    public void AddProcess(MyProcess process)
    {
        processes.Add(process);
        process.State = ProcessState.Waiting;
    }

    public void Run()
    {
        while (processes.Any(p => p.State != ProcessState.Finished))
        {
            
            var process = processes
                .Where(p => p.State == ProcessState.Waiting)
                .OrderByDescending(p => p.Priority)
                .ThenBy(p => p.RemainingTime)
                .FirstOrDefault();

            if (process != null && currentMemoryUsage + process.MemorySize <= maxMemory)
            {
                RunProcess(process);
            }

           
            UpdateTable();
            Thread.Sleep(1000);
        }
    }

    private void RunProcess(MyProcess process)
    {
        process.State = ProcessState.Running;
        UpdateTable();
        AnsiConsole.Write(new Markup($"Running process: [bold]{process.Name}[/] (PID: {process.PID})\n"));
        Thread.Sleep(quantum * 200);

        process.RemainingTime -= quantum;
        if (process.RemainingTime <= 0)
        {
            process.State = ProcessState.Finished;
            AnsiConsole.Write(new Markup($"[bold]{process.Name}[/] (PID: {process.PID}) finished.\n"));

            processes.Remove(process);
        }
        else
        {
            process.State = ProcessState.Waiting;
        }
    }

    private void UpdateTable()
    {
        AnsiConsole.Clear();
        table.Rows.Clear();
        foreach (var process in processes)
        {
            table.AddRow(
                process.PID.ToString(),
                process.Name,
                process.ExecutionTime.ToString(),
                Math.Max(process.RemainingTime, 0).ToString(),
                process.State.ToString(),
                process.Priority.ToString()
            );
        }
        AnsiConsole.Render(table);
    }
}