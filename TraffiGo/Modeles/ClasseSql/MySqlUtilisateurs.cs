using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
using TraffiGo.Modeles.Data;

namespace TraffiGo.Modeles.ClasseSql
{
    public class MySqlUtilisateurs
    {
        private static MySqlConnexion ConnectionBD { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Utilisateur> RetrieveAll()
        {
            List<Utilisateur> lstResultat = new List<Utilisateur>();
            DataSet dsResultat;
            DataTable dtResultat;

            ConnectionBD = new MySqlConnexion();

            dsResultat = ConnectionBD.Query("SELECT * FROM Utilisateurs");
            dtResultat = dsResultat.Tables[0];

            foreach (DataRow dr in dtResultat.Rows)
            {
                lstResultat.Add(new Utilisateur((string)dr["prenom"]
                                          , (string)dr["nom"]
                                          , (string)dr["nomUtilisateur"]
                                          , (string)dr["courriel"]
                                          , (string)dr["motPasse"]
                                          , (string)dr["salt"]
                                          )
                               );
            }
            return lstResultat;
        }


        public static Utilisateur Retrieve(string nomUtilisateur)
        {
            
            DataSet dsResultat;
            DataTable dtResultat;
            DataRow dtRow;
            Utilisateur Resultat;

            ConnectionBD = new MySqlConnexion();
            try
            {
                // On trouve l'utilisateur qui est demandé par l'utilisateur
                dsResultat = ConnectionBD.Query("SELECT * FROM Utilisateurs WHERE nomUtilisateur = '" + MySqlHelper.EscapeString(nomUtilisateur) + "';");

                dtResultat = dsResultat.Tables[0];
                dtRow = dtResultat.Rows[0];
                
                //on crée un vrai utilisateur à partir de ce qu'on a reçu de la BD
                Resultat = new Utilisateur((string)dtRow["prenom"]
                                        , (string)dtRow["nom"]
                                        , (string)dtRow["nomUtilisateur"]
                                        , (string)dtRow["motDePasse"]
                                        , (string)dtRow["salt"]
                                        , (string)dtRow["courriel"]
                                        );
            } catch (MySqlException)
            {
                //on ne peut pas se rendre à la BD
                throw new Exception("Connexion au serveur impossible , veuillez réessayer plus tard. ");
            } catch (Exception)
            {
                //on a rien trouvé en BD ( seule erreur possible )
                Resultat = null;

            }
            return Resultat;
        }

        public static Utilisateur RetrieveWithCourriel(string courriel)
        {

            DataSet dsResultat;
            DataTable dtResultat;
            DataRow dtRow;
            Utilisateur Resultat;

            ConnectionBD = new MySqlConnexion();
            try
            {
                // On trouve l'utilisateur qui est demandé par l'utilisateur
                dsResultat = ConnectionBD.Query("SELECT * FROM Utilisateurs WHERE courriel = '" + courriel + "';");

                dtResultat = dsResultat.Tables[0];
                dtRow = dtResultat.Rows[0];

                //on crée un vrai utilisateur à partir de ce qu'on a reçu de la BD
                Resultat = new Utilisateur((string)dtRow["prenom"]
                                        , (string)dtRow["nom"]
                                        , (string)dtRow["nomUtilisateur"]
                                        , (string)dtRow["motDePasse"]
                                        , (string)dtRow["salt"]
                                        , (string)dtRow["courriel"]
                                        );
            }
            catch (MySqlException)
            {
                //on ne peut pas se rendre à la BD
                throw new Exception("Connexion au serveur impossible , veuillez réessayer plus tard. ");
            }
            catch (Exception)
            {
                //on a rien trouvé en BD ( seule erreur possible )
                Resultat = null;

            }
            return Resultat;
        }

        public static void Insert(Utilisateur utilisateur)
        {
            StringBuilder nonquery = new StringBuilder();
            ConnectionBD = new MySqlConnexion();

            try
            {
                nonquery.Append("INSERT INTO Utilisateurs (prenom, nom, nomUtilisateur, motDePasse, salt, courriel) VALUES ")
                        .Append("('").Append(MySqlHelper.EscapeString(utilisateur.Prenom)).Append("'")
                        .Append(",'").Append(MySqlHelper.EscapeString(utilisateur.Nom)).Append("'")
                        .Append(",'").Append(MySqlHelper.EscapeString(utilisateur.NomUtilisateur)).Append("'")
                        .Append(",'").Append(MySqlHelper.EscapeString(utilisateur.MotPasse)).Append("'")
                        .Append(",'").Append(utilisateur.Salt).Append("'")
                        .Append(",'").Append(utilisateur.Courriel).Append("')");

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
                    case 1044:
                        error = new Exception("Les identifiants utilisés pour la connexion au serveur ne sont pas valides");
                        break;
                    case 1062:
                        if (e.Message.Contains(utilisateur.NomUtilisateur))
                        {
                            error= new Exception("Cet identifiant est déjà utilisé. ");
                        }
                        else if (e.Message.Contains(utilisateur.Courriel))
                        {
                            error = new Exception("Ce courriel est déjà utilisé. ");

                        }
                        else
                        {
                            error = new Exception("Impossible de créer le compte. ");

                        }
                        break;
                    default:
                        error = new Exception("Connexion au serveur impossible , veuillez réessayer plus tard. "); ;
                        break;
                }

                throw error;
            }
        }

        public static void ChangePassword(Utilisateur utilisateur, string newPassword)
        {
            StringBuilder nonquery = new StringBuilder();
            ConnectionBD = new MySqlConnexion();

            try
            {
                nonquery.Append("UPDATE Utilisateurs SET motDePasse = '" + MySqlHelper.EscapeString(newPassword) + "' WHERE nomUtilisateur = '" + MySqlHelper.EscapeString(utilisateur.NomUtilisateur) + "';");
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
                    default:
                        error = new Exception("Connexion au serveur impossible , veuillez réessayer plus tard. "); ;
                        break;
                }

                throw error;
            }
        }

        public static bool CourrielExistant(string courriel)
        {
            List<Utilisateur> lstResultat = new List<Utilisateur>();
            DataSet dsResultat;
            DataTable dtResultat;

            ConnectionBD = new MySqlConnexion();
            dsResultat = ConnectionBD.Query("SELECT * FROM Utilisateurs WHERE courriel = '" + courriel + "'");
            dtResultat = dsResultat.Tables[0];

            if (dtResultat.Rows.Count !=0)
            {
                return true;
            }
                return false;
        }
    }
}
