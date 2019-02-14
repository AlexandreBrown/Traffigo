using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows;
using System.Collections.ObjectModel;
using TraffiGo.Modeles.Data;
using TraffiGo.Modeles.ClasseSql;

namespace TraffiGo.Modeles.ClasseSql
{
    class MySqlSimulations
    {
        private static MySqlConnexion ConnectionBD { get; set; }
        public static object Utilisateur { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Simulation> RetrieveAll()
        {
            List<Simulation> lstResultat = new List<Simulation>();
            List<Intersection> lstIntersections;
            List<Chemin> lstChemins;
            DataSet dsResultat;
            DataTable dtResultat;

            ConnectionBD = new MySqlConnexion();


            dsResultat = ConnectionBD.Query("SELECT s.dateCreation, s.nom, u.nomUtilisateur FROM Simulations AS s INNER JOIN Utilisateurs u ON s.idUtilisateur = u.idUtilisateur");
            dtResultat = dsResultat.Tables[0];

            foreach (DataRow dr in dtResultat.Rows)
            {
                lstResultat.Add(new Simulation((string)dr["nomUtilisateur"]
                                          , (DateTime)dr["dateCreation"]
                                          , (string)dr["nom"]
                                          )
                               );
            }

            foreach (Simulation s in lstResultat)
            {
                lstIntersections = MySqlIntersections.Retrieve(s);

                foreach (Intersection i in lstIntersections)
                {
                    s.AjouterIntersection(i.Position);
                }
            }

            foreach (Simulation s in lstResultat)
            {
                lstChemins = MySqlChemins.RetrieveSansInter(s);

                foreach (Chemin c in lstChemins)
                {
                    s.AjouterChemin(c.Position);
                }
            }
            return lstResultat;
        }


        public static List<Simulation> RetrievePartiellePublic()
        {
            List<Simulation> lstResultat = new List<Simulation>();
            DataSet dsResultat;
            DataTable dtResultat;

            ConnectionBD = new MySqlConnexion();


            dsResultat = ConnectionBD.Query("SELECT s.dateCreation, s.nom, u.nomUtilisateur FROM Simulations AS s INNER JOIN Utilisateurs u ON s.idUtilisateur = u.idUtilisateur WHERE isPublic = 1");
            dtResultat = dsResultat.Tables[0];

            foreach (DataRow dr in dtResultat.Rows)
            {
                lstResultat.Add(new Simulation((string)dr["nomUtilisateur"]
                                          , (DateTime)dr["dateCreation"]
                                          , MySqlHelper.EscapeString((string)dr["nom"])
                                          )
                               );
            }

            return lstResultat;
        }

        public static List<Simulation> RetrievePartiellePrive(string nomUtilisateur)
        {
            List<Simulation> lstResultat = new List<Simulation>();
            DataSet dsResultat;
            DataTable dtResultat;

            ConnectionBD = new MySqlConnexion();


            dsResultat = ConnectionBD.Query("SELECT s.dateCreation, s.nom, u.nomUtilisateur FROM Simulations AS s INNER JOIN Utilisateurs u ON s.idUtilisateur = u.idUtilisateur WHERE u.nomUtilisateur = '" + MySqlHelper.EscapeString(nomUtilisateur) + "'");
            dtResultat = dsResultat.Tables[0];

            foreach (DataRow dr in dtResultat.Rows)
            {
                lstResultat.Add(new Simulation((string)dr["nomUtilisateur"]
                                          , (DateTime)dr["dateCreation"]
                                          , (string)dr["nom"]
                                          )
                               );
            }

            return lstResultat;
        }

