using KleinMapLibrary.Enums;
using KleinMapLibrary.Helpers;
using KleinMapLibrary.Interfaces;
using KleinMapLibrary.Managers;
using KleinMapLibrary.Models;
using KleinMapLibrary.Values;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KleinMapAlertService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ISMTPClient _smtpClient;
        private readonly IDatabaseClient _databaseClient;
        private readonly IMailTemplateManager _mailTemplateManager;
        private readonly IConfiguration _configuration;

        private List<Subscriber> allSubscribers;
        private List<Station> allStations;

        public Worker(
            ILogger<Worker> logger,
            ISMTPClient smtpClient,
            IDatabaseClient databaseClient,
            IMailTemplateManager mailTemplateManager,
            IConfiguration configuration)
        {
            _logger = logger;
            _smtpClient = smtpClient;
            _databaseClient = databaseClient;
            _mailTemplateManager = mailTemplateManager;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await GetAllData();
                await SendVerifyMail();
                await SendDailyMessages();
                await Task.Delay(3000, stoppingToken);
            }
        }
        private async Task GetAllData()
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            string dataPath = _configuration.GetSection("DataDirectory").Value;
            allStations = new List<Station>();

            foreach (var item in DictonaryValues.Provinces)
            {
                var stations = await FileManager.Instance.LoadDataAsync(item.Value, dataPath);
                allStations.AddRange(stations);
            }

            _logger.LogInformation("Get all data for {count} stations", allStations.Count);

            allSubscribers = (await _databaseClient.ExecuteSelectQuery<Subscriber>(s => s.Id > 0, "SELECT * FROM Subscribers")).ToList();
            _logger.LogInformation("Get all users", allSubscribers.Count);
        }

        private async Task SendVerifyMail()
        {
            IEnumerable<Subscriber> newSubscribers = allSubscribers.FindAll(s => s.IsSendVerifyCode == 0);

            foreach (Subscriber newSub in newSubscribers)
            {
                string verifyCode = EncryptionHelper.GetMd5Hash(newSub);
                string stationName = allStations.FirstOrDefault(station => station.id == newSub.StationId)?.stationName;

                string mailTemplate = _mailTemplateManager.GetTemplate(TemplateType.Confirm).Run(new
                {
                    KleinAppAddress = $"{_configuration.GetSection("KleinAppAddress").Value}daily?userId={newSub.Id}?code={verifyCode}",
                    VerifyCode = verifyCode,
                    StationName = stationName,
                    UserId = newSub.Id
                });

                _smtpClient.SendMail("dailyanalytics@kleinmap.com", "to@example.com", "KleinMap verify code", mailTemplate);
                _logger.LogInformation("Sended verify e-mail to {mailAddress} at: {time}", newSub.MailAddress, DateTimeOffset.Now);

                int isUpdate = await _databaseClient
                    .ExecuteModifyQuery($"UPDATE Subscribers SET [IsSendVerifyCode] = 1 WHERE [Id] = {newSub.Id}");

                _logger.LogInformation("IsSendVerifyCode updated for {mailAddress} at: {time} with status: {isUpdate}",
                    newSub.MailAddress, DateTimeOffset.Now, isUpdate);
            }
        }

        private async Task SendDailyMessages()
        {
            IEnumerable<Subscriber> verifySubscribers = allSubscribers.FindAll(s =>
                s.IsVerify == 1 &&
                s.LastDailyMail != DateTime.Now.DayOfYear
            );

            foreach (Subscriber verifySub in verifySubscribers)
            {
                if (verifySub.LastDailyMail == DateTime.Now.DayOfYear)
                {
                    continue;
                }

                IEnumerable<int> stationsId = verifySubscribers
                    .Where(s => s.MailAddress == verifySub.MailAddress)
                    .Select(x => x.StationId);

                IEnumerable<Station> stations = allStations
                    .Where(x => stationsId.Contains(x.id));

                string mailTemplate = _mailTemplateManager.GetTemplate(TemplateType.Analysis).Run(new
                {
                    Stations = stations
                });

                _smtpClient.SendMail("dailyanalytics@kleinmap.com", "to@example.com", "KleinMap daily", mailTemplate);
                _logger.LogInformation("Sended daily information to {mailAddress} at: {time}", verifySub.MailAddress, DateTimeOffset.Now);

                foreach (Subscriber subToUpdate in verifySubscribers.Where(s => s.MailAddress == verifySub.MailAddress))
                {
                    int isUpdate = await _databaseClient.ExecuteModifyQuery($"UPDATE Subscribers SET [LastDailyMail] = {DateTime.Now.DayOfYear} WHERE [Id] = {subToUpdate.Id}");
                    _logger.LogInformation("LastDailyMail updated for {mailAddress} at: {time} with status: {isUpdate}", subToUpdate.MailAddress, DateTimeOffset.Now, isUpdate);
                    subToUpdate.LastDailyMail = DateTime.Now.DayOfYear;
                }
            }
        }
    }

}

