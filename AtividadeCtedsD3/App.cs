using System.Diagnostics;
using AtividadeCtedsD3.Controllers;
using AtividadeCtedsD3.Helpers;
using AtividadeCtedsD3.Models;
using Spectre.Console;

namespace AtividadeCtedsD3;

public class App
{
    private const string DataSource = "SQLEXPRESS";
    private const string User = "maikeu";
    private const string Password = "1234";
    private const string Database = "ATIVIDADE";
    private const string LogarNoSistema = "Logar no Sistema";
    private const string SairDoSistema = "Sair";
    private const string DeslogarNoSistema = "Deslogar";
    private const string LogsDoSistema = "Mostrar Logs";
    private const string LogFile = nameof(LogFile);
    private readonly UserReader _userReader = new(DataSource, User, Password, Database);
    private readonly SelectionPrompt<string> _selectionPrompt = new();
    private List<User> Users { get; set; } = new();
    private User? _loggedUser;

    public void Init()
    {
        Users = _userReader.ReadAll();
        _selectionPrompt.AddChoice(LogarNoSistema);
        _selectionPrompt.AddChoice(SairDoSistema);
    }

    public void Run()
    {
        for (;;)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Panel("[yellow]Bem vindo a nossa Atividade[/]"));
            var option = AnsiConsole.Prompt(_selectionPrompt);
            var close = false;
            var loginResult = false;
            switch (option)
            {
                case LogarNoSistema:
                    loginResult = Login();
                    break;
                case SairDoSistema:
                    close = true;
                    break;
            }

            if (loginResult)
            {
                close = EnterProgram();
            }

            if (!close) continue;
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("\n[blue]Pressione qualquer tecla para sair...[/]");
            Console.ReadKey();
            break;
        }
    }

    private bool EnterProgram()
    {
        Debug.Assert(_loggedUser != null, nameof(_loggedUser) + " != null");
        var loginInfo = new LoginInfo(EventType.Login, DateTime.Now, _loggedUser.Name);
        JsonLogger<LoginInfo>.WriteToLocalFile(loginInfo, LogFile);
        var selection = new SelectionPrompt<string>();
        selection.AddChoice(DeslogarNoSistema);
        selection.AddChoice(LogsDoSistema);
        selection.AddChoice(SairDoSistema);
        string option;
        for (;;)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new Panel("[yellow]Bem Vindo ao Programa[/]")
                {
                    Header = new PanelHeader("Aproveite")
                });
            option = AnsiConsole.Prompt(selection);
            if (option == LogsDoSistema)
            {
                ShowLogs();
            } else
                break;
        }

        loginInfo = new LoginInfo(EventType.Logoff, DateTime.Now, _loggedUser.Name);
        JsonLogger<LoginInfo>.WriteToLocalFile(loginInfo, LogFile);
        return option == SairDoSistema;
    }

    private void ShowLogs()
    {
        var list = JsonLogger<LoginInfo>.ReadFromLocalFile(LogFile);

        var table = new Table().RoundedBorder()
            .AddColumns("[purple]Data[/]", "[purple]Tipo[/]", "[purple]Usuario[/]");
        foreach (var info in list)
        {
            table.AddRow($"{info.Time:dd/MM/yyyy HH:mm:ss}", info.EventType.ToString(),
                info.UserName);
        }

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("\n[blue]Pressione qualquer tecla para voltar...[/]");
        Console.ReadKey();
    }

    private bool Login()
    {
        string? email;
        for (;;)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Panel(
                "[blue]Digite um E-Mail Válido ou[/] [purple]sair[/] [blue]para sair[/]"));
            email = AnsiConsole.Ask<string>("[yellow]E-Mail:[/] ");
            if (email is "sair" or "Sair") return false;
            if (!Validator.ValidateEmail(email))
            {
                AnsiConsole.MarkupLine("[red]Formato de E-Mail Inválido[/]");
                Task.Delay(1200).Wait();
            } else
            {
                //Email Válido
                break;
            }
        }

        var password = AnsiConsole.Ask<string>("[yellow]Senha: [/]");

        foreach (var user in Users)
        {
            if (user.Email != email || user.Password != password) continue;
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new Panel("[green]Sucesso no Login[/]") { Header = new PanelHeader("Sucesso!!") });
            Task.Delay(1000).Wait();
            _loggedUser = user;
            return true;
        }

        AnsiConsole.MarkupLine("[red]E-Mail ou Senha Inválidos[/]");
        AnsiConsole.MarkupLine("\n[blue]Pressione qualquer tecla para voltar ao Menu[/]");
        Console.ReadKey();

        return false;
    }
}