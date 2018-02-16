using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using TP.Entities;

namespace TP.DataAccessLayer.EntityFramwork
{
    public class MyInitializer:CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            Rank r1 = new Rank()
            {
                rank="Çaylak",
                requiredMinScore=0
            };
            context.Ranks.Add(r1);

            Rank r2 = new Rank()
            {
                rank = "Tecrübeli",
                requiredMinScore = 250
            };
            context.Ranks.Add(r2);

            Rank r3 = new Rank()
            {
                rank = "Uzman",
                requiredMinScore = 500
            };
            context.Ranks.Add(r3);

            context.SaveChanges();
                    

        }
    }
}
