﻿using LibreLancer;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Xwt;

namespace Launcher
{
    static class Program
    {
        public static LibreLancer.GameConfig Config;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.Initialize();
            Config = LibreLancer.GameConfig.Create();
            if (Environment.OSVersion.Platform != PlatformID.MacOSX &&
               Environment.OSVersion.Platform != PlatformID.Unix)
            {
                //Check WMP for video playback
                object legacyWMPCheck = Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\Software\Microsoft\Active Setup\Installed Components\{22d6f312-b0f6-11d0-94ab-0080c74c7e95}", "IsInstalled", null);
                if (legacyWMPCheck == null || legacyWMPCheck.ToString() != "1")
                {
                    new CrashWindow(
                        "Uh oh!",
                        "Missing Components",
                        new StreamReader(typeof(Program).Assembly.GetManifestResourceStream("Launcher.WMPMessage.txt")).ReadToEnd(),
                        true).Show();
                    Application.Run();
                }
                else
                {
                    ShowLauncher();
                }
            }
            else
            {
                ShowLauncher();
            }
        }
        //Show Launcher Directly
        //This makes sure Application.Run() is only called once
        static void ShowLauncher()
        {
            var win = new MainWindow(false);
            win.Show();
            Application.Run();
        }

        public static void Run()
        {
            Thread thread = new Thread(StartGame);
            thread.Name = "Main";
            thread.Start();
            Application.MainLoop.DispatchPendingEvents();
            thread.Join();
            if(ex != null)
            {
                new CrashWindow(ex).Show();
            }
            else
            {
                Application.Exit();
            }
        }

        [DllImport("kernel32.dll")]
        static extern bool SetDllDirectory(string directory);

        static Exception ex;
        static void StartGame()
        {
            try
            {
                Config.Save();

                if (Platform.RunningOS == OS.Windows)
                {
                    string bindir = Path.GetDirectoryName(typeof(GameConfig).Assembly.Location);
                    var fullpath = Path.Combine(bindir, IntPtr.Size == 8 ? "x64" : "x86");
                    SetDllDirectory(fullpath);
                }
                else
                    Config.ForceAngle = false;

                var game = new FreelancerGame(Config);
#if !DEBUG
                try
                {
#endif
                    game.Run();
#if !DEBUG
                }
                catch (Exception ex)
                {
                    game.Crashed();
                    Console.Out.WriteLine("Unhandled {0}: ", ex.GetType().Name);
                    Console.Out.WriteLine(ex.Message);
                    Console.Out.WriteLine(ex.StackTrace);
                    Program.ex = ex;
                }
#endif
            }
            catch (Exception xcpt)
            {
                Program.ex = xcpt;
            }
        }
    }
}
