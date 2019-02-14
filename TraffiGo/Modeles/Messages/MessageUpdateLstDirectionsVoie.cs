using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TraffiGo.Modeles.Messages
{
    public class MessageUpdateLstDirectionsVoie
    {
        public List<Direction> LstDirectionsPossibles { get; set; }
        public List<Direction> LstDirectionsActuelles { get; set; }
    }
}
