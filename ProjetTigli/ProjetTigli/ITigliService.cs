using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ProjetTigli
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IService1" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface ITigliService
    {

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "tigli/origin={origin}&destination={destination}",
           RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string GetItinerary(string origin, string destination);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "coord/address={address}",
           RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string GetCoords(string address);
        
    }
    
}
