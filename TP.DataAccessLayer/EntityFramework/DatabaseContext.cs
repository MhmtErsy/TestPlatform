using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP.Entities;

namespace TP.DataAccessLayer.EntityFramwork
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Tester> Testers { get; set; }
        public DbSet<Test_Master> TestMasters { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Rank> Ranks { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Job_Adv> JobAdverts { get; set; }
        public DbSet<Test_Job> TestJobs { get; set; }
        public DbSet<Job_Ans> JobAnswers { get; set; }
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Model> Models  { get; set; }
        public DbSet <Browser> Browsers{ get; set; }
        public DbSet<OS> OS { get; set; }
        public DbSet<Entities.Type> Types { get; set; }

        public DatabaseContext()
        {
            Database.SetInitializer(new MyInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
          
        }
    }
}
