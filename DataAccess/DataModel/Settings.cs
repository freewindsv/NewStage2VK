using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.DataAccess.DataModel
{
    /// <summary>
    /// Класс настроек для приложения
    /// </summary>
    public class Settings : IDbEntity
    {
        /// <summary>
        /// Идентификатор (ключ) в БД
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Текущий оператор приложения
        /// </summary>
        public User CurrentUser { get; set; }
    }
}
