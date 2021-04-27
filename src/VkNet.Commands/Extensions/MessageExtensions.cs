using System;
using VkNet.Model;

namespace VkNet.Commands.Extensions
{
    /// <summary>
    ///     Provides extension methods for <see cref="Message" /> that relates to commands.
    /// </summary>
    public static class MessageExtensions
    {
        public static string GetMessageContent(this Message msg) => msg.Text ?? msg.Body;

        /// <summary>
        ///     Gets whether the message starts with the provided character.
        /// </summary>
        /// <param name="msg">The message to check against.</param>
        /// <param name="c">The char prefix.</param>
        /// <param name="argPos">References where the command starts.</param>
        /// <returns>
        ///     <c>true</c> if the message begins with the char <paramref name="c"/>; otherwise <c>false</c>.
        /// </returns>
        public static bool HasCharPrefix(this Message msg, char c, ref int argPos)
        {
            var text = msg.GetMessageContent();
            if (!string.IsNullOrEmpty(text) && text[0] == c)
            {
                argPos = 1;
                return true;
            }
            return false;
        }
        /// <summary>
        ///     Gets whether the message starts with the provided string.
        /// </summary>
        public static bool HasStringPrefix(this Message msg, string str, ref int argPos, StringComparison comparisonType = StringComparison.Ordinal)
        {
            var text = msg.GetMessageContent();
            if (!string.IsNullOrEmpty(text) && text.StartsWith(str, comparisonType))
            {
                argPos = str.Length;
                return true;
            }
            return false;
        }
        /// <summary>
        ///     Gets whether the message starts with the user's mention string.
        /// </summary>
        public static bool HasMentionPrefix(this Message msg, long mentionId, ref int argPos)
        {
            var text = msg.GetMessageContent();
            if (string.IsNullOrEmpty(text)) return false;
            if (mentionId < 0 && (text.Length <= 8 || text[0] != '[' || text[1] != 'c' || text[2] != 'l' ||
                                  text[3] != 'u' || text[4] != 'b')) return false;
            if (mentionId > 0 && (text.Length <= 6 || text[0] != '[' || text[1] != 'i' || text[2] != 'd'))
                return false;

            var parseIdFromIndex = mentionId switch
            {
                <0 => 5,
                >0 => 3,
                _ => throw new IndexOutOfRangeException()
            };

            var endPos = text.IndexOf('|');
            if (endPos == -1)
                return false;
            var mentionEndPos = text.IndexOf(']', endPos + 1);
            if (mentionEndPos == -1)
                return false;

            if (text.Length < mentionEndPos + 2 || text[mentionEndPos + 1] != ' ') return false; //Must end in "] "

            if (!long.TryParse(text.Substring(parseIdFromIndex, endPos), out var userId))
                return false;
            if (mentionId < 0)
                userId *= -1;
            if (userId == mentionId)
            {
                argPos = mentionEndPos + 2;
                return true;
            }
            return false;
        }
    }
}
