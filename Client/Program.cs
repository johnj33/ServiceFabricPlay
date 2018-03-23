using System;
using System.Collections.Generic;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using MyFirstActor.Interfaces;

namespace Client
{
    class Program
    {
        private static IMyFirstActor _myFirstActor;

        static void Main(string[] args)
        {
            var actorIds = new List<string>
            {
                "Act",
                "Act2",
                "Act3"
            };
            foreach (var id in actorIds)
            {
                AddNewKeyToActor(new ActorId(id));
            }
            Console.ReadLine();
        }

        private static void AddNewKeyToActor(ActorId actorId)
        {
            _myFirstActor = ActorProxy.Create<IMyFirstActor>(actorId, new Uri("fabric:/ServiceFabricPlay/MyFirstActorService"));
            Console.WriteLine($"current data in {actorId}");
            WriteKeys();
            var newKey = Guid.NewGuid();
            Console.WriteLine($"Adding Key: {newKey}");
            _myFirstActor.AddKey(newKey).Wait();
            Console.WriteLine($"current data in {actorId}");
            WriteKeys();
        }

        private static void WriteKeys()
        {
            var returnvalue = _myFirstActor.GetKeys().Result;
            foreach (var guid in returnvalue)
            {
                Console.WriteLine(guid);
            }
        }
    }
}
