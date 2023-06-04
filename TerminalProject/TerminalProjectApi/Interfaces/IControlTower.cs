using TerminalProjectApi.Database;
using TerminalProjectApi.Models;

namespace TerminalProjectApi.Interfaces
{
    public interface IControlTower
    {
        Task MoveFlight(Flight f);
    }
}
