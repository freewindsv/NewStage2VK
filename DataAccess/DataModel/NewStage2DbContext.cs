using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewStage2VK.DataAccess.DataModel
{
    /// <summary>
    /// Контекст доступа к БД
    /// </summary>
    internal class NewStage2DbContext : DbContext
    {
        //static NewStage2DbContext()
        //{
        //    // Проблема первого запроса в EF:
        //    // Это отключит действия, предпринимаемые EF для инициализации соединения с БД.
        //    // Если вы используете подход Database First - эта строчка будет необходимой и достаточной.
        //    Database.SetInitializer<NewStage2DbContext>(null);
        //}

        public NewStage2DbContext()
            : base("name=NewStage2DbConnection")
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<Profile> Profile { get; set; }
        public DbSet<EventMessage> EventMessage { get; set; }
        public DbSet<ProfileMessage> ProfileMessage { get; set; }
        public DbSet<Message> Message { get; set; }
    }
}
