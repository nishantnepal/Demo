using System.Collections.Generic;

//http://piotrgankiewicz.com/2016/08/01/handling-domain-events/
//https://lostechies.com/gabrielschenker/2015/07/13/event-sourcing-applied-the-repository/

namespace Demo.Core.Models
{
    public interface IEntity
    {
        ICollection<IDomainEvent> Events { get; }
    }

    public interface IDomainEvent
    {
    }

    public interface IHandles<in T> where T : IDomainEvent
    {
        void Handle(T args);
    }

    public class NewOrderCreated : IDomainEvent
    {
        public NewOrderCreated(Order order)
        {
            Order = order;
        }

        public Order Order { get; private set; }
    }
}
