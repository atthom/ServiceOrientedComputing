﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClientIntranet.VelibService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="VelibService.IVelibService")]
    public interface IVelibService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IVelibService/GetItinerary", ReplyAction="http://tempuri.org/IVelibService/GetItineraryResponse")]
        string GetItinerary(string origin, string destination);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IVelibService/GetItinerary", ReplyAction="http://tempuri.org/IVelibService/GetItineraryResponse")]
        System.Threading.Tasks.Task<string> GetItineraryAsync(string origin, string destination);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IVelibService/GetCoords", ReplyAction="http://tempuri.org/IVelibService/GetCoordsResponse")]
        string GetCoords(string address);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IVelibService/GetCoords", ReplyAction="http://tempuri.org/IVelibService/GetCoordsResponse")]
        System.Threading.Tasks.Task<string> GetCoordsAsync(string address);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IVelibServiceChannel : ClientIntranet.VelibService.IVelibService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class VelibServiceClient : System.ServiceModel.ClientBase<ClientIntranet.VelibService.IVelibService>, ClientIntranet.VelibService.IVelibService {
        
        public VelibServiceClient() {
        }
        
        public VelibServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public VelibServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public VelibServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public VelibServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string GetItinerary(string origin, string destination) {
            return base.Channel.GetItinerary(origin, destination);
        }
        
        public System.Threading.Tasks.Task<string> GetItineraryAsync(string origin, string destination) {
            return base.Channel.GetItineraryAsync(origin, destination);
        }
        
        public string GetCoords(string address) {
            return base.Channel.GetCoords(address);
        }
        
        public System.Threading.Tasks.Task<string> GetCoordsAsync(string address) {
            return base.Channel.GetCoordsAsync(address);
        }
    }
}
