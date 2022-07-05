using System;
using Sula.Core.Models.Support;

namespace Sula.Core.Models.Entity
{
    public class TextMessage
    {
        public string Id { get; set; }
        public MessageType Type { get; set; }
        public MessageStatus Status { get; set; }
        public DateTimeOffset SentAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public int? ErrorCode { get; set; }

        public TextMessage()
        {
        }

        public TextMessage(string messageSid, MessageType type)
        {
            Id = messageSid;
            Type = type;
            Status = MessageStatus.None;
            SentAt = DateTimeOffset.Now;
        }
    }
}