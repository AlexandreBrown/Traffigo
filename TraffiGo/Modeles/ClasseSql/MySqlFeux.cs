using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraffiGo.Modeles.Data;

namespace TraffiGo.Modeles.ClasseSql
{
    class MySqlFeux
    {
        public static MySqlConnexion ConnectionBD { get; private set; }

        public static ObservableCollection<Feu> Retrieve(Intersection i, string nom, string auteur)
        {

            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM Feux WHERE idIntersection = (SELECT idIntersection FROM Intersections WHERE idSimulation = (SELECT idSimulation FROM Simulations WHERE nom = '" + MySqlHelper.EscapeString(nom) + "' AND idUtilisateur = (SELECT idUtilisateur FROM Utilisateurs WHERE nomUtilisateur = '" + MySqlHelper.EscapeString(auteur) + "')) AND posX = '" + (int)(i.Position.X) + "' AND posY = '" + (int)(i.Position.Y) + "')");


            return RetrieveListeFeux(query.ToString(), nom, auteur, i);
        }

        public static ObservableCollection<Feu> RetrieveListeFeux(string query, string nom, string auteur, Intersection i)
        {
            ObservableCollection<Feu> lstResultat = new ObservableCollection<Feu>();
            DataSet dsResultat;
            DataTable dtResultat;

            ConnectionBD = new MySqlConnexion();

            dsResultat = ConnectionBD.Query(query);
            dtResultat = dsResultat.Tables[0];

            foreach (DataRow dr in dtResultat.Rows)
            {
                lstResultat.Add(new Feu((CouleurFeu)dr["couleur"], (int)dr["tempsVert"], (int)dr["tempsJaune"], (int)dr["cycle"], (Orientation)dr["idOrientation"])
                               );
            }

            foreach (Feu f in lstResultat)
            {
                MySqlDirectionsFeux.Retrieve(f, nom, auteur, i);
            }

            return lstResultat;
        }
        public static void Insert(Simulation simulation, Feu f, int posX, int posY)
        {
            StringBuilder nonquery = new StringBuilder();
            ConnectionBD = new MySqlConnexion();
            Utilisateur u = MySqlUtilisateurs.Retrieve(simulation.Auteur);

            try
            {
                nonquery = new StringBuilder();

                nonquery.Append("INSERT INTO Feux (idIntersection, idOrientation, couleur, tempsVert, tempsJaune, cycle) VALUES ")
                   .Append("(").Append("(SELECT idIntersection FROM Intersections WHERE idSimulation = (SELECT idSimulation FROM Simulations WHERE nom = '" + MySqlHelper.EscapeString(simulation.Nom) + "' AND idUtilisateur = (SELECT idUtilisateur FROM Utilisateurs WHERE nomUtilisateur = '" + MySqlHelper.EscapeString(simulation.Auteur) + "')) AND posX = '" + posX + "' AND posY = '" + posY + "')")
                   .Append(",'").Append((int)(f.OrientationControlee)).Append("'")
                   .Append(",'").Append((int)(f.Couleur)).Append("'")
                   .Append(",'").Append(f.TempsVert).Append("'")
                   .Append(",'").Append((int)(f.TempsJaune)).Append("'")
                   .Append(",'").Append(f.Cycle).Append("')");

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
