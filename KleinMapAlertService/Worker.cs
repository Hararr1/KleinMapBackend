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

        private int lastUserIndex;
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
            lastUserIndex = 0;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
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

                allSubscribers ??= _databaseClient.ExecuteSelectQuery<Subscriber>(s => s.Id > 0, "SELECT * FROM Subscribers").ToList();

                IEnumerable<Subscriber> newSubscribers = allSubscribers.FindAll(s => s.IsSendVerifyCode == 0) ?? _databaseClient
                    .ExecuteSelectQuery<Subscriber>(s =>
                        s.IsSendVerifyCode == 0,
                        $"SELECT * FROM Subscribers WHERE [Id] > {lastUserIndex}");

                foreach (Subscriber newSub in newSubscribers)
                {
                    _logger.LogInformation("Sending e-mail to {mailAddress} at: {time}", newSub.MailAddress, DateTimeOffset.Now);

                    string verifyCode = EncryptionHelper.GetMd5Hash(newSub);
                    string stationName = allStations.FirstOrDefault(station => station.id == newSub.StationId)?.stationName;

                    string result = _mailTemplateManager.GetTemplate(TemplateType.Confirm).Run(new
                    {
                        VerifyCode = verifyCode,
                        StationName = stationName
                    });

                    _smtpClient.SendMail("dailyanalytics@kleinmap.com", "to@example.com", "KleinMap verify code", result);
                    _logger.LogInformation("Sended e-mail to {mailAddress} at: {time}", newSub.MailAddress, DateTimeOffset.Now);

                    if (allSubscribers.FirstOrDefault(s => s.Id == newSub.Id) == null)
                    {
                        newSub.IsSendVerifyCode = 1;
                        allSubscribers.Add(newSub);
                    } else
                    {
                        // First loop ExecuteAsync
                        allSubscribers.First(s => s.Id == newSub.Id).IsSendVerifyCode = 1;
                    }

                    int isUpdate = _databaseClient.ExecuteModifyQuery($"UPDATE Subscribers SET [IsVerify] = 1 WHERE [Id] = {newSub.Id}");
                    _logger.LogInformation("Updated database for {mailAddress} at: {time} with status: {isUpdate}", newSub.MailAddress, DateTimeOffset.Now, isUpdate);
                }

                lastUserIndex = allSubscribers.Max(s => s.Id);

                await Task.Delay(3000, stoppingToken);
            }
        }
    }

}

