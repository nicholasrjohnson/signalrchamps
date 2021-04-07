//-----------------------------------------------------------------------
// <copyright file="SmsProxyEventArgs.cs" company="Champion International Moving, Ltd">
// Copyright (c) Champion International Moving, Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GraphicalPush
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Event args to house SMS message count.
    /// </summary>
    public class BroadcastProxyEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="broadcastProxyEventArgs"/> class.
        /// </summary>
        /// <param name="messageCount">The SMS message count for the current user.</param>
        public BroadcastProxyEventArgs(List<Tuple<int?, int?>> data)
        {
            this.data = data;
        }

        /// <summary>
        /// Gets or sets the count of new user's messages.
        /// </summary>
        public List<Tuple<int?, int?>> data { get; set; }

        /// <summary>
        /// Gets the data notifcation based on the message counts.
        /// </summary>
        public List<Tuple<int?, int?>> UpdateChart
        {
            get
            {
                return this.data;
            }
        }
    }
}
