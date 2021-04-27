using VkNet.Abstractions;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;

namespace VkNet.Commands
{
    /// <summary>
    ///     Represents a context of a command. This may include the client, peer id, peer type, user, and message.
    /// </summary>
    public interface ICommandContext
    {
        /// <summary>
        ///     Gets the <see cref="IVkApi" /> that the command is executed with.
        /// </summary>
        IVkApi Client { get; }
        /// <summary>
        ///     Gets the peer id that the command is executed in.
        /// </summary>
        long PeerId { get; }
        /// <summary>
        ///     Gets the <see cref="ConversationPeerType" /> that the command is executed in.
        /// </summary>
        ConversationPeerType PeerType { get; }
        /// <summary>
        ///     Gets the user id of the user who executed the command.
        /// </summary>
        long UserId { get; }
        /// <summary>
        ///     Gets the <see cref="Message" /> that the command is interpreted from.
        /// </summary>
        Message Message { get; }
        /// <summary>
        ///     Gets the <see cref="ClientInfo"/> of the user who executed the command.
        /// </summary>
        ClientInfo ClientInfo { get; }
    }
}
