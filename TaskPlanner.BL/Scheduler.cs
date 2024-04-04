using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskPlanner.BL
{
    public enum ProcessState
    {
        Running,
        Waiting,
        Completed
    }

    public enum ProcessPriority
    {
        Low,
        Medium,
        High
    }

    public class MyProcess
    {
        public int PID { get; set; }
        public string ProcessName { get; set; }
        public int ExecutionTime { get; set; }
        public int RemainingTime { get; set; }
        public int MemorySize { get; set; }
        public ProcessState State { get; set; }
        public ProcessPriority Priority { get; set; }

        public MyProcess(int pid, string processName, int executionTime, int memorySize, ProcessPriority priority)
        {
            PID = pid;
            ProcessName = processName;
            ExecutionTime = executionTime;
            RemainingTime = executionTime;
            MemorySize = memorySize;
            State = ProcessState.Waiting;
            Priority = priority;
        }
    }

    public class Scheduler
    {
        private List<MyProcess> processes;
        private int timeQuantum;
        private int maxMemorySize;
        private int availableMemorySize;

        public Scheduler(int timeQuantum, int maxMemorySize)
        {
            this.timeQuantum = timeQuantum;
            this.maxMemorySize = maxMemorySize;
            processes = new List<MyProcess>();
            availableMemorySize = maxMemorySize;
        }

        public void AddProcess(MyProcess process)
        {
            MyProcess existingProcess = processes.FirstOrDefault(p => p.PID == process.PID);
            if (existingProcess != null)
            {
                existingProcess.ProcessName = process.ProcessName;
                existingProcess.ExecutionTime = process.ExecutionTime;
                existingProcess.RemainingTime = process.RemainingTime;
                existingProcess.MemorySize = process.MemorySize;
                existingProcess.Priority = process.Priority;
            }
            else if(existingProcess.MemorySize <= availableMemorySize) 
            {
                processes.Add(process);
                availableMemorySize -= existingProcess.MemorySize; 
            }
        }

        public void Run()
        {
            Console.WriteLine("Starting Scheduler...");

            while (processes.Count > 0)
            {
                MyProcess currentProcess = GetNextProcess();
                if (currentProcess != null)
                {
                    if (currentProcess.State == ProcessState.Waiting)
                    {
                        RunProcess(currentProcess);
                    }

                    if (currentProcess.RemainingTime > 0)
                    {
                        if (currentProcess.State != ProcessState.Running)
                        {
                            SwapProcess(currentProcess);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Process {currentProcess.PID} ({currentProcess.ProcessName}) completed.");
                        processes.Remove(currentProcess);
                        availableMemorySize += currentProcess.MemorySize;
                    }
                }

                PrintProcessTable();
                Thread.Sleep(3000);
                Console.Clear();
            }

            Console.WriteLine("Scheduler finished.");
        }

        private MyProcess GetNextProcess()
        {
            if (processes.Count == 0)
            {
                return null;
            }

            processes.Sort((p1, p2) =>
            {
                int priorityCompare = p2.Priority.CompareTo(p1.Priority);
                if (priorityCompare != 0)
                {
                    return priorityCompare;
                }
                else
                {
                    return p1.RemainingTime.CompareTo(p2.RemainingTime);
                }
            });

            return processes.FirstOrDefault(p => p.MemorySize <= availableMemorySize);
        }

        private void RunProcess(MyProcess process)
        {
            Console.WriteLine($"Running process {process.PID} ({process.ProcessName}) with memory size {process.MemorySize}...");
            process.State = ProcessState.Running;

            for (int i = 0; i < timeQuantum; i++)
            {
                Thread.Sleep(100);
                process.RemainingTime--;
                if (process.RemainingTime <= 0)
                {
                    break;
                }
            }

            process.State = ProcessState.Waiting;
        }

        private void SwapProcess(MyProcess process)
        {
            MyProcess existingProcess = processes.FirstOrDefault(p => p.PID == process.PID);

            if (existingProcess != null)
            {
                existingProcess.ProcessName = process.ProcessName;
                existingProcess.ExecutionTime = process.ExecutionTime;
                existingProcess.RemainingTime = process.RemainingTime;
                existingProcess.MemorySize = process.MemorySize;
                existingProcess.Priority = process.Priority;
                Console.WriteLine($"Process {existingProcess.PID} already exists. Updating process details.");
            }
            else
            {
                if (availableMemorySize >= process.MemorySize)
                {
                    Console.WriteLine($"Swapping out process {process.PID} ({process.ProcessName})...");
                    availableMemorySize += process.MemorySize;
                }
                else
                {
                    MyProcess lowestPriorityProcess = processes.OrderBy(p => p.Priority).FirstOrDefault(p => p.MemorySize <= availableMemorySize);
                    if (lowestPriorityProcess != null)
                    {
                        Console.WriteLine($"Swapping out process {lowestPriorityProcess.PID} ({lowestPriorityProcess.ProcessName}) due to low available memory...");
                        processes.Remove(lowestPriorityProcess);
                        availableMemorySize += lowestPriorityProcess.MemorySize;
                    }
                    else
                    {
                        Console.WriteLine("No process to swap out. Memory is full.");
                        return;
                    }
                }

                Console.WriteLine($"Swapping in process {process.PID} ({process.ProcessName})...");
                processes.Add(process);
                availableMemorySize -= process.MemorySize;
            }
        }

        private void PrintProcessTable()
        {
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("|  PID  |    Process Name    |  Execution Time  |  Remaining Time  |  Memory Size  |     State      |  Memory Usage (%)  |");
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");

            foreach (var process in processes)
            {
                double memoryUsage = (double)process.MemorySize / maxMemorySize * 100;
                Console.WriteLine($"{process.PID,-5} |  {process.ProcessName,-18} |  {process.ExecutionTime,-15} |  {process.RemainingTime,-15} |  {process.MemorySize,-12} |  {process.State,-15} |  {memoryUsage,-17:f2} |");
            }

            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine();
        }
    }
}