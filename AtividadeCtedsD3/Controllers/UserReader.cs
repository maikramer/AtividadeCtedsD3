using AtividadeCtedsD3.Interfaces;
using AtividadeCtedsD3.Models;
using Microsoft.Data.SqlClient;

namespace AtividadeCtedsD3.Controllers;

public class UserReader : IReader<User>
{
    private const string TableName = "Users";
    private string ConnectionString { get; }

    public UserReader(string source, string user, string password, string database)
    {
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = Environment.MachineName + '\\' + source,
            Authentication = SqlAuthenticationMethod.SqlPassword,
            UserID = user,
            Password = password,
            InitialCatalog = database,
            IntegratedSecurity = false,
            TrustServerCertificate = true
        };

        ConnectionString = builder.ToString();
    }

    public List<User> ReadAll()
    {
        List<User> users = new();

        using var con = new SqlConnection(ConnectionString);
        const string querySelectAll =
            $"SELECT {nameof(User.IdUser)},{nameof(User.Name)},{nameof(User.Email)},{nameof(User.Password)} " +
            $"FROM {TableName}";
        con.Open();

        using SqlCommand cmd = new(querySelectAll, con);
        var rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
            User user = new()
            {
                IdUser = (decimal)rdr[nameof(User.IdUser)],
                Name = rdr[nameof(User.Name)].ToString() ?? string.Empty,
                Email = rdr[nameof(User.Email)].ToString() ?? string.Empty,
                Password = rdr[nameof(User.Password)].ToString() ?? string.Empty,
            };

            users.Add(user);
        }

        return users;
    }
}