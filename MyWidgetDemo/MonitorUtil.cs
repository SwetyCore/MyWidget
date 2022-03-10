using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWidget
{
    // from https://blog.csdn.net/a497785609/article/details/83316165
    public class SysParams

    {

        //    public string NodeName { get; set; }

        //    public float CPUProcessorTime { get; set; }

        //    public float CPUPrivilegedTime { get; set; }

        //    public float CPUInterruptTime { get; set; }

        //    public float CPUDPCTime { get; set; }

        public float processorUtility { get; set; }

        public float MEMAvailable { get; set; }

        //    public float MEMCommited { get; set; }

        //    public float MEMCommitLimit { get; set; }

        public float MEMCommitedPerc { get; set; }

        //    public float MEMPoolPaged { get; set; }

        //    public float MEMPoolNonPaged { get; set; }

        //    public float MEMCached { get; set; }

        //    public float PageFile { get; set; }

        //    public float ProcessorQueueLengh { get; set; }

        //    public float DISCQueueLengh { get; set; }

        //    public float DISKRead { get; set; }

        //    public float DISKWrite { get; set; }

        //    public float DISKAverageTimeRead { get; set; }

        //    public float DISKAverageTimeWrite { get; set; }

        //    public float DISKTime { get; set; }

        //    public float HANDLECountCounter { get; set; }

        //    public float THREADCount { get; set; }

        //    public int CONTENTSwitches { get; set; }

        //    public int SYSTEMCalls { get; set; }

        //    public float NetTrafficSend { get; set; }

        //    public float NetTrafficReceive { get; set; }

        //    public DateTime SamplingTime { get; set; }


        //    private PerformanceCounter cpuProcessorTime = new PerformanceCounter("Processor", "% Processor Time", "_Total", true);

        //    private PerformanceCounter cpuPrivilegedTime = new PerformanceCounter("Processor", "% Privileged Time", "_Total");

        //    private PerformanceCounter cpuInterruptTime = new PerformanceCounter("Processor", "% Interrupt Time", "_Total");

        //    private PerformanceCounter cpuDPCTime = new PerformanceCounter("Processor", "% DPC Time", "_Total");

        private PerformanceCounter cpuProcessorUtility = new PerformanceCounter("Processor Information", "% Processor Utility", "_Total");

        private PerformanceCounter memAvailable = new PerformanceCounter("Memory", "Available MBytes", null);

        //    private PerformanceCounter memCommited = new PerformanceCounter("Memory", "Committed Bytes", null);

        //    private PerformanceCounter memCommitLimit = new PerformanceCounter("Memory", "Commit Limit", null);

        private PerformanceCounter memCommitedPerc = new PerformanceCounter("Memory", "% Committed Bytes In Use", null);

        //    private PerformanceCounter memPollPaged = new PerformanceCounter("Memory", "Pool Paged Bytes", null);

        //    private PerformanceCounter memPollNonPaged = new PerformanceCounter("Memory", "Pool Nonpaged Bytes", null);

        //    private PerformanceCounter memCached = new PerformanceCounter("Memory", "Cache Bytes", null);

        //    private PerformanceCounter pageFile = new PerformanceCounter("Paging File", "% Usage", "_Total");

        //    private PerformanceCounter processorQueueLengh = new PerformanceCounter("System", "Processor Queue Length", null);

        //    private PerformanceCounter diskQueueLengh = new PerformanceCounter("PhysicalDisk", "Avg. Disk Queue Length", "_Total");

        //    private PerformanceCounter diskRead = new PerformanceCounter("PhysicalDisk", "Disk Read Bytes/sec", "_Total");

        //    private PerformanceCounter diskWrite = new PerformanceCounter("PhysicalDisk", "Disk Write Bytes/sec", "_Total");

        //    private PerformanceCounter diskAverageTimeRead = new PerformanceCounter("PhysicalDisk", "Avg. Disk sec/Read", "_Total");

        //    private PerformanceCounter diskAverageTimeWrite = new PerformanceCounter("PhysicalDisk", "Avg. Disk sec/Write", "_Total");

        //    private PerformanceCounter diskTime = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");

        //    private PerformanceCounter handleCountCounter = new PerformanceCounter("Process", "Handle Count", "_Total");

        //    private PerformanceCounter threadCount = new PerformanceCounter("Process", "Thread Count", "_Total");

        //    private PerformanceCounter contentSwitches = new PerformanceCounter("System", "Context Switches/sec", null);

        //    private PerformanceCounter systemCalls = new PerformanceCounter("System", "System Calls/sec", null);

        //    private PerformanceCounterCategory performanceNetCounterCategory;

        //    private PerformanceCounter[] trafficSentCounters;

        //    private PerformanceCounter[] trafficReceivedCounters;

        //    private string[] interfaces = null;

        //    public void initNetCounters()

        //    {

        //        // PerformanceCounter(CategoryName,CounterName,InstanceName)

        //        performanceNetCounterCategory = new PerformanceCounterCategory("Network Interface");

        //        interfaces = performanceNetCounterCategory.GetInstanceNames();

        //        int length = interfaces.Length;

        //        if (length > 0)

        //        {

        //            trafficSentCounters = new PerformanceCounter[length];

        //            trafficReceivedCounters = new PerformanceCounter[length];

        //        }

        //        for (int i = 0; i < length; i++)

        //        {

        //            // Initializes a new, read-only instance of the PerformanceCounter class.

        //            //   1st paramenter: "categoryName"-The name of the performance counter category (performance object) with which this performance counter is associated.

        //            //   2nd paramenter: "CounterName" -The name of the performance counter.

        //            //   3rd paramenter: "instanceName" -The name of the performance counter category instance, or an empty string (""), if the category contains a single instance.

        //            trafficReceivedCounters[i] = new PerformanceCounter("Network Interface", "Bytes Sent/sec", interfaces[i]);

        //            trafficSentCounters[i] = new PerformanceCounter("Network Interface", "Bytes Sent/sec", interfaces[i]);

        //        }

        //        // List of all names of the network interfaces

        //        for (int i = 0; i < length; i++)

        //        {

        //            Console.WriteLine("Name netInterface: {0}", performanceNetCounterCategory.GetInstanceNames()[i]);

        //        }

        //    }

        //    public void getProcessorCpuTime()

        //    {

        //        float tmp = cpuProcessorTime.NextValue();

        //        CPUProcessorTime = (float)Math.Round(tmp, 1);

        //        // Environment.ProcessorCount: return the total number of cores

        //    }

        //    public void getCpuPrivilegedTime()

        //    {

        //        float tmp = cpuPrivilegedTime.NextValue();

        //        CPUPrivilegedTime = (float)(Math.Round((double)tmp, 1));

        //    }

        //    public void getCpuinterruptTime()

        //    {
        //        cpuInterruptTime.MachineName=".";
        //        float tmp = cpuInterruptTime.NextValue();

        //        CPUInterruptTime = (float)(Math.Round((double)tmp, 1));

        //    }

        //    public void getcpuDPCTime()

        //    {

        //        float tmp = cpuDPCTime.NextValue();

        //        CPUDPCTime = (float)(Math.Round((double)tmp, 1));

        //    }

        public void getprocessorUtility()
        {
            float tmp = cpuProcessorUtility.NextValue();

            processorUtility = (float)(Math.Round((double)tmp, 1));
        }
        //    public void getPageFile()

        //    {

        //        PageFile = pageFile.NextValue();

        //    }

        //    public void getProcessorQueueLengh()

        //    {

        //        ProcessorQueueLengh = processorQueueLengh.NextValue();

        //    }

        public void getMemAvailable()

        {

            MEMAvailable = memAvailable.NextValue();

        }

        //    public void getMemCommited()

        //    {

        //        MEMCommited = memCommited.NextValue() / (1024 * 1024);

        //    }

        //    public void getMemCommitLimit()

        //    {

        //        MEMCommitLimit = memCommitLimit.NextValue() / (1024 * 1024);

        //    }

        public void getMemCommitedPerc()

    {

        float tmp = memCommitedPerc.NextValue();

        // return the value of Memory Commit Limit

        MEMCommitedPerc = (float)(Math.Round((double)tmp, 1));

    }

        //    public void getMemPoolPaged()

        //    {

        //        float tmp = memPollPaged.NextValue() / (1024 * 1024);

        //        MEMPoolPaged = (float)(Math.Round((double)tmp, 1));

        //    }

        //    public void getMemPoolNonPaged()

        //    {

        //        float tmp = memPollNonPaged.NextValue() / (1024 * 1024);

        //        MEMPoolNonPaged = (float)(Math.Round((double)tmp, 1));

        //    }

        //    public void getMemCachedBytes()

        //    {

        //        // return the value of Memory Cached in MBytes

        //        MEMCached = memCached.NextValue() / (1024 * 1024);

        //    }

        //    public void getDiskQueueLengh()

        //    {

        //        DISCQueueLengh = diskQueueLengh.NextValue();

        //    }

        //    public void getDiskRead()

        //    {

        //        float tmp = diskRead.NextValue() / 1024;

        //        DISKRead = (float)(Math.Round((double)tmp, 1));

        //    }

        //    public void getDiskWrite()

        //    {

        //        float tmp = diskWrite.NextValue() / 1024;

        //        DISKWrite = (float)(Math.Round((double)tmp, 1)); // round 1 digit decimal

        //    }

        //    public void getDiskAverageTimeRead()

        //    {

        //        float tmp = diskAverageTimeRead.NextValue() * 1000;

        //        DISKAverageTimeRead = (float)(Math.Round((double)tmp, 1)); // round 1 digit decimal

        //    }

        //    public void getDiskAverageTimeWrite()

        //    {

        //        float tmp = diskAverageTimeWrite.NextValue() * 1000;

        //        DISKAverageTimeWrite = (float)(Math.Round((double)tmp, 1)); // round 1 digit decimal

        //    }

        //    public void getDiskTime()

        //    {

        //        float tmp = diskTime.NextValue();

        //        DISKTime = (float)(Math.Round((double)tmp, 1));

        //    }



        //    public void getHandleCountCounter()

        //    {

        //        HANDLECountCounter = handleCountCounter.NextValue();

        //    }

        //    public void getThreadCount()

        //    {

        //        THREADCount = threadCount.NextValue();

        //    }

        //    public void getContentSwitches()

        //    {

        //        CONTENTSwitches = (int)Math.Ceiling(contentSwitches.NextValue());

        //    }

        //    public void getsystemCalls()

        //    {

        //        SYSTEMCalls = (int)Math.Ceiling(systemCalls.NextValue());

        //    }

        //    public void getCurretTrafficSent()

        //    {

        //        int length = interfaces.Length;

        //        float sendSum = 0.0F;

        //        for (int i = 0; i < length; i++)

        //        {

        //            sendSum += trafficSentCounters[i].NextValue();

        //        }

        //        float tmp = 8 * (sendSum / 1024);

        //        NetTrafficSend = (float)(Math.Round((double)tmp, 1));

        //    }

        //    public void getCurretTrafficReceived()

        //    {

        //        int length = interfaces.Length;

        //        float receiveSum = 0.0F;

        //        for (int i = 0; i < length; i++)

        //        {

        //            receiveSum += trafficReceivedCounters[i].NextValue();

        //        }

        //        float tmp = 8 * (receiveSum / 1024);

        //        NetTrafficReceive = (float)(Math.Round((double)tmp, 1));

        //    }

        //    public void getSampleTime()

        //    {

        //        SamplingTime = DateTime.Now;

        //    }

    }
}
