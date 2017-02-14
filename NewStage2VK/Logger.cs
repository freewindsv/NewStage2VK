using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK
{
    /// <summary>
    /// Класс логирования (запись ошибок и данных отладки) в файл
    /// </summary>
    public class FileLogger : ILogger
    {
        private string filePath;

        public FileLogger(string filePath)
        {
            this.filePath = filePath;
        }

        /// <summary>
        /// Записать строку в файл журнала
        /// </summary>
        /// <param name="text">Текст сообщения</param>
        public void WriteLine(string text)
        {
            lock (this)
            {
                StreamWriter sw = null;
                try
                {
                    sw = new StreamWriter(filePath, true, Encoding.UTF8);
                    sw.WriteLine(DateTime.Now.ToString() + ": " + text);
                }
                catch
                {
                    //
                }
                finally
                {
                    if (sw != null)
                    {
                        sw.Dispose();
                        sw = null;
                    }
                }
            }
        }

        /// <summary>
        /// Записать объект исключения в файл
        /// </summary>
        /// <param name="ex">Объект исключения</param>
        public void WriteException(Exception ex)
        {
            string text = "EXCEPTION: ";
            while (ex != null)
            {
                text += "\r\n  " + ex.Message;
                ex = ex.InnerException;
            }
            WriteLine(text);
        }
    }
}
