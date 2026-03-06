using System;
using System.Collections.Generic;
using System.Text;

namespace Hospital.Infrastructure.Logging
{
    public static class Logger
    {
        private static readonly string filePath = "errorlog.txt";

        public static void Log(Exception ex)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("--------------------------------------------------");
                    writer.WriteLine($"Date: {DateTime.Now}");
                    writer.WriteLine($"Message: {ex.Message}");
                    writer.WriteLine($"StackTrace: {ex.StackTrace}");
                    writer.WriteLine("--------------------------------------------------");
                    writer.WriteLine();
                }
            }
            catch
            {
                // If logging fails, we do nothing
                // Because logging should never crash the app
            }

        }
    }
}
