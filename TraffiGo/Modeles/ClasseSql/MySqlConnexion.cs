using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using GalaSoft.MvvmLight.Messaging;
using TraffiGo.Modeles.Messages.ChangementVue;
using TraffiGo.VueModeles;
using System.Windows;

namespace TraffiGo
{
    /// <summary>
    ///
    /// 
    /// </summary>
    public class MySqlConnexion
    {
        private MySqlConnection Connection { get; set; }
        private MySqlTransaction Transaction { get; set; }


        public MySqlConnexion()
        {
            try
            {
                string connexionString;
                connexionString =
                ConfigurationManager.ConnectionStrings["MySqlConnexion"].ConnectionString;
                Connection = new MySqlConnection(connexionString);
            }
            catch (Exception)
            {
                throw new Exception("Une erreur est survenue, vérifiez le fichier de configuration de la base de données.");
            }
        }

        /// <summary>
        ///
        ///
        /// </summary>
        /// <returns></returns>
        private bool Open()
        {
            Connection.Open();

            return true;
        }

        /// <summary>
        ///
        ///
        /// </summary>
        /// <returns></returns>
        public bool OpenWithTransaction()
        {
            if (Open())
            {
                Transaction = Connection.BeginTransaction();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///
        ///
        ///
        /// </summary>
        private void Close()
        {
            Connection.Close();
        }

        /// <summary>
        ///
        ///
        /// </summary>
        public void Commit()
        {
            Transaction.Commit();
            Transaction = null;
            Connection.Close();
        }

        /// <summary>
        ///
        ///
        /// </summary>
        public void Rollback()
        {
            Transaction.Rollback();
            Transaction = null;
            Connection.Close();
        }

        /// <summary>
        ///
        ///
        /// </summary>
        /// <param name="nonquery"></param>
        /// <returns></returns>
        public int NonQuery(string nonquery)
        {
            int nbResultat = 0;
            try
            {
                if (Open() || Connection.State == ConnectionState.Open)
                {
                    MySqlCommand command = new MySqlCommand(nonquery, Connection);
                    nbResultat = command.ExecuteNonQuery();
                }

                return nbResultat;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Transaction == null)
                {
                    Close();
                }
            }
        }


        public DataSet Query(string query)
        {

            DataSet dataset = new DataSet();

            try
            {
                if (Open() || Transaction != null)
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter();
                    adapter.SelectCommand = new MySqlCommand(query, Connection);
                    adapter.Fill(dataset);
                }
                return dataset;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Transaction == null)
                {
                    Close();
                }
            }

        }


        public DataSet StoredProcedure(string query, IList<MySqlParameter> parameters = null)
        {
            DataSet dataset = new DataSet();

            try
            {
                if (Open() || Transaction != null)
                {

                    MySqlCommand commande = new MySqlCommand(query, Connection);
                    commande.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        commande.Parameters.AddRange(parameters.ToArray());
                    }
                    MySqlDataAdapter adapter = new MySqlDataAdapter();
                    adapter.SelectCommand = commande;
                    adapter.Fill(dataset);
                }
                return dataset;

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Transaction == null)
                {
                    Close();
                }
            }
        }
    }
}

