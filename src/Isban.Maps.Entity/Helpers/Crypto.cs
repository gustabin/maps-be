using System.Security.Cryptography;
using System.Text;

namespace Isban.Maps.Entity.Helpers
{
    /// <summary>
    /// Clase dedicada a la generación de palabras encriptadas. 
    /// </summary>
    public static class Crypto
    {
        /// <summary>
        /// Obtiene el código hash desde una series de cadenas pasadas como argumento.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static int ObtenerHashDesdeString(params string[] args)
        {
            string valor = string.Concat(args);

            return valor.GetHashCode();
        }

        /// <summary>
        /// Obtiene una clave de hexadecimal a partir de una cadena dada.
        /// </summary>
        /// <param name="cadena"></param>
        /// <returns>Clave en Hexadecimal</returns>
        public static string ObtenerClaveSHA1(string cadena)
        {

            UTF8Encoding enc = new UTF8Encoding();
            byte[] data = enc.GetBytes(cadena);
            byte[] result;

            SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();

            result = sha.ComputeHash(data);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                // Convertimos los valores en hexadecimal
                // cuando tiene una cifra hay que rellenarlo con cero
                // para que siempre ocupen dos dígitos.
                if (result[i] < 16)
                {
                    sb.Append("0");
                }
                sb.Append(result[i].ToString("x"));
            }

            //Devolvemos la cadena con el hash en mayúsculas para que quede más chuli 🙂
            return sb.ToString().ToUpper();
        }

        public static string ObtenerMD5(string cadena)
        {
            string hash = string.Empty;

            using (MD5 md5Hash = MD5.Create())
            {

                UTF8Encoding enc = new UTF8Encoding();
                byte[] data = enc.GetBytes(cadena);
                byte[] result;

                result = md5Hash.ComputeHash(data);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < result.Length; i++)
                {
                    // Convertimos los valores en hexadecimal
                    // cuando tiene una cifra hay que rellenarlo con cero
                    // para que siempre ocupen dos dígitos.
                    if (result[i] < 16)
                    {
                        sb.Append("0");
                    }
                    sb.Append(result[i].ToString("x"));
                }

                //Devolvemos la cadena con el hash en mayúsculas para que quede más chuli 🙂
                return sb.ToString().ToUpper();
            }



        }
    }
}
