using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Domain.Models;

namespace Data.Context
{
    public class FileTransferContext : IdentityDbContext
    {
        public FileTransferContext(DbContextOptions<FileTransferContext> options)
            : base(options)
        { }

        public DbSet<Box> Boxes { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
