using System;
using System.Collections.Generic;
using System.Diagnostics;
using OpenLibSys;
using System.Windows.Forms;
using System.Management;
using System.Threading;

namespace WindowsFormsApp2
{
    public class UsageAndFre
    {
        #region Init
        public UsageAndFre()
        {
            for (int i = 0; i < System.Environment.ProcessorCount; i++)
            {
                counters[i] = new PerformanceCounter("Processor", "% Processor Time", i.ToString());
                usages.Add(float.PositiveInfinity);
                frequencys.Add(float.PositiveInfinity);
            }
            this.Update();
        }
        #endregion

        #region Update
        public void Update()
        {
            for (int i = 0; i < counters.Length; i++)
            {
                usages[i] = counters[i].NextValue();
                frequencys[i] = ring0.GetFrequency(i);
            }
        }
        #endregion

        #region UserDate
        public List<float> Usages
        {
            get
            {
                return usages;
            }
        }

        public List<float> Frequencys
        {
            get
            {
                return frequencys;
            }
        }

        private List<float> usages = new List<float>();
        private List<float> frequencys = new List<float>();
        private WinRing0 ring0 = new WinRing0();
        private PerformanceCounter[] counters = new PerformanceCounter[System.Environment.ProcessorCount];//从系统计数器中读取信息
        #endregion
    }

    public class WinRing0
    {
        #region Init
        public WinRing0()
        {
            try
            {
                //-----------------------------------------------------------------------------
                // Initialize
                //-----------------------------------------------------------------------------
                Ols ols = new Ols();

                // Check support library sutatus
                switch (ols.GetStatus())
                {
                    case (uint)Ols.Status.NO_ERROR:
                        break;
                    case (uint)Ols.Status.DLL_NOT_FOUND:
                        MessageBox.Show("Status Error!! DLL_NOT_FOUND");
                        Environment.Exit(0);
                        break;
                    case (uint)Ols.Status.DLL_INCORRECT_VERSION:
                        MessageBox.Show("Status Error!! DLL_INCORRECT_VERSION");
                        Environment.Exit(0);
                        break;
                    case (uint)Ols.Status.DLL_INITIALIZE_ERROR:
                        MessageBox.Show("Status Error!! DLL_INITIALIZE_ERROR");
                        Environment.Exit(0);
                        break;
                }

                // Check WinRing0 status
                switch (ols.GetDllStatus())
                {
                    case (uint)Ols.OlsDllStatus.OLS_DLL_NO_ERROR:
                        break;
                    case (uint)Ols.OlsDllStatus.OLS_DLL_DRIVER_NOT_LOADED:
                        MessageBox.Show("DLL Status Error!! OLS_DRIVER_NOT_LOADED");
                        Environment.Exit(0);
                        break;
                    case (uint)Ols.OlsDllStatus.OLS_DLL_UNSUPPORTED_PLATFORM:
                        MessageBox.Show("DLL Status Error!! OLS_UNSUPPORTED_PLATFORM");
                        Environment.Exit(0);
                        break;
                    case (uint)Ols.OlsDllStatus.OLS_DLL_DRIVER_NOT_FOUND:
                        MessageBox.Show("DLL Status Error!! OLS_DLL_DRIVER_NOT_FOUND");
                        Environment.Exit(0);
                        break;
                    case (uint)Ols.OlsDllStatus.OLS_DLL_DRIVER_UNLOADED:
                        MessageBox.Show("DLL Status Error!! OLS_DLL_DRIVER_UNLOADED");
                        Environment.Exit(0);
                        break;
                    case (uint)Ols.OlsDllStatus.OLS_DLL_DRIVER_NOT_LOADED_ON_NETWORK:
                        MessageBox.Show("DLL Status Error!! DRIVER_NOT_LOADED_ON_NETWORK");
                        Environment.Exit(0);
                        break;
                    case (uint)Ols.OlsDllStatus.OLS_DLL_UNKNOWN_ERROR:
                        MessageBox.Show("DLL Status Error!! OLS_DLL_UNKNOWN_ERROR");
                        Environment.Exit(0);
                        break;
                }

                uint eax = 0, ebx = 0, ecx = 0, edx = 0;
                ols.Cpuid(0, ref eax, ref ebx, ref ecx, ref edx);

                if ((ebx == 0x756e6547) && (ecx == 0x6c65746e) && (edx == 0x49656e69))
                    this.div = 1;

                ManagementObjectSearcher search = new ManagementObjectSearcher("SELECT ExtClock FROM Win32_Processor"); //建立WMI的类

                foreach (ManagementObject mo in search.Get())
                {
                    Originbusspeed = (uint)mo["ExtClock"] * 1.0f;
                }

                if (ols.Rdmsr(0xCE, ref eax, ref edx) == 1)
                {
                    MaxTurboFre = (eax & 0xFF00) >> 8;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                Environment.Exit(0);
            }
        }
        #endregion

        #region Get Frequency Info
        private void EstimateTimeStampCounterFrequency(
          out double frequency)
        {
            ulong countBegin, countEnd;

            long timeBegin = Stopwatch.GetTimestamp();

            uint eax = 0, edx = 0;
            ols.Rdtsc(ref eax, ref edx);
            countBegin = (ulong)(edx << 32) | eax;
            Thread.Sleep(1);

            ols.Rdtsc(ref eax, ref edx);
            countEnd = (ulong)(edx << 32) | eax;
            long timeEnd = Stopwatch.GetTimestamp();

            double delta = (timeEnd - timeBegin);
            frequency = 1e-6 *
              (((double)(countEnd - countBegin)) * Stopwatch.Frequency) / delta / MaxTurboFre;
        }
        public float GetFrequency(int ThreadAffinityMask)
        {
            uint eax = 0, edx = 0;
            if (ols.RdmsrTx(0x198, ref eax, ref edx, (UIntPtr)(1 << ThreadAffinityMask)) != 0)
            {
                if (MaxTurboFre != 0)
                {
                    EstimateTimeStampCounterFrequency( out estimatedTimeStampCounterFrequency);
                    if (estimatedTimeStampCounterFrequency < MaxTurboFre * Originbusspeed)
                    {
                        busspeed = (float)(estimatedTimeStampCounterFrequency);
                    }
                    else
                        busspeed = Originbusspeed;
                }
#if DEBUG 
                if (busspeed < 97 || busspeed > 104)
                    Debug.Print("Core: {0} Fre:{1} Mhz", ThreadAffinityMask.ToString(), busspeed.ToString());
#endif
                return ((eax & (0xFF00)) >> 8) / div * busspeed * 1.0f;
            }
            return float.PositiveInfinity;
        }
        #endregion

        #region Register
        private float busspeed = 0.0f;
        private float Originbusspeed = 0.0f;
        private uint MaxTurboFre = 0;
        private uint div = 2;
        private Ols ols = new Ols();
        private double estimatedTimeStampCounterFrequency = 0.0;
        #endregion
    }

}
