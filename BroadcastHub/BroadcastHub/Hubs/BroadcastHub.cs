using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using BroadcastHub.Models;

namespace BroadcastHub.Hubs
{
    class Broadcast : Hub
    {
        private readonly BroadcastContext _context;

        public Broadcast( BroadcastContext context)
        {
            _context = context;
            //new Action( async() => await BroadcastDataAsync())(); 
        }

        public async Task SendMessage()
        {
            while(true)
            {
                await Task.Delay(100);

                List<DataSet> data = (from d in _context.DataSet
                           select d).ToList();
                List<Tuple<int?, int?>> dataList = new List<Tuple<int?, int?>>();

                for (int i = 0; i < data.Count(); i++)
                {
                    dataList.Add( new Tuple<int?, int?>(data.ElementAt(i).x, data.ElementAt(i).y));
                }

                await Clients.All.SendAsync("updatechartasync", dataList);
            }
        }
    }
}
