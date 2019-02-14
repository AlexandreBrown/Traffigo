using System.Collections.Generic;

namespace TraffiGo.Modeles.Messages
{
    public class MessageModifyDirections
    {
        // Destiné à une voie sélectionnée
        public List<Direction> nouvellesDirections { get; set; }
    }
}
