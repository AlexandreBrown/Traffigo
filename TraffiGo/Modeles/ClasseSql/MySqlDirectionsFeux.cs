using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraffiGo.Modeles.Data;

namespace TraffiGo.Modeles.ClasseSql
{
    class MySqlDirectionsFeux
    {
        public static MySqlConnexion ConnectionBD { get; private set; }

        public static List<Direction> Retrieve(Feu f, string nom, string auteur, Intersection i)
        {

            StringBuilder query = new StringBuilder();
            query.Append("SELECT idDirection FROM DirectionsFeux WHERE idFeu = (SELECT idFeu FROM Feux WHERE idIntersection = (SELECT idIntersection FROM Intersections WHERE idSimulation = (SELECT idSimulation FROM Simulations WHERE nom = '" + MySqlHelper.EscapeString(nom) + "' AND idUtilisateur = (SELECT idUtilisateur FROM Utilisateurs WHERE nomUtilisateur = '" + MySqlHelper.EscapeString(auteur) + "') AND posX = '" + i.Position.X + "' AND posY = '" + i.Position.Y + "') AND cycle = '" + f.Cycle + "' AND idOrientation = '" + (int)(f.OrientationControlee) + "'))");


            return RetrieveListeDirectionsFeux(query.ToString(), nom, auteur, f);
        }

        public static List<Direction> RetrieveListeDirectionsFeux(string query, string nom, string auteur, Feu f)
        {
            List<Direction> lstResultat = new List<Direction>();
            DataSet dsResultat;
            DataTable dtResultat;

            ConnectionBD = new MySqlConnexion();

            dsResultat = ConnectionBD.Query(query);
            dtResultat = dsResultat.Tables[0];

            foreach (DataRow dr in dtResultat.Rows)
            {
                lstResultat.Add((Direction)dr["idDirection"]
                               );
            }

            foreach (Direction d in lstResultat)
            {
                f.AjouterDirection(d);
            }

            return lstResultat;
        }

        public static void Insert(Simulation simulation, Feu f, Direction d, int posX, int posY)
        {
            StringBuilder nonquery = new StringBuilder();
            ConnectionBD = new MySqlConnexion();
            Utilisateur u = MySqlUtilisateurs.Retrieve(simulation.Auteur);

            try
            {
                nonquery = new StringBuilder();

                nonquery.Append("INSERT INTO DirectionsFeux (idFeu, idDirection) VALUES ")
               .Append("(").Append("(SELECT idFeu FROM Feux WHERE idIntersection = (SELECT idIntersection FROM Intersections WHERE idSimulation = (SELECT idSimulation FROM Simulations WHERE nom = '" + MySqlHelper.EscapeString(simulation.Nom) + "' AND idUtilisateur = (SELECT idUtilisateur FROM Utilisateurs WHERE nomUtilisateur = '" + MySqlHelper.EscapeString(simulation.Auteur) + "') AND posX = '" + posX + "' AND posY = '" + posY + "') AND cycle = '" + f.Cycle + "' AND idOrientation = '" + (int)(f.OrientationControlee) + "'))")
               .Append(",'").Append((int)d).Append("')");

                ConnectionBD.NonQuery(nonquery.ToString());
            }
            catch (MySqlException e)
            {
                Exception error;
                switch (e.Number)
                {
                    case 1040:
                        error = new Exception("Le serveur reçoit trop de connexions simultanées, veuillez réessayer dans quelques instants. ");
                        break;
                    case 1062:
                        if (e.Message.Contains(simulation.Nom))
                        {
                            error = new Exception("Ce nom de simulation est déjà utilisé. ");
                        }
                        else
                        {
                            error = new Exception("Impossible de sauvegarder la simulation. ");

                        }
                        break;
                    default:
                        error = new Exception("Connexion au serveur impossible , veuillez réessayer plus tard. "); ;
                        break;
                }

                throw error;
            }
        }
    }
}
