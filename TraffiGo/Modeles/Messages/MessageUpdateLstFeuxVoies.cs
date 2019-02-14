using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TraffiGo.Modeles.Messages
{
    public class MessageUpdateLstFeuxVoies
    {
        public ObservableCollection<FeuGraphique> NouvelleListFeux { get; set; }
    }
}
