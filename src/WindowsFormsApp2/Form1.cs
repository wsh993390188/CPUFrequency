using System;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;
using Microsoft.Win32;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private TreeModel _model;
        private Thread m_thread;
        private UsageAndFre info;
        #region Init
        public Form1()
        {
            if (!IsAdmin())
            {
                RunAsRestart();
                Environment.Exit(0);
            }
            info = new UsageAndFre();
            InitializeComponent();
            _model = new TreeModel();
            NodeInsert(info.Usages, info.Frequencys);
            treeViewAdv.Model = _model;
        }
        private bool RunAsRestart()
        {
            string[] args = Environment.GetCommandLineArgs();

            foreach (string s in args)
            {
                if (s.Equals("runas"))
                {
                    return false;
                }
            }
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.WorkingDirectory = Environment.CurrentDirectory;
            startInfo.FileName = Application.ExecutablePath;
            startInfo.Verb = "runas";
            startInfo.Arguments = "runas";

            try
            {
                Process.Start(startInfo);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private bool IsAdmin()
        {
            OperatingSystem osInfo = Environment.OSVersion;
            if (osInfo.Platform == PlatformID.Win32Windows)
            {
                return true;
            }
            else
            {
                WindowsIdentity usrId = WindowsIdentity.GetCurrent();
                WindowsPrincipal p = new WindowsPrincipal(usrId);
                return p.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }
        #endregion
        private void timer1_Tick(object sender, EventArgs e)
        {
            if(m_thread == null)
            {
                m_thread = new Thread(new ThreadStart(UpdateData));
                m_thread.IsBackground = true;
                m_thread.Start();
            }
        }

        private void UpdateData()
        {
            while(true)
            {
                Thread.Sleep(1000);
                NodeUpdate();
                treeViewAdv.Invalidate();
            }

        } 

        private void NodeInsert(List<float> Usages, List<float> Fres)
        {
            RegistryKey MyReg = Registry.LocalMachine.OpenSubKey("HARDWARE\\DESCRIPTION\\SYSTEM\\CentralProcessor\\0");//获取注册表中CPU的信息 打开注册表键LocalMachine下的子键
            var cpuname =  MyReg.GetValue("ProcessorNameString").ToString();//读取子键中的信息
            Node temp = new Node(cpuname, string.Empty);
            Node usagevar = new Node("CPU Usages", string.Empty);
            Node frequencyvar = new Node("CPU Frequencys", string.Empty);
            temp.IsStillExist = true;
            uint i = 0;
            foreach (var usage in Usages)
            {
                Node u = new Node(string.Format("Core #{0:d} Usage", i++), usage.ToString("0.00") + "%");
                usagevar.Nodes.Add(u);
            }
            i = 0;
            foreach (var fre in Fres)
            {
                Node u = new Node(string.Format("Core #{0:d} Frequency", i++), fre.ToString("0000.00") + "MHz");
                frequencyvar.Nodes.Add(u);
            }

            temp.Nodes.Add(usagevar);
            temp.Nodes.Add(frequencyvar);
            _model.Nodes.Add(temp);
        }

        private void NodeUpdate()
        {
            info.Update();
            foreach (var node in _model.Nodes)
            {
               foreach(var subnode in node.Nodes)
               {
                    if (subnode.Text.Contains("Usage"))
                    {
                        var info_dis = info.Usages;
                        int i = 0;
                        foreach (var infonode in subnode.Nodes)
                        {
                            infonode.Data = info_dis[i++].ToString("0.00") + "%";
                        }
                    }
                    else
                    {
                        var info_dis = info.Frequencys;
                        int i = 0;
                        foreach (var infonode in subnode.Nodes)
                        {
                            infonode.Data = info_dis[i++].ToString("0000.00") + "MHz";
                        }
                    }

               }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            treeViewAdv.ExpandAll();
        }
    }
}