        public static List<Simulation> VerifNomSimulation(string nom, string auteur, bool estPublic)
        {
            List<Simulation> lstResultat = new List<Simulation>();
            DataSet dsResultat;
            DataTable dtResultat;

            ConnectionBD = new MySqlConnexion();
            if (estPublic == true)
            {
                dsResultat = ConnectionBD.Query("SELECT s.dateCreation, s.nom, u.nomUtilisateur FROM Simulations AS s INNER JOIN Utilisateurs u ON s.idUtilisateur = u.idUtilisateur WHERE s.nom = '" + MySqlHelper.EscapeString(nom.ToLower()) + "' AND s.isPublic = 1");
                dsResultat.Merge(ConnectionBD.Query("SELECT s.dateCreation, s.nom, u.nomUtilisateur FROM Simulations AS s INNER JOIN Utilisateurs u ON s.idUtilisateur = u.idUtilisateur WHERE s.nom = '" + MySqlHelper.EscapeString(nom.ToLower()) + "' AND u.nomUtilisateur = '" + MySqlHelper.EscapeString(auteur.ToLower()) + "'"));
                dtResultat = dsResultat.Tables[0];

                foreach (DataRow dr in dtResultat.Rows)
                {
                    lstResultat.Add(new Simulation((string)dr["nomUtilisateur"]
                                              , (DateTime)dr["dateCreation"]
                                              , (string)dr["nom"]
                                              )
                                   );
                }
            }
            else
            {
                dsResultat = ConnectionBD.Query("SELECT s.dateCreation, s.nom, u.nomUtilisateur FROM Simulations AS s INNER JOIN Utilisateurs u ON s.idUtilisateur = u.idUtilisateur WHERE s.nom = '" + MySqlHelper.EscapeString(nom.ToLower()) + "' AND u.nomUtilisateur = '" + MySqlHelper.EscapeString(auteur) + "'");
                dtResultat = dsResultat.Tables[0];

                foreach (DataRow dr in dtResultat.Rows)
                {
                    lstResultat.Add(new Simulation((string)dr["nomUtilisateur"]
                                              , (DateTime)dr["dateCreation"]
                                              , (string)dr["nom"]
                                              )
                                   );
                }
            }

            return lstResultat;
        }

        public static bool EstPublique(string nom, string auteur)
        {
            List<Simulation> lstResultat = new List<Simulation>();
            DataSet dsResultat;
            DataTable dtResultat;

            ConnectionBD = new MySqlConnexion();
            dsResultat = ConnectionBD.Query("SELECT s.dateCreation, s.nom, u.nomUtilisateur, s.isPublic FROM Simulations AS s INNER JOIN Utilisateurs u ON s.idUtilisateur = u.idUtilisateur WHERE s.nom = '" + MySqlHelper.EscapeString(nom.ToLower()) + "' AND s.isPublic = 1");
            dtResultat = dsResultat.Tables[0];

            if (dtResultat.Rows.Count != 0)
            {
                return true;
            }
            return false;

        }

        public static ObservableCollection<Simulation> RetrievePublic()
        {
            ObservableCollection<Simulation> lstResultat = new ObservableCollection<Simulation>();
            List<Intersection> lstIntersections;
            List<Chemin> lstChemins;
            DataSet dsResultat;
            DataTable dtResultat;

            ConnectionBD = new MySqlConnexion();


            dsResultat = ConnectionBD.Query("SELECT s.dateCreation, s.nom, u.nomUtilisateur FROM Simulations AS s INNER JOIN Utilisateurs u ON s.idUtilisateur = u.idUtilisateur WHERE isPublic = 1");
            dtResultat = dsResultat.Tables[0];

            foreach (DataRow dr in dtResultat.Rows)
            {
                lstResultat.Add(new Simulation((string)dr["nomUtilisateur"]
                                          , (DateTime)dr["dateCreation"]
                                          , (string)dr["nom"]
                                          )
                               );
            }

            foreach (Simulation s in lstResultat)
            {
                lstIntersections = MySqlIntersections.Retrieve(s);

                foreach (Intersection i in lstIntersections)
                {
                    s.AjouterIntersection(i);
                }

                lstChemins = MySqlChemins.RetrieveSansInter(s);

                foreach (Chemin c in lstChemins)
                {
                    s.AjouterChemin(c);
                }
            }

            return lstResultat;
        }

