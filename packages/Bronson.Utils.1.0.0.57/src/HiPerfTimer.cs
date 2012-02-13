using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Bronson.Utils
{
    public class HiPerfTimer
    {

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

        private long frequency;
        private long startTick;
        private long stopTick;

        public HiPerfTimer()
        {
            frequency = 0;
            startTick = 0;
            stopTick = 0;
            if (QueryPerformanceFrequency(out frequency) == false)
                throw new Exception("high-performance counter not supported");
        }
          

        public void Start()
        {
            QueryPerformanceCounter(out startTick);
        }

        public void Stop()
        {
            QueryPerformanceCounter(out stopTick);
        }

        /// <summary>
        /// Measure ticks in seconds
        /// </summary>
        public double MeasureTick()
        {
            QueryPerformanceCounter(out this.stopTick);
            return (double)(stopTick - startTick) / (double)frequency;
        }
    }
}
