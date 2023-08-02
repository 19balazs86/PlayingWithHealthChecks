using Microsoft.EntityFrameworkCore;

namespace PlayingWithHealthChecks;

public sealed class DataBaseContext : DbContext
{
    public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
    {

    }
}
