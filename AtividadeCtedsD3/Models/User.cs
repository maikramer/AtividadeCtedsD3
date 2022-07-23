using AtividadeCtedsD3.Interfaces;
using Microsoft.Data.SqlClient;

namespace AtividadeCtedsD3.Models;

public class User
{
    public decimal IdUser { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}