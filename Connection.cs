using System.Data.Linq;

namespace cryptoFinance
{
    public static class Connection
    {
        private static string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;

        public static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + (System.IO.Path.GetDirectoryName(executable)) + "\\CryptoDatabase.mdf;Integrated Security = True";

        public static DataContext db = new DataContext(connectionString);

        public static InteractionsWithDatabase iwdb = new InteractionsWithDatabase();
    }
}
