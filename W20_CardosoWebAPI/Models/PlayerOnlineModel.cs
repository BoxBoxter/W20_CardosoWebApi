using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace W20_CardosoWebAPI.Models
{
    public class PlayerOnlineModel
    {
        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private int _online;
        public int Online
        {
            get { return _online; }
            set { _online = value; }
        }
    }
}