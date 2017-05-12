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

        /*
        [OperationContract]
        string GetData(int value);
        
        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);
        */
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "tigli/origin={origin}&destination={destination}",
           RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string echoget(string origin, string destination);


        // TODO: ajoutez vos opérations de service ici
    }

    // Utilisez un contrat de données comme indiqué dans l'exemple ci-après pour ajouter les types composites aux opérations de service.
    // Vous pouvez ajouter des fichiers XSD au projet. Une fois le projet généré, vous pouvez utiliser directement les types de données qui y sont définis, avec l'espace de noms "ProjetTigli.ContractType".

        /*
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string origin_lat = "";
        string origin_long = "";
        string dest_lat = "";
        string dest_long = "";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string Origin_long
        {
            get { return origin_long; }
            set { origin_long = value; }
        }

        [DataMember]
        public string Origin_lat
        {
            get { return origin_lat; }
            set { origin_lat = value; }
        }

        [DataMember]
        public string Dest_long
        {
            get { return dest_long; }
            set { dest_long = value; }
        }

        [DataMember]
        public string Dest_lat
        {
            get { return dest_lat; }
            set { dest_lat = value; }
        }


    }*/
}
