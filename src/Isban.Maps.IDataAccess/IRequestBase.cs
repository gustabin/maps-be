namespace Isban.Maps.IDataAccess
{
    public interface IRequestBase
    {  
        /// <summary>
       /// Gets or sets the codigo error.
       /// </summary>
       /// <value>
       /// The codigo error.
       /// </value>
        long? CodigoError { get; set; }
        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>

        string Error { get; set; }
        /// <summary>
        /// Gets or sets the ip.
        /// </summary>
        /// <value>
        /// The ip.
        /// </value>

        string Ip { get; set; }
        /// <summary>
        /// Gets or sets the usuario.
        /// </summary>
        /// <value>
        /// The usuario.
        /// </value>        
        string Usuario { get; set; }

        /// <summary>
        /// Método para verificar si existen errores en los contenidos
        /// </summary>
        void CheckError();
    }
}
