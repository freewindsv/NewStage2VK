using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewStage2VK.ViewModel
{
    /// <summary>
    /// Аргументы события окончания загрузки страницы браузером
    /// </summary>
    public class PageLoadEventArgs : EventArgs
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="uri">Uri страницы</param>
        public PageLoadEventArgs(Uri uri)
        {
            this.Uri = uri;
        }

        /// <summary>
        /// Uri страницы
        /// </summary>
        public Uri Uri { get; private set; }
    }
}
