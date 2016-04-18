using BLToolkit.Data.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class CRUDTests:TestBase
    {
        [SetUp]
        public void CreateTables()
        {
            using (var context = base.CreateContext())
            {
                context.SetCommand(Scripts.Scripts.CreateTablesCRUD).ExecuteNonQuery();
            }
        }

        [Test]
        public void TestInsert()
        {
            using (var context = CreateContext())
            {
                var user = new User
                {
                    Name = "Test",
                    Phone = "000"
                };
                context.Insert(user);
                Assert.AreEqual(1, context.GetTable<User>().Count());
            }
        }

        [Test]
        public void TestBatchInsert()
        {
            using (var context = CreateContext())
            {
                var users = new List<User>();
                for(var i=0;i<100;i++)
                users.Add(new User
                {
                    Name = "Test"+i,
                    Phone = "00"+i
                });
                context.InsertBatch(users);
                Assert.AreEqual(100, context.GetTable<User>().Count());
            }
        }

        [Test]
        public void TestBatchInsertTransaction()
        {
            using (var context = CreateContext())
            {
                using (var ts = context.BeginTransaction())
                {
                    var users = new List<User>();
                    context.GetTable<User>().Delete();
                    for (var i = 0; i < 100; i++)
                        users.Add(new User
                        {
                            Name = "Test" + i,
                            Phone = "00" + i
                        });
                    context.InsertBatch(users);
                    Assert.AreEqual(100, context.GetTable<User>().Count());
                    ts.Rollback();
                    Assert.AreEqual(0, context.GetTable<User>().Count());
                }
            }
        }
    }
}
