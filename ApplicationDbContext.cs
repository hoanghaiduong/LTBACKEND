﻿using Microsoft.EntityFrameworkCore;

namespace LTBACKEND
{
    public class ApplicationDbContext : DbContext
    {
        
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
