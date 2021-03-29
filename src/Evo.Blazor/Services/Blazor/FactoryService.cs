using Evo.Attributes.Blazor;
using Evo.Models.Blazor;
using System;


namespace Evo.Services.Blazor
{
    public class FactoryService: FactoryService_I
    {
        private long _LastId;
        private System.Reflection.MethodInfo _GetSinkMethod;
        private System.Type _ElementSinkType;
        private MessageBus_I _MessageBus;

        public FactoryService(IServiceProvider serviceProvider, MessageBus_I messageBus)
        {
            _MessageBus = messageBus;

            var messageBusType = _MessageBus.GetType();

            _GetSinkMethod = messageBusType.GetMethod("GetSink");

            _ElementSinkType = typeof(Evo.Delegates.Blazor.EventSink<object>).GetGenericTypeDefinition();

            ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; set; }

        public T Create<T>()
        {
            var t = (T)ServiceProvider.GetService(typeof(T));

            long id = 0;

            if (t is Object_I objectT)
            {
                id = NextId();

                objectT.Id = id;
            }

            if (t is EventSource_I)
            {
                var type = t.GetType();

                var properties = type.GetProperties<EventSourceAttribute>();

                foreach (var property in properties)
                {
                    var propertyType = property.PropertyType;

                    var genericTypeDefinition = propertyType.GetGenericTypeDefinition();

                    if (genericTypeDefinition != _ElementSinkType)
                    {
                        throw new Exception($"Expected the element sink to be of type {_ElementSinkType.FullName}");
                    }

                    var genericArguments = propertyType.GetGenericArguments();

                    var concreteGetSink = _GetSinkMethod.MakeGenericMethod(genericArguments);

                    var sink = concreteGetSink.Invoke(_MessageBus, new object[] { id });

 
                    //var sink = _MessageBus.GetSink(id, );

                    property.SetValue(t, sink);

                    //Console.WriteLine("Set value");
                }
            }

            return t;
        }

        private long NextId()
        {
            return ++_LastId;
        }
    }
}
