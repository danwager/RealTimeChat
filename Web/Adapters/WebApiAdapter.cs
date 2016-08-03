using Common.Models;
using RestSharp;
using System;
using System.Configuration;
using RestSharp.Deserializers;


namespace Web.Adapters
{
    public interface IWepApiAdapter
    {
        ChatModel NewUser(User user);
        void UserLoggedOut(User user);
        void NewMessage(Message message);
    }

    public class WebApiAdapter : IWepApiAdapter
    {
        public WebApiAdapter()
        {
        }

        public void NewMessage(Message message)
        {
            RestRequest newMessage = new RestRequest("NewMessage", Method.POST);

            newMessage.AddJsonBody(message);

            Execute(newMessage);
        }

        public ChatModel NewUser(User user)
        {
            RestRequest addNewUser = new RestRequest("NewUser", Method.POST);

            addNewUser.AddJsonBody(user);
      
            return Execute<ChatModel>(addNewUser);
    }

        public void UserLoggedOut(User user)
        {
            RestRequest logoutUser = new RestRequest("UserLoggedOut", Method.POST);

            logoutUser.AddJsonBody(user);

            Execute(logoutUser);
        }

    private void Execute(RestRequest request)
        {
            var client = new RestClient();
            client.BaseUrl = new Uri(ConfigurationManager.AppSettings["ServiceUrl"]);

            var response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
              client.Execute(request);
            }
        }

        private T Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient();
            client.BaseUrl = new Uri(ConfigurationManager.AppSettings["ServiceUrl"]);

            var response = client.Execute<T>(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception(response.ErrorMessage);
            }

            return response.Data;
        }
    }
}