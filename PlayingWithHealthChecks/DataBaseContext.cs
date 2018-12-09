using Microsoft.EntityFrameworkCore;

namespace PlayingWithHealthChecks
{
  public class DataBaseContext : DbContext
  {
    public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
    {

    }
  }
}
