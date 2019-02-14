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
    class MySqlRoutes
    {
        public static MySqlConnexion ConnectionBD { get; private set; }

        public static ObservableCollection<Route> Retrieve(Chemin c, string nom, string auteur)
        {

            StringBuilder query = new StringBuilder();
            if (c.Emplacement != null)
            {
                query.Append("SELECT * FROM Routes WHERE idChemin = (SELECT idChemin FROM Chemins WHERE idSimulation = (SELECT idSimulation FROM Simulations WHERE nom = '" + MySqlHelper.EscapeString(nom) + "' AND idUtilisateur = (SELECT idUtilisateur FROM Utilisateurs WHERE nomUtilisateur = '" + MySqlHelper.EscapeString(auteur) + "')) AND  posX = '" + (int)(c.Position.X) + "' AND posY = '" + (int)(c.Position.Y) + "' AND idEmplacement = '" + (int)c.Emplacement + "')");
            }
            else
            {
                query.Append("SELECT * FROM Routes WHERE idChemin = (SELECT idChemin FROM Chemins WHERE idSimulation = (SELECT idSimulation FROM Simulations WHERE nom = '" + MySqlHelper.EscapeString(nom) + "' AND idUtilisateur = (SELECT idUtilisateur FROM Utilisateurs WHERE nomUtilisateur = '" + MySqlHelper.EscapeString(auteur) + "')) AND  posX = '" + (int)(c.Position.X) + "' AND posY = '" + (int)(c.Position.Y) + "' AND idEmplacement IS NULL)");
            }

            return RetrieveListeRoutes(query.ToString(), nom, auteur, c);
        }

        public static ObservableCollection<Route> RetrieveListeRoutes(string query, string nom, string auteur, Chemin c)
        {
            ObservableCollection<Route> lstResultat = new ObservableCollection<Route>();
            DataSet dsResultat;
            DataTable dtResultat;

            ConnectionBD = new MySqlConnexion();

            dsResultat = ConnectionBD.Query(query);
            dtResultat = dsResultat.Tables[0];

            foreach (DataRow dr in dtResultat.Rows)
            {
                lstResultat.Add(new Route((Orientation)dr["idOrientation"])
                               );
            }

            foreach (Route r in lstResultat)
            {
                r.LstVoies = MySqlVoies.Retrieve(r, nom, auteur, c);
            }

            return lstResultat;
        }

        public static void Insert(Simulation simulation, Route r, Chemin c)
        {
            StringBuilder nonquery = new StringBuilder();
            ConnectionBD = new MySqlConnexion();
            Utilisateur u = MySqlUtilisateurs.Retrieve(simulation.Auteur);

            try
            {
                nonquery = new StringBuilder();

                nonquery.Append("INSERT INTO Routes (idChemin, idOrientation) VALUES ")
                                .Append("(").Append("(SELECT idChemin FROM Chemins WHERE idSimulation = (SELECT idSimulation FROM Simulations WHERE nom = '" + MySqlHelper.EscapeString(simulation.Nom) + "' AND idUtilisateur = (SELECT idUtilisateur FROM Utilisateurs WHERE nomUtilisateur = '" + MySqlHelper.EscapeString(simulation.Auteur) + "')) AND  posX = '" + (int)(c.Position.X) + "' AND posY = '" + (int)(c.Position.Y) + "')")
                                .Append(",'").Append((int)r.Orientation).Append("')");

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

        public static void InsertAvecIdIntersection(Simulation simulation, Route r, Chemin c, int posX, int posY)
        {
            StringBuilder nonquery = new StringBuilder();
            ConnectionBD = new MySqlConnexion();
            Utilisateur u = MySqlUtilisateurs.Retrieve(simulation.Auteur);

            try
            {
                nonquery = new StringBuilder();

                nonquery.Append("INSERT INTO Routes (idChemin, idOrientation) VALUES ")
                                .Append("(").Append("(SELECT idChemin FROM Chemins WHERE idSimulation = (SELECT idSimulation FROM Simulations WHERE nom = '" + MySqlHelper.EscapeString(simulation.Nom) + "' AND idUtilisateur = (SELECT idUtilisateur FROM Utilisateurs WHERE nomUtilisateur = '" + MySqlHelper.EscapeString(simulation.Auteur) + "')) AND idIntersection = (SELECT idIntersection FROM Intersections WHERE idSimulation = (SELECT idSimulation FROM Simulations WHERE nom = '" + MySqlHelper.EscapeString(simulation.Nom) + "' AND idUtilisateur = (SELECT idUtilisateur FROM Utilisateurs WHERE nomUtilisateur = '" + MySqlHelper.EscapeString(simulation.Auteur) + "')) AND posX = '" + posX + "' AND posY = '" + posY + "')  AND  posX = '" + (int)(c.Position.X) + "' AND posY = '" + (int)(c.Position.Y) + "' AND idEmplacement = '" + (int)(c.Emplacement) + "')")
                                .Append(",'").Append((int)r.Orientation).Append("')");

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
