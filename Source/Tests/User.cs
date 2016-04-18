using BLToolkit.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TableName("Users")]
    public class User
    {
        public User()
        {
            Id = Guid.NewGuid();
        }

        [PrimaryKey]
        public Guid Id { get; set; }

        public String Name { get; set; }

        public String Phone { get; set; }
    }
}