        public static ObservableCollection<Simulation> RetrievePrive(string nomUtilisateur)
        {
            ObservableCollection<Simulation> lstResultat = new ObservableCollection<Simulation>();
            List<Intersection> lstIntersections;
            List<Chemin> lstChemins;
            DataSet dsResultat;
            DataTable dtResultat;

            ConnectionBD = new MySqlConnexion();


            dsResultat = ConnectionBD.Query("SELECT s.dateCreation, s.nom, u.nomUtilisateur FROM Simulations AS s INNER JOIN Utilisateurs u ON s.idUtilisateur = u.idUtilisateur WHERE u.nomUtilisateur = '" + MySqlHelper.EscapeString(nomUtilisateur) + "'");
            dtResultat = dsResultat.Tables[0];

            foreach (DataRow dr in dtResultat.Rows)
            {
                lstResultat.Add(new Simulation((string)dr["nomUtilisateur"]
                                          , (DateTime)dr["dateCreation"]
                                          , (string)dr["nom"]
                                          )
                               );
            }

            foreach (Simulation s in lstResultat)
            {
                lstIntersections = MySqlIntersections.Retrieve(s);

                foreach (Intersection i in lstIntersections)
                {
                    s.AjouterIntersection(i);
                }

                lstChemins = MySqlChemins.RetrieveSansInter(s);

                foreach (Chemin c in lstChemins)
                {
                    s.AjouterChemin(c);
                }
            }

            return lstResultat;
        }


        public static Simulation Retrieve(string nom)
        {
            List<Intersection> lstIntersections;
            List<Chemin> lstChemins;
            DataSet dsResultat;
            DataTable dtResultat;
            DataRow dtRow;
            Simulation resultat;

            ConnectionBD = new MySqlConnexion();


            try
            {
                dsResultat = ConnectionBD.Query("SELECT s.dateCreation, s.nom, u.nomUtilisateur FROM Simulations s INNER JOIN Utilisateurs u ON s.idUtilisateur = u.idUtilisateur WHERE s.nom = '" + MySqlHelper.EscapeString(nom) + "';");

                dtResultat = dsResultat.Tables[0];
                dtRow = dtResultat.Rows[0];
                resultat = new Simulation((string)dtRow["nomUtilisateur"]
                                        , (DateTime)dtRow["dateCreation"]
                                        , (string)dtRow["nom"]
                                        );
            }

            catch (MySqlException e)
            {
                if (e.ErrorCode == 1046)
                {
                    throw new Exception("Connexion au serveur impossible, veuillez réessayer plus tard.");
                }
                resultat = null;
            }
            catch (Exception)
            {
                resultat = null;
                throw new Exception("Une erreur est survenue, veuillez réessayer plus tard.");

            }

            lstIntersections = MySqlIntersections.Retrieve(resultat);

            foreach (Intersection i in lstIntersections)
            {
                resultat.AjouterIntersection(i);
            }

            lstChemins = MySqlChemins.RetrieveSansInter(resultat);

            foreach (Chemin c in lstChemins)
            {
                resultat.AjouterChemin(c);
            }

            return resultat;
        }

        public static void Rename(string acienNom, string nom)
        {
            StringBuilder nonquery = new StringBuilder();
            ConnectionBD = new MySqlConnexion();

            try
            {
                nonquery.Append("UPDATE Simulations SET nom = '" + MySqlHelper.EscapeString(nom) + "' WHERE nom = '" + MySqlHelper.EscapeString(acienNom) + "';");
                ConnectionBD.NonQuery(nonquery.ToString());
            }
            catch (MySqlException)
            {
                throw;
            }
        }

