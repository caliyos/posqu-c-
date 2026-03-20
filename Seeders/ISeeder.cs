using Npgsql;

namespace POS_qu.Seeders
{
    public interface ISeeder
    {
        string Id { get; }
        void Run(NpgsqlConnection conn, NpgsqlTransaction tran);
    }
}
