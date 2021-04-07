//-----------------------------------------------------------------------
// <copyright file="SmsMessageProxy.cs" company="Champion International Moving, Ltd">
// Copyright (c) Champion International Moving, Ltd. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace GraphicalPush
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR.Client;
    using Microsoft.AspNetCore.SignalR;
    using Newtonsoft.Json;

    /// <summary>
    /// Handles SignalR logic for the client.  Used to connect to SignalR server at URI to get received message updates.
    /// </summary>
    public class BroadcastHubProxy
    {
        /// <summary>
        /// The uri where the signalR server is located.
        /// </summary>
        private const string SignalRUri = "http://localhost:13128/broadcast";

        /// <summary>
        /// Initializes a new instance of the <see cref="SmsMessageProxy"/> class.
        /// </summary>
        /// <param name="user">The current user.</param>
        public BroadcastHubProxy()
        {
        }

        /// <summary>
        /// Event used to fire notifications for new messages.
        /// </summary>
        public event EventHandler<BroadcastProxyEventArgs> ReceiveNewMessage;

        /// <summary>
        /// Gets or sets the hub connection.
        /// </summary>
        public HubConnection Connection { get; set; }

        /// <summary>
        /// Makes a hub connection asynchronously and registers a method for the Notify event.
        /// </summary>
        public async Task ConnectAsync()
        {
            //var hub = connection.CreateHubProxy("broadcaster");
            this.Connection = new HubConnectionBuilder().WithUrl(BroadcastHubProxy.SignalRUri
            ).Build();

            Console.WriteLine("UP");
            //this.Connection.On("updateChartAsync", async (o) => await this.UpdateChartAsync(o));
            try
            {
                //var hub = connection.CreateHubProxy("broadcaster");
                this.Connection = new HubConnectionBuilder().WithUrl(BroadcastHubProxy.SignalRUri).Build();
                this.Connection.On<List<Tuple<int?,int?>>>("updatechartasync", async (o) => await this.UpdateChartAsync(o));

                await Connection.StartAsync();
                await this.Connection.InvokeAsync("SendMessage");

                return;
            }
            catch (HttpRequestException e)
            {
  //              Console.WriteLine("Http Request Exception" + e.Message + " InnerException " + e.InnerException);
      //          Console.ReadLine();
                return;
            }
            catch (Exception e)
            {
           //     Console.WriteLine("Exception" + e.Message + " InnerException " + e.InnerException);
          //      Console.ReadLine();
                return;
            }
        }

        /// <summary>
        /// Check for new messages.  Called by the signalR server when a new message notification is received.
        /// </summary>
        /// <returns>A task.</returns>
        public async System.Threading.Tasks.Task UpdateChartAsync(List<Tuple<int?, int?>> input)
        {
            //List<Tuple<int?, int?>> dataList = new List<Tuple<int?, int?>>();
            try
            {
                Console.WriteLine(input);
                await this.OnNewMessageReceived(new BroadcastProxyEventArgs(input));
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                Console.ReadLine();
            }
        }

        /// <summary>
        /// Calls the event handler when a new message is received.
        /// </summary>
        /// <param name="e">The event args, containing new message count.</param>
        public async Task OnNewMessageReceived(BroadcastProxyEventArgs e)
        {
            var handler = this.ReceiveNewMessage;

            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