        public static void Delete(Simulation simulation)
        {
            StringBuilder nonquery = new StringBuilder();
            ConnectionBD = new MySqlConnexion();

            try
            {
                nonquery.Append("Delete s FROM " +
                    "Simulations S " + 
                    "WHERE S.nom = '" + MySqlHelper.EscapeString(simulation.Nom) + "';");
                ConnectionBD.NonQuery(nonquery.ToString());
            }
            catch (MySqlException)
            {
                throw;
            }
        }

        public static void Delete(string nom)
        {
            StringBuilder nonquery = new StringBuilder();
            ConnectionBD = new MySqlConnexion();

            try
            {
                nonquery.Append("Delete s FROM " +
                    "Simulations S " +
                    "WHERE S.nom = '" + MySqlHelper.EscapeString(nom) + "';");
                ConnectionBD.NonQuery(nonquery.ToString());
            }
            catch (MySqlException)
            {
                throw;
            }
        }

        public static void InsertSimple(Simulation simulation, int Public)
        {
            StringBuilder nonquery = new StringBuilder();
            ConnectionBD = new MySqlConnexion();
            Utilisateur u = MySqlUtilisateurs.Retrieve(simulation.Auteur);

            try
            {
                nonquery = new StringBuilder();

                nonquery.Append("INSERT INTO Simulations (idUtilisateur, dateCreation, nom, isPublic) VALUES ")
                        .Append("(").Append("(SELECT idUtilisateur FROM Utilisateurs WHERE nomUtilisateur = '" + MySqlHelper.EscapeString(simulation.Auteur) + "')")
                        .Append(",'").Append(simulation.DateCreation).Append("'")
                        .Append(",'").Append(MySqlHelper.EscapeString(simulation.Nom)).Append("'")
                        .Append(",'").Append(Public).Append("')");

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

        public static void InsertComplet(Simulation simulation, bool isPublic)
        {
            int posXI = 0;
            int posYI = 0;
            int Public = 0;

            if (isPublic == true)
                Public = 1;
            else
                Public = 0;
            try
            {
                InsertSimple(simulation, Public);

                foreach (var cases in simulation.LstElem)
                {
                    foreach (Element c in cases)
                    {
                        if (c is Intersection)
                        {
                            posXI = (int)(c.Position.X);
                            posYI = (int)(c.Position.Y);

                            MySqlIntersections.Insert(simulation, posXI, posYI);

                            foreach (Chemin ch in ((Intersection)c).LstChemins)
                            {
                                MySqlChemins.InsertAvecEmplacement(simulation, ch, posXI, posYI);

                                foreach (var r in ch.LstRoutes)
                                {
                                    MySqlRoutes.InsertAvecIdIntersection(simulation, r, ch, posXI, posYI);

                                    foreach (var v in r.LstVoies)
                                    {
                                        MySqlVoies.InsertAvecIntersection(simulation, r, ch, posXI, posYI);

                                        foreach (var dv in v.LstDirections)
                                        {
                                            MySqlDirectionsVoies.InsertAvecIntersection(simulation, r, ch, dv, posXI, posYI);
                                        }
                                    }
                                }
                            }

                            foreach (var f in ((Intersection)c).LstFeux)
                            {
                                MySqlFeux.Insert(simulation, f, posXI, posYI);

                                foreach (var d in f.DirectionsControlees)
                                {
                                    MySqlDirectionsFeux.Insert(simulation, f, d, posXI, posYI);
                                }                          
                            }
                        }
                        if (c is Chemin)
                        {
                            MySqlChemins.InsertSansIntersection(simulation, (Chemin)c, posXI, posYI);

                            foreach (var r in ((Chemin)c).LstRoutes)
                            {
                                MySqlRoutes.Insert(simulation, r, (Chemin)c);

                                foreach (var v in r.LstVoies)
                                {
                                    MySqlVoies.InsertSansIntersection(simulation, r, (Chemin)c);
                                    
                                    foreach (var dv in v.LstDirections)
                                    {
                                        MySqlDirectionsVoies.InsertSansIntersection(simulation, r, (Chemin)c, dv);
                                    }
                                }
                            }
                        }
                    }
                }
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
