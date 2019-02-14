using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Text;
using System.Windows;
using TraffiGo.Modeles.Data;

namespace TraffiGo.Modeles.ClasseSql
{
    class MySqlIntersections
    {
        private static MySqlConnexion ConnectionBD { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static List<Intersection> RetrieveAll()
        {
            List<Intersection> lstResultat = new List<Intersection>();
            DataSet dsResultat;
            DataTable dtResultat;

            ConnectionBD = new MySqlConnexion();


            dsResultat = ConnectionBD.Query("SELECT * From Intersections");
            dtResultat = dsResultat.Tables[0];

            foreach (DataRow dr in dtResultat.Rows)
            {
                lstResultat.Add(new Intersection(new Point((int)dr["posX"], (int)dr["posY"]))
                               );
            }
            return lstResultat;
        }


        public static List<Intersection> Retrieve(Simulation simulation)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT * FROM Intersections WHERE idSimulation = (SELECT idSimulation FROM Simulations WHERE nom = '" + MySqlHelper.EscapeString(simulation.Nom) + "' AND idUtilisateur = (SELECT idUtilisateur FROM Utilisateurs WHERE nomUtilisateur = '" + MySqlHelper.EscapeString(simulation.Auteur) + "'))");


            return RetrieveListeIntersections(query.ToString(), simulation);
        }

        public static List<Intersection> RetrieveListeIntersections(string query, Simulation s)
        {
            List<Intersection> lstResultat = new List<Intersection>();
            DataSet dsResultat;
            DataTable dtResultat;

            ConnectionBD = new MySqlConnexion();

            dsResultat = ConnectionBD.Query(query);
            dtResultat = dsResultat.Tables[0];

            foreach (DataRow dr in dtResultat.Rows)
            {
                lstResultat.Add(new Intersection(new Point((int)dr["posX"], (int)dr["posY"]))
                               );
            }

            foreach (Intersection i in lstResultat)
            {
                i.LstFeux = MySqlFeux.Retrieve(i, s.Nom, s.Auteur);
                i.LstChemins = new ObservableCollection<Chemin>(MySqlChemins.Retrieve(s, i));
            }

            return lstResultat;
        }

        public static void Insert(Simulation simulation, int posX, int posY)
        {
            StringBuilder nonquery = new StringBuilder();
            ConnectionBD = new MySqlConnexion();
            Utilisateur u = MySqlUtilisateurs.Retrieve(simulation.Auteur);

            try
            {
                nonquery = new StringBuilder();

                nonquery.Append("INSERT INTO Intersections (idSimulation, posX, posY) VALUES ")
                    .Append("(").Append("(SELECT idSimulation FROM Simulations WHERE nom = '" + MySqlHelper.EscapeString(simulation.Nom) + "' AND idUtilisateur = (SELECT idUtilisateur FROM Utilisateurs WHERE nomUtilisateur = '" + MySqlHelper.EscapeString(simulation.Auteur) + "'))")
                    .Append(",'").Append(posX).Append("'")
                    .Append(",'").Append(posY).Append("')");

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
