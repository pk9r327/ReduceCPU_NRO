using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ReduceCPU_NRO
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll")]
        static extern int SetWindowText(IntPtr hWnd, string text);

        List<Process> Processes = new List<Process>();

        public Form1()
        {
            InitializeComponent();
            LoadProcesses();
            comboBox1.SelectedIndex = 0;
        }

        private void LoadProcesses()
        {
            progressBar1.Value = 0;
            Processes.Clear();
            comboBox1.Items.Clear();
            var proc = Process.GetProcesses();
            for (int i = 0; i < proc.Length; i++)
            {
                if (checkBox1.Checked || IsWindow(proc[i].MainWindowHandle))
                {
                    Processes.Add(proc[i]);
                    comboBox1.Items.Add(string.Format("{0} - {1} - {2}", proc[i].ProcessName, proc[i].MainWindowTitle, proc[i].Id));
                }
            }
            progressBar1.Value = 100;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoadProcesses();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                Process process = Processes[comboBox1.SelectedIndex];
                if (!process.HasExited)
                {
                    SetParent(process.MainWindowHandle, Handle);
                    SetWindowText(process.MainWindowHandle, textBox1.Text);
                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra");
                }
                LoadProcesses();
            }    
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Process process = Processes[comboBox1.SelectedIndex];
            textBox1.Text = process.MainWindowTitle;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/pk9rprotein/ReduceCPU_NRO/blob/main/README.md");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            LoadProcesses();
        }
    }
}
