using System.Windows;
using TraffiGo.Modeles.Data;

namespace TraffiGo.Modeles.Messages
{
    public class MessageSaveChanges
    {
        public Simulation Simulation { get; set; }
        public Point CaseSelectionnee { get; set; }
    }
}
