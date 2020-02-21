using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using W20_CardosoWebAPI.Models;
using Dapper;
using Microsoft.AspNet.Identity;

namespace W20_CardosoWebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Player")]
    public class PlayerController : ApiController
    {
        // POST api/Player/RegisterPlayer
        [HttpPost]
        [Route("RegisterPlayer")]
        public IHttpActionResult RegisterPlayer(PlayerModel player)
        {
            using (IDbConnection cnn = new ApplicationDbContext().Database.Connection)
            {
                string sql = "INSERT INTO dbo.Players(Id, Name, Email, BirthDay) " +
                    $"VALUES('{player.Id}','{player.Name}','{player.Email}','{player.BirthDay}')";
                try
                {
                    cnn.Execute(sql);
                }
                catch (Exception ex)
                {
                    return BadRequest("Error inserting player in database: " + ex.Message);
                }

                return Ok();
            }
        }

        // POST api/Player/RegisterOnlinePlayer
        [HttpPost]
        [Route("RegisterOnlinePlayer")]
        public IHttpActionResult RegisterOnlinePlayer(PlayerOnlineModel player)
        {
            using (IDbConnection cnn = new ApplicationDbContext().Database.Connection)
            {
                string sql = "INSERT INTO dbo.OnlinePlayers(Id) " +
                    $"VALUES('{player.Id}')";
                try
                {
                    cnn.Execute(sql);
                }
                catch (Exception ex)
                {
                    return BadRequest("Error inserting player in database: " + ex.Message);
                }

                return Ok();
            }
        }


        // GET api/Player/Info
        [HttpGet]
        [Route("Info")]
        public PlayerModel GetPlayerInfo()
        {
            string authenticatedAspNetUserId = RequestContext.Principal.Identity.GetUserId();
            using (IDbConnection cnn = new ApplicationDbContext().Database.Connection)
            {
                string sql = $"SELECT Id, Name, Email, BirthDay FROM dbo.Players " +
                    $"WHERE Id LIKE '{authenticatedAspNetUserId}'";
                var player = cnn.Query<PlayerModel>(sql).FirstOrDefault();
                return player;
            }
        }

        // GET api/Player/GetPlayerDateJoined
        [HttpGet]
        [Route("GetPlayerDateJoined")]
        public string GetPlayerdateJoined()
        {
            using(IDbConnection cnn = new ApplicationDbContext().Database.Connection)
            {
                string id = RequestContext.Principal.Identity.GetUserId();
                string sql = $"SELECT DateJoined FROM dbo.Players WHERE Id LIKE '{id}'";
                DateTime dateJoined = cnn.Query<DateTime>(sql).FirstOrDefault();
                return dateJoined.ToShortDateString();
            }
        }

        // GET api/Player/Online
        [HttpGet]
        [Route("Online")]
        public List<PlayerOnlineModel> GetPlayerOnline()
        {
            using (IDbConnection cnn = new ApplicationDbContext().Database.Connection)
            {
               string sql = $"SELECT Id FROM dbo.OnlinePlayers " +
                    $"WHERE Online = 1";
                List<PlayerOnlineModel> player = cnn.Query<PlayerOnlineModel>(sql).ToList();
                return player;
            }
        }
        // GET api/Player/ModifyPlayerOnline
        [HttpPost]
        [Route("ModifyPlayerOnline")]
        public void ModifyPlayerOnline()
        {
            string authenticatedAspNetUserId = RequestContext.Principal.Identity.GetUserId();
            using (IDbConnection cnn = new ApplicationDbContext().Database.Connection)
            {
                string sql = $"Update dbo.OnlinePlayers SET Online = 1 " +
                    $"WHERE Id = '{authenticatedAspNetUserId}'";
                cnn.Execute(sql);
            }
        }

        // GET api/Player/ModifyPlayerOffline
        [HttpPost]
        [Route("ModifyPlayerOffline")]
        public void ModifyPlayerOffline()
        {
            string authenticatedAspNetUserId = RequestContext.Principal.Identity.GetUserId();
            using (IDbConnection cnn = new ApplicationDbContext().Database.Connection)
            {
                string sql = $"Update dbo.OnlinePlayers SET Online = 0 " +
                    $"WHERE Id = '{authenticatedAspNetUserId}'";
                cnn.Execute(sql);
            }
        }

    }
}
