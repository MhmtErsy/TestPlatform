using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP.DataAccessLayer;
using TP.DataAccessLayer.EntityFramwork;
using TP.Entities;

namespace TP.BusinessLayer
{
    public class Test
    {
        Repository<Tester> repo_tester = new Repository<Tester>();
        Repository<Test_Master> repo_testmaster = new Repository<Test_Master>();

        public Test()
        {
           DatabaseContext db = new DatabaseContext();
           db.Admins.ToList();
        }
        public void UpdateTest()
        {
            
        }

        public void DeleteTest()
        {
            
        }
    }
}
