﻿using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Rebus.Messages;
using System.Linq;

namespace Rebus.Serialization.Binary
{
    public class BinaryMessageSerializer : ISerializeMessages
    {
        public TransportMessageToSend Serialize(Message message)
        {
            using (var memoryStream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, message);
                memoryStream.Position = 0;
                
                return new TransportMessageToSend
                           {
                               Label = message.GetLabel(),
                               Headers = message.Headers.ToDictionary(k => k.Key, v => v.Value),
                               Data = Convert.ToBase64String(memoryStream.ToArray()),
                           };
            }
        }

        public Message Deserialize(ReceivedTransportMessage transportMessage)
        {
            using (var memoryStream = new MemoryStream(Convert.FromBase64String(transportMessage.Data)))
            {
                var formatter = new BinaryFormatter();
                var message = (Message) formatter.Deserialize(memoryStream);
                return message;
            }
        }
    }
}