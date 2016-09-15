using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

/**
 * Basic principles:
 * http://www.codeproject.com/Articles/571813/A-Beginners-Tutorial-on-Creating-WCF-REST-Services
 * 
 * format as JSON:
 * http://www.codeproject.com/Articles/167159/How-to-create-a-JSON-WCF-RESTful-Service-in-sec
 */
namespace ccms
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        [WebGet(UriTemplate = "test/{data}",ResponseFormat=WebMessageFormat.Json)]
        Login GetLogin(string data);
    }



    
    public class Service1:IService1
    {
        
        
        public Login GetLogin(string data)
        {
            Login l = new Login();
            l.data = data;
            return l;
        }

    }

    public class Login
    {
        public string data{get;set;}
    }
}
