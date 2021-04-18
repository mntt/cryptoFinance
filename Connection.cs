using System.Data.Linq;

namespace cryptoFinance
{
    public static class Connection
    {
        public static string connectionString = "connectionstring";

        public static DataContext db = new DataContext(connectionString);

        public static InteractionsWithDatabase iwdb = new InteractionsWithDatabase();
    }
}
