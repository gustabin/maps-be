using Newtonsoft.Json;

namespace Isban.Maps.Entity.Base
{
    public interface IError
    {       
        int Error { get; set; }
               
        string Error_desc { get; set; }
       
        string Error_tecnico { get; set; }     
    }
}
