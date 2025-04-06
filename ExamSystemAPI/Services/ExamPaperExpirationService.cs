using ExamSystemAPI.Model;
using ExamSystemAPI.Model.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace ExamSystemAPI.Services
{
    public class ExamPaperExpirationService : BackgroundService
    {
        private readonly IServiceScope serviceScope;

        public ExamPaperExpirationService(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScope = serviceScopeFactory.CreateScope();
        }

        public override void Dispose()
        {
            this.serviceScope.Dispose();
            base.Dispose();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var ctx = serviceScope.ServiceProvider.GetRequiredService<MyDbContext>();
                    // 查询出已经过期的试卷
                    DbConnection conn = ctx.Database.GetDbConnection();
                    if (conn.State != System.Data.ConnectionState.Open)
                    {
                        await conn.OpenAsync(stoppingToken);
                    }
                    List<PaperTeam> expiredPapers = new List<PaperTeam>();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select PaperId, TeamId, State, Deadline from T_Papers_Teams where Deadline <= GETDATE() and State = '1'";
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                PaperTeam paperTeam = new PaperTeam();
                                paperTeam.PaperId = reader.GetInt64(0);
                                paperTeam.TeamId = reader.GetInt64(1);
                                paperTeam.State = reader.GetString(2);
                                paperTeam.Deadline = reader.GetDateTime(3);
                                expiredPapers.Add(paperTeam);
                            }
                        }
                    }

                    // 更新过期试卷的状态为 0
                    foreach (var paperTeam in expiredPapers)
                    {
                        using (var updateCmd = conn.CreateCommand())
                        {
                            updateCmd.CommandText = $"update T_Papers_Teams set State = '0' WHERE PaperId = {paperTeam.PaperId} and TeamId = {paperTeam.TeamId}";
                            await updateCmd.ExecuteNonQueryAsync(stoppingToken);
                        }
                    }
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
