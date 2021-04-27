using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VkNet.Commands.Builders;
using VkNet.Commands.Info;
using VkNet.Enums.SafetyEnums;
using VkNet.Model.Attachments;
using VkNet.Model.Keyboard;
using VkNet.Model.RequestParams;
using VkNet.Model.Template;

namespace VkNet.Commands
{
    /// <summary>
    ///     Provides a base class for a command module to inherit from.
    /// </summary>
    public abstract class ModuleBase : ModuleBase<ICommandContext> { }

    /// <summary>
    ///     Provides a base class for a command module to inherit from.
    /// </summary>
    /// <typeparam name="T">A class that implements <see cref="ICommandContext"/>.</typeparam>
    public abstract class ModuleBase<T> : IModuleBase
        where T : class, ICommandContext
    {
        /// <summary>
        ///     The underlying context of the command.
        /// </summary>
        /// <seealso cref="T:Discord.Commands.ICommandContext" />
        /// <seealso cref="T:Discord.Commands.CommandContext" />
        public T Context { get; private set; }

        /// <summary>
        ///     Sends a message to the source channel.
        /// </summary>
        /// <param name="message">
        /// Contents of the message; optional only if <paramref name="attachments"/> is specified
        /// </param>
        /// <param name="attachments">
        /// Attachments of the message; optional only if <paramref name="message"/> is specified
        /// </param>
        /// <param name="keyboard">Keyboard template for the message</param>
        /// <param name="template">Template for the message</param>
        /// <param name="replyTo">Id of the message you want to reference to</param>
        /// <param name="forwardMessages">List of message Ids to include with the sent message</param>
        /// <param name="doNotParseLink">Don't parse links</param>
        /// <param name="disableMentions">Don't send mentions to users</param>
        /// <param name="intent">The subject for the message</param>
        /// <param name="subscribeId">Subscribe Id</param>
        protected virtual async Task<long> ReplyAsync(string message = null, IEnumerable<MediaAttachment> attachments = null,
            MessageKeyboard keyboard = null, MessageTemplate template = null, long? replyTo = null,
            IEnumerable<long> forwardMessages = null, bool doNotParseLink = false, bool disableMentions = false,
            Intent intent = null, byte? subscribeId = null)
        {
            return await Context.Client.Messages.SendAsync(new MessagesSendParams
            {
               Message = message,
               Attachments = attachments,
               Keyboard = keyboard,
               Template = template,
               ReplyTo = replyTo,
               ForwardMessages = forwardMessages,
               DontParseLinks = doNotParseLink,
               DisableMentions = disableMentions,
               Intent = intent,
               SubscribeId = subscribeId,
               PeerId = Context.PeerId,
               RandomId = DateTime.Now.Ticks
            }).ConfigureAwait(false);
        }

        /// <summary>
        ///     The method to execute before executing the command.
        /// </summary>
        /// <param name="command">The <see cref="CommandInfo"/> of the command to be executed.</param>
        protected virtual void BeforeExecute(CommandInfo command)
        {
        }
        /// <summary>
        ///     The method to execute after executing the command.
        /// </summary>
        /// <param name="command">The <see cref="CommandInfo"/> of the command to be executed.</param>
        protected virtual void AfterExecute(CommandInfo command)
        {
        }

        /// <summary>
        ///     The method to execute when building the module.
        /// </summary>
        /// <param name="commandService">The <see cref="CommandService"/> used to create the module.</param>
        /// <param name="builder">The builder used to build the module.</param>
        protected virtual void OnModuleBuilding(CommandService commandService, ModuleBuilder builder)
        {
        }

        //IModuleBase
        void IModuleBase.SetContext(ICommandContext context)
        {
            var newValue = context as T;
            Context = newValue ?? throw new InvalidOperationException($"Invalid context type. Expected {typeof(T).Name}, got {context.GetType().Name}.");
        }
        void IModuleBase.BeforeExecute(CommandInfo command) => BeforeExecute(command);
        void IModuleBase.AfterExecute(CommandInfo command) => AfterExecute(command);
        void IModuleBase.OnModuleBuilding(CommandService commandService, ModuleBuilder builder) => OnModuleBuilding(commandService, builder);
    }
}
