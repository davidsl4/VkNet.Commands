using System;
using VkNet.Abstractions;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.GroupUpdate;

namespace VkNet.Commands
{
    /// <summary> The context of a command which may include the client, peer id, peer type, user, and message. </summary>
    public class CommandContext : ICommandContext
    {
        public IVkApi Client { get; }
        public long PeerId { get; }
        public ConversationPeerType PeerType { get; }
        public long UserId { get; }
        public Message Message { get; }
        public ClientInfo ClientInfo { get; }

        public CommandContext(IVkApi client, MessageNew messageNew)
        {
            Client = client;
            PeerId = messageNew.Message.PeerId.GetValueOrDefault();
            PeerType = PeerId switch
            {
                <0 => ConversationPeerType.Group,
                >0 and <2000000000 => ConversationPeerType.User,
                >2000000000 => ConversationPeerType.Chat,
                _ => throw new IndexOutOfRangeException()
            };
            UserId = messageNew.Message.FromId.GetValueOrDefault();
            (Message, ClientInfo) = (messageNew.Message, messageNew.ClientInfo);
        }
    }
}
