using ArmAPI.Model;
using ArmAPI.Rabbit;
using Nancy;
using Nancy.ModelBinding;
using Newtonsoft.Json;

namespace ArmAPI
{
    public sealed class MessageBrokerModule : NancyModule
    {
        public MessageBrokerModule()
        {
            Get("/", args => "OK");

            Post("/", args =>
            {
                var obj = this.BindAndValidate<RootObject>();

                if (!this.ModelValidationResult.IsValid)
                    return 422;

                var objRabbit = new MessageBroker();
                objRabbit.WriteMessageOnQueue("AppVeyor", JsonConvert.SerializeObject(obj));

                return obj;
            });
        }
    }
}