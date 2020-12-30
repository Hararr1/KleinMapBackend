using KleinMapLibrary.Helpers;
using KleinMapLibrary.Interfaces;
using KleinMapLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KleinMapAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SubscribeController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IDatabaseClient _databaseClient;
        public SubscribeController(IConfiguration config, IDatabaseClient databaseClient)
        {
            configuration = config;
            _databaseClient = databaseClient;
        }

        [HttpPost]
        public async Task<IActionResult> AddNewSubscriber(Subscriber newSub)
        {
            int output = await _databaseClient.ExecuteModifyQuery(
                $"INSERT INTO Subscribers (MailAddress, IsVerify, StationId, IsSendVerifyCode) VALUES " +
                $"({newSub.MailAddress}, {newSub.IsVerify}, {newSub.StationId}, {newSub.IsSendVerifyCode})");

            return output == 1 ? Ok() : NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> VerifyCode(int userId, string hash)
        {
            Subscriber user = await _databaseClient
                .ExecuteSelectQuery<Subscriber>($"SELECT * FROM Subscribers WHERE Id = {userId}");

            bool isVerify = EncryptionHelper.VerifyMd5Hash(user, hash);

            int output = await _databaseClient.ExecuteModifyQuery($"UPDATE Subcribers SET [IsVerify] = 1 WHERE [Id] = {user.Id}");
            return output == 1 ? Ok() : NoContent();
        }
    }
}
