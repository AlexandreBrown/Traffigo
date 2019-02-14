using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows;
using TraffiGo.Modeles.Data;

namespace TraffiGo.Modeles.ClasseSql
{
    class MySqlVehicules
    {
        private static MySqlConnexion ConnectionBD { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static List<Vehicule> RetrieveAll()
        {
            List<Vehicule> lstResultat = new List<Vehicule>();
            DataSet dsResultat;
            DataTable dtResultat;

            ConnectionBD = new MySqlConnexion();


            dsResultat = ConnectionBD.Query("SELECT * From Vehicules");
            dtResultat = dsResultat.Tables[0];

            foreach (DataRow dr in dtResultat.Rows)
            {
                lstResultat.Add(new Vehicule((string)dr["nom"]
                                          , (int)dr["acceleration"]
                                          , (int)dr["decceleration"]
                                          , (new Point((int)dr["posX"], (int)dr["posY"]))
                                          )
                               );
            }
            return lstResultat;
        }


        public static List<Vehicule> Retrieve(Simulation simulation)
        {

            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM Vehicules WHERE nom = ").Append(simulation.Nom);

            return RetrieveListeVehicules(query.ToString());
        }

        public static List<Vehicule> RetrieveListeVehicules(string query)
        {
            List<Vehicule> lstResultat = new List<Vehicule>();
            DataSet dsResultat;
            DataTable dtResultat;

            ConnectionBD = new MySqlConnexion();

            dsResultat = ConnectionBD.Query(query);
            dtResultat = dsResultat.Tables[0];

            foreach (DataRow dr in dtResultat.Rows)
            {
                lstResultat.Add(new Vehicule((string)dr["nom"]
                                          , (int)dr["acceleration"]
                                          , (int)dr["decceleration"]
                                          , (new Point((int)dr["posX"], (int)dr["posY"]))
                                          )
                               );
            }

            return lstResultat;
        }
    }
}
