using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Scada.Scheme.Editor
{
    public class CorsMessageInspector : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            StateMessage stateMsg = null;
            HttpRequestMessageProperty requestProperty = null;
            if (request.Properties.ContainsKey(HttpRequestMessageProperty.Name))
            {
                requestProperty = request.Properties[HttpRequestMessageProperty.Name] as HttpRequestMessageProperty;
            }

            if (requestProperty != null)
            {
                var origin = requestProperty.Headers["Origin"];
                if (!string.IsNullOrEmpty(origin))
                {
                    stateMsg = new StateMessage();
                    if (requestProperty.Method == "OPTIONS")
                    {
                        stateMsg.Message = Message.CreateMessage(request.Version, null);
                    }
                    request.Properties.Add("CrossOriginResourceSharingState", stateMsg);
                }
            }

            return stateMsg;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            var stateMsg = correlationState as StateMessage;

            if (stateMsg != null)
            {
                if (stateMsg.Message != null)
                {
                    reply = stateMsg.Message;
                }

                HttpResponseMessageProperty responseProperty = null;

                if (reply.Properties.ContainsKey(HttpResponseMessageProperty.Name))
                {
                    responseProperty = reply.Properties[HttpResponseMessageProperty.Name]
                                       as HttpResponseMessageProperty;
                }

                if (responseProperty == null)
                {
                    responseProperty = new HttpResponseMessageProperty();
                    reply.Properties.Add(HttpResponseMessageProperty.Name,
                                         responseProperty);
                }
                responseProperty.Headers.Set("Access-Control-Allow-Origin", "*");
                if (stateMsg.Message != null)
                {
                    responseProperty.Headers.Set("Access-Control-Allow-Methods", "POST, OPTIONS, GET");
                    responseProperty.Headers.Set("Content-Type", "application/json");
                    responseProperty.Headers.Set("Access-Control-Allow-Headers", "Content-Type, Accept, Authorization, x-requested-with");
                }
            }

        }
    }
    public class MessageInspectorBehavior : IEndpointBehavior
    {
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new CorsMessageInspector());
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }

    public class MessageInspectorExtensionElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(MessageInspectorBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new MessageInspectorBehavior();
        }
    }

    public class StateMessage
    {
        public Message Message;
    }
}
