using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemberManager.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MemberManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json", optional: false)
          .Build();
            AppSettings appSettings = new AppSettings();
            config.GetSection("AppSettings").Bind(appSettings);
            CreateHostBuilder(args, appSettings.EnvironmentName).Build().Run();
            //Build()��e�m�ǳƳ��]�w������A�N�i�H�I�s����k��Ҥ� Host�A�P�ɤ]�|��Ҥ� WebHost�C
            //Run()�Ұ� Host�C
        }

        public static IHostBuilder CreateHostBuilder(string[] args, string environmentName) =>
       //CreateDefaultBuilder()�w�]�ϥ�  Kestrel ���� WebServer �إ�WebHost
       Host.CreateDefaultBuilder(args)
           .UseEnvironment(environmentName)
           .ConfigureWebHostDefaults(webBuilder =>
           {
               webBuilder.UseStartup<Startup>();//�]�w WebHostBuilder ���ͪ� WebHost �ɡA�ҭn���檺���O�C
                });
    }
}
