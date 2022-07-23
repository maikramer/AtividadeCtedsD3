namespace AtividadeCtedsD3.Interfaces;

public interface IReader<T>
{
    public List<T> ReadAll();
}