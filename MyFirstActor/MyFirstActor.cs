using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using MyFirstActor.Interfaces;

namespace MyFirstActor
{
    [StatePersistence(StatePersistence.Persisted)]
    internal class MyFirstActor : Actor, IMyFirstActor
    {
        public MyFirstActor(ActorService actorService, ActorId actorId) 
            : base(actorService, actorId)
        {
        }

        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");
            StateManager.TryAddStateAsync("KeyList", new List<Guid>());
            return StateManager.TryAddStateAsync("count", 0);
        }

        public Task AddKey(Guid key)
        {
            StateManager.AddOrUpdateStateAsync("KeyList", new List<Guid>{key}, (s, list) =>
            {
                list.Add(key);
                return list;
            }).Wait();
            //StateManager.SaveStateAsync().Wait();
            SaveStateAsync();
            return Task.CompletedTask;
        }

        public Task<List<Guid>> GetKeys()
        {
            return StateManager.GetStateAsync<List<Guid>>("KeyList");
        }
    }
}
