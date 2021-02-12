using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ServiceProcess;
using System.Diagnostics;
using System.Threading;

namespace KepSerwerReboot
{
    class Program
    {
        static void Main(string[] args)
        {
            string textFromFile = "";
            string textFromFiletIME = "";
            using (FileStream fstream = File.OpenRead($"config.txt"))
            {
                byte[] array = new byte[fstream.Length];
                // считываем данные
                fstream.Read(array, 0, array.Length);

                textFromFile = System.Text.Encoding.Default.GetString(array);
            }

            using (FileStream fstream = File.OpenRead($"configtIME.txt"))
            {
                byte[] array = new byte[fstream.Length];
                // считываем данные
                fstream.Read(array, 0, array.Length);

                textFromFiletIME = System.Text.Encoding.Default.GetString(array);
            }

            ServiceController controller = new ServiceController(textFromFile);
            Console.WriteLine("Программа по перезапуску службы");
            while (true)
            {
                try
                {
                    if (controller.Status == ServiceControllerStatus.Running)
                    {
                        controller.Stop();
                        controller.WaitForStatus(ServiceControllerStatus.Stopped);
                        controller.Start();
                        controller.WaitForStatus(ServiceControllerStatus.Running);
                        Console.WriteLine("Служба перезапущена");
                    }

                    if (controller.Status == ServiceControllerStatus.Stopped)
                    {
                        controller.Start();
                        controller.WaitForStatus(ServiceControllerStatus.Running);
                        Console.WriteLine("Служба перезапущена");
                    }

                    //Thread.Sleep(10000);
                    Thread.Sleep(Convert.ToInt32(textFromFiletIME));
                    Console.WriteLine("Прошло 1,5 часа перезагрузка");
                }
                catch(Exception ex)
                {

                }

            }

        }

        public string StatusService(ServiceController controller)
        {
            switch (controller.Status)
            {
                case ServiceControllerStatus.ContinuePending:
                    return "Continue Pending";
                case ServiceControllerStatus.Paused:
                    return "Paused";
                case ServiceControllerStatus.PausePending:
                    return "Pause Pending";
                case ServiceControllerStatus.StartPending:
                    return "Start Pending";
                case ServiceControllerStatus.Running:
                    return "Running";
                case ServiceControllerStatus.Stopped:
                    return "Stopped";
                case ServiceControllerStatus.StopPending:
                    return "Stop Pending";
                default:
                    return "Unknown status";
            }
        }

    }
}
