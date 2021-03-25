﻿using System;
using System.Collections.Generic;
using System.Text;
using PenisPincher.Persistence;

namespace PenisPincher.Core.Models
{
    public class DiscordServerConfiguration : Entity<ulong>
    {
        public DiscordServerConfiguration(ulong serverId)
        {
            ServerId = serverId;
            MonitoredStreams = new List<MonitoredStream>();
            RoleReactions = new List<ReactionRole>();
            RuleMessageIds = new List<ulong>();
        }

        public ulong ServerId { get; set; }

        public List<MonitoredStream> MonitoredStreams { get; set; }

        public List<ReactionRole> RoleReactions { get; set; }

        public List<ulong> RuleMessageIds { get; set; }

    }
}