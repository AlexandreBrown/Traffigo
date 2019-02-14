using System;
using System.Text;
using System.Security.Cryptography;
using TraffiGo.Modeles.ClasseSql;

namespace TraffiGo.Modeles.Data
{
    public class Utilisateur
   {
      public string Prenom { get; set; }
      public string Nom { get; set; }
      public string NomUtilisateur { get; set; }
      public string Courriel { get; set; }
      public string MotPasse { get; set; }
      public string Salt { get; set; }

      public Utilisateur(string prenom, string nom, string nomUtilisateur, string motPasse, string courriel)
      {
         this.Prenom = prenom;
         this.Nom = nom;
         this.NomUtilisateur = nomUtilisateur;
         this.Courriel = courriel;
         CreerSalt();
         this.MotPasse = HashMDP(motPasse);
      }
      public Utilisateur(string prenom, string nom, string username, string password, string salt, string courriel)
      {
         this.Prenom = prenom;
         this.Nom = nom;
         this.NomUtilisateur = username;
         this.Courriel = courriel;
         this.MotPasse = password;
         this.Salt = salt;
      }

        public Utilisateur(string username, string mdp)
        {
            this.NomUtilisateur = username;
            this.MotPasse = mdp;
        }

        /// <summary>
        /// hash le mot de passe reçu en paramètre et le compare à celui de l'utilisateur
        /// </summary>
        /// <param name="mdp">Le mot de passe à checker</param>
        /// <returns>vrai si c'est le même, faux sinon</returns>
        public bool CheckPassword(string mdp)
        {
            return HashMDP(mdp) == this.MotPasse;
        } 

        public void ModifierMotPasse(string newPassword)
        {
            MySqlUtilisateurs.ChangePassword(this, HashMDP(newPassword));
            MotPasse = HashMDP(newPassword);
        }

      /// <summary>
      /// Hash un mot de passe recu en paramètre avec le salt de l'utilisateur.
      /// </summary>
      /// <param name="mdp">Le mot de passe a hasher</param>
      /// <returns>le mot de passe hashé</returns>
      private string HashMDP(string mdp)
      {
         SHA512 shaM = new SHA512Managed();
         string mdpSalt = mdp + this.Salt;
         byte[] data = new byte[mdpSalt.Length];
         byte[] result;

         data = Encoding.UTF8.GetBytes(mdpSalt);
         result = shaM.ComputeHash(data);
         return Convert.ToBase64String(result);
      }

      /// <summary>
      /// Permet de créer le Salt de l'utilisateur. 
      /// Ne crée le salt qu'une seule fois.
      /// </summary>
      private void CreerSalt()
      {
         if (this.Salt != null)
            return;

         //on remplis un tableau d'octets avec des nombres aléatoires
         var random = new RNGCryptoServiceProvider();

         byte[] salt = new byte[32];
         random.GetNonZeroBytes(salt);

         //on transforme ce tableau d'octets en string, puis on le met dans le Salt
         this.Salt = Convert.ToBase64String(salt);
        }

    
      public void Connexion()
      {

            Utilisateur utilisateurBD = null;
            try
            {
                utilisateurBD = MySqlUtilisateurs.Retrieve(NomUtilisateur);

            }catch(Exception e)
            {
                throw e;

            }
            
            if (utilisateurBD == null)
            {
                throw new Exception("Le compte n'a pas été trouvé. ");
            }

            this.Salt = utilisateurBD.Salt;

            this.MotPasse = HashMDP(this.MotPasse);
            //si le mot de passe reçu est valide, on ajoute les champs de l'utilisateur BD dans nous-même
            if (this.MotPasse == utilisateurBD.MotPasse)
            {
                this.Prenom = utilisateurBD.Prenom;
                this.Nom = utilisateurBD.Nom;
                this.NomUtilisateur = utilisateurBD.NomUtilisateur;

            }
            else
            {
                //le mot de passe n'est pas valide
                throw new Exception("Le mot de passe n'est pas valide. ");
            }
      }
        /// <summary>
        /// Charge la simulation que l'utilisateur veut utiliser
        /// </summary>
        /// <param name="nomSimulation">le nom de la simulation à charger</param>
        public void ChargerSimulation(string nomSimulation)
        {

        }

        public void ChargerSimulations()
        {

        }
   }
}
