using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TraffiGo.Modeles
{
    public static class ExtensionMethods
    {
        // Utilisation : Ajouter [Serializable] en haut de la classe que vous voulez cloner
        /// <summary>
        /// Fonction qui fait une copie profonde d'un objet T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <returns></returns>
        public static T DeepClone<T>(this T a)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, a);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
