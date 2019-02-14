using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows;
using TraffiGo.Modeles.Data;

namespace TraffiGo.Modeles.ClasseSql
{
    class MySqlChemins
    {
        private static MySqlConnexion ConnectionBD { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static List<Chemin> RetrieveAll()
        {
            List<Chemin> lstResultat = new List<Chemin>();
            DataSet dsResultat;
            DataTable dtResultat;

            ConnectionBD = new MySqlConnexion();


            dsResultat = ConnectionBD.Query("SELECT * From Chemins");
            dtResultat = dsResultat.Tables[0];

            foreach (DataRow dr in dtResultat.Rows)
            {
                lstResultat.Add(new Chemin(new Point((int)dr["posX"], (int)dr["posY"]))
                               );
            }
            return lstResultat;
        }


        public static List<Chemin> Retrieve(Simulation simulation, Intersection i)
        {

            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM Chemins WHERE idSimulation = (SELECT idSimulation FROM Simulations WHERE nom = '" + MySqlHelper.EscapeString(simulation.Nom) + "' AND idUtilisateur = (SELECT idUtilisateur FROM Utilisateurs WHERE nomUtilisateur = '" + MySqlHelper.EscapeString(simulation.Auteur) + "')) AND idIntersection = (SELECT idIntersection FROM Intersections WHERE idSimulation = (SELECT idSimulation FROM Simulations WHERE nom = '" + MySqlHelper.EscapeString(simulation.Nom) + "' AND idUtilisateur = (SELECT idUtilisateur FROM Utilisateurs WHERE nomUtilisateur = '" + MySqlHelper.EscapeString(simulation.Auteur) + "')) AND posX = '" + (int)(i.Position.X) + "' AND posY = '" + (int)(i.Position.Y) + "')");

            return RetrieveListeChemins(query.ToString(), simulation);
        }

        public static List<Chemin> RetrieveListeChemins(string query, Simulation s)
        {
            List<Chemin> lstResultat = new List<Chemin>();
            DataSet dsResultat;
            DataTable dtResultat;

            ConnectionBD = new MySqlConnexion();

            dsResultat = ConnectionBD.Query(query);
            dtResultat = dsResultat.Tables[0];

            foreach (DataRow dr in dtResultat.Rows)
            {
                lstResultat.Add(new Chemin(new Point((int)dr["posX"], (int)dr["posY"]), (Orientation)dr["idEmplacement"])
                               );
            }

            foreach (Chemin c in lstResultat)
            {
                c.LstRoutes = MySqlRoutes.Retrieve(c, s.Nom, s.Auteur);
            }

            return lstResultat;
        }

        public static List<Chemin> RetrieveSansInter(Simulation simulation)
        {

            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM Chemins WHERE idSimulation = (SELECT idSimulation FROM Simulations WHERE nom = '" + MySqlHelper.EscapeString(simulation.Nom) + "' AND idUtilisateur = (SELECT idUtilisateur FROM Utilisateurs WHERE nomUtilisateur = '" + MySqlHelper.EscapeString(simulation.Auteur) + "')) AND idIntersection IS NULL");

            return RetrieveListeCheminsSansInter(query.ToString(), simulation);
        }

        public static List<Chemin> RetrieveListeCheminsSansInter(string query, Simulation s)
        {
            List<Chemin> lstResultat = new List<Chemin>();
            DataSet dsResultat;
            DataTable dtResultat;

            ConnectionBD = new MySqlConnexion();

            dsResultat = ConnectionBD.Query(query);
            dtResultat = dsResultat.Tables[0];

            foreach (DataRow dr in dtResultat.Rows)
            {
                lstResultat.Add(new Chemin(new Point((int)dr["posX"], (int)dr["posY"]))
                               );
            }

            foreach (Chemin c in lstResultat)
            {
                c.LstRoutes = MySqlRoutes.Retrieve(c, s.Nom, s.Auteur);
            }

            return lstResultat;
        }

        public static void InsertAvecEmplacement(Simulation simulation, Chemin c, int posX, int posY)
        {
            StringBuilder nonquery = new StringBuilder();
            ConnectionBD = new MySqlConnexion();
            Utilisateur u = MySqlUtilisateurs.Retrieve(simulation.Auteur);

            try
            {
                nonquery = new StringBuilder();

                nonquery.Append("INSERT INTO Chemins (idSimulation, idIntersection, idEmplacement, posX, posY) VALUES ")
                               .Append("(").Append("(SELECT idSimulation FROM Simulations WHERE nom = '" + MySqlHelper.EscapeString(simulation.Nom) + "' AND idUtilisateur = (SELECT idUtilisateur FROM Utilisateurs WHERE nomUtilisateur = '" + MySqlHelper.EscapeString(simulation.Auteur) + "'))")
                               .Append(",").Append("(SELECT idIntersection FROM Intersections WHERE idSimulation = (SELECT idSimulation FROM Simulations WHERE nom = '" + MySqlHelper.EscapeString(simulation.Nom) + "' AND idUtilisateur = (SELECT idUtilisateur FROM Utilisateurs WHERE nomUtilisateur = '" + MySqlHelper.EscapeString(simulation.Auteur) + "')) AND posX = '" + posX + "' AND posY = '" + posY + "')")
                               .Append(",'").Append((int)(c.Emplacement)).Append("'")
                               .Append(",'").Append((int)(c.Position.X)).Append("'")
                               .Append(",'").Append((int)c.Position.Y).Append("')");

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

        public static void InsertSansIntersection(Simulation simulation, Chemin c, int posX, int posY)
        {
            StringBuilder nonquery = new StringBuilder();
            ConnectionBD = new MySqlConnexion();
            Utilisateur u = MySqlUtilisateurs.Retrieve(simulation.Auteur);

            try
            {
                nonquery = new StringBuilder();

                nonquery.Append("INSERT INTO Chemins (idSimulation, posX, posY) VALUES ")
                                .Append("(").Append("(SELECT idSimulation FROM Simulations WHERE nom = '" + MySqlHelper.EscapeString(simulation.Nom) + "' AND idUtilisateur = (SELECT idUtilisateur FROM Utilisateurs WHERE nomUtilisateur = '" + MySqlHelper.EscapeString(simulation.Auteur) + "'))")
                                .Append(",'").Append((int)(c.Position.X)).Append("'")
                                .Append(",'").Append((int)c.Position.Y).Append("')");

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

        public static void InsertAvecIntersection(Simulation simulation, Chemin c, int posX, int posY)
        {
            StringBuilder nonquery = new StringBuilder();
            ConnectionBD = new MySqlConnexion();
            Utilisateur u = MySqlUtilisateurs.Retrieve(simulation.Auteur);

            try
            {
                nonquery = new StringBuilder();

                nonquery.Append("INSERT INTO Chemins (idSimulation, idIntersection, posX, posY) VALUES ")
                                  .Append("(").Append("(SELECT idSimulation FROM Simulations WHERE nom = '" + MySqlHelper.EscapeString(simulation.Nom) + "' AND idUtilisateur = (SELECT idUtilisateur FROM Utilisateurs WHERE nomUtilisateur = '" + MySqlHelper.EscapeString(simulation.Auteur) + "'))")
                                  .Append(",").Append("(SELECT idIntersection FROM Intersections WHERE idSimulation = (SELECT idSimulation FROM Simulations WHERE nom = '" + MySqlHelper.EscapeString(simulation.Nom) + "' AND idUtilisateur = (SELECT idUtilisateur FROM Utilisateurs WHERE nomUtilisateur = '" + MySqlHelper.EscapeString(simulation.Auteur) + "')) AND posX = '" + posX + "' AND posY = '" + posY + "')")
                                  .Append(",'").Append((int)(c.Position.X)).Append("'")
                                  .Append(",'").Append((int)c.Position.Y).Append("')");

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
