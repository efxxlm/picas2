using asivamosffie.model.APIModels;
using asivamosffie.model.Models;
using asivamosffie.services.Interfaces;
using asivamosffie.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using System.Security;
using System.Text;
using asivamosffie.model.AditionalModels;
using System.Net.Http;
using System.IO;
//using Newtonsoft.Json;

namespace asivamosffie.services
{
    public class GenerarGraficoService : IGenerarGraficoService
    {
        private static readonly HttpClient Client = new HttpClient();

        public GenerarGraficoService()
        { }

        public async Task<object> CreateChartasFile(string path, ChartConfig config)
        {
            var jsonChartConfig = JsonSerializer.Serialize(config);
            Chart qc = new Chart
            {
                Width = 500,
                Height = 300,
                Config = jsonChartConfig
            };
            var ByteArray = await ToByteArray(qc);

            File.WriteAllBytes(path, ByteArray);

            var save = File.Exists(path);
            return save ? path : "";
        }

        public async Task<object> CreateChartasFile(string path)
        {
            Chart qc = new Chart
            {
                Width = 500,
                Height = 300,
                Config = @"{
                type: 'line',
                data: {
                    labels: ['Q1', 'Q2', 'Q3', 'Q4'],
                    datasets: [{
                        label: 'Acumulado mensual - Programado',
                        fill: false,
                        backgroundColor: 'rgb(255, 99, 132)',
                        borderColor: 'rgb(255, 99, 132)',
                        data: [50, 60, 70, 180]
                    },
                    {
                        label: 'Facturacion mensual - Ejecutado',
                        fill: false,
                        backgroundColor: 'rgb(54, 162, 235)',
                        borderColor: 'rgb(54, 162, 235)',
                        data: [55, 65, 75, 185]
                    },
                    {
                        label: 'Facturacion mensual - Programado',
                        fill: false,
                        backgroundColor: 'rgb(96, 162, 235)',
                        borderColor: 'rgb(96, 162, 235)',
                        data: [80, 85, 75, 95]
                    },
                    {
                        label: 'Facturacion mensual - Ejecutado',
                        fill: false,
                        backgroundColor: 'rgb(10, 162, 235)',
                        borderColor: 'rgb(10, 162, 235)',
                        data: [40, 42, 44, 46]
                    }]
                },
                options: {
                    title: {
                      display: true,
                      text: 'Avance Financiero',
                    },
                }
            }"
            };
            var ByteArray = await ToByteArray(qc);

            File.WriteAllBytes(path, ByteArray);

            var save = File.Exists(path);
            return save ? "Se creó el gráfico en la ruta " + path : "No se creó el gráfico.";
        }

        public async Task<object> CreateChartasURL()
        {
            Console.WriteLine("Fetching short url...");
            Chart qc = new Chart
            {
                Width = 500,
                Height = 300,
                Config = @"{
                type: 'bar',
                data: {
                    labels: ['Q1', 'Q2', 'Q3', 'Q4'],
                    datasets: [{
                    label: 'Users',
                    data: [50, 60, 70, 180]
                    }]
                    }
                }"
            };

            var url = await GetShortUrl(qc);
            return "Se creó el gráfico en la url " + url;
        }

        //public string GetUrl()
        //{
        //    if (Config == null)
        //    {
        //        throw new NullReferenceException("You must set Config on the QuickChart object before generating a URL");
        //    }

        //    StringBuilder builder = new StringBuilder();
        //    builder.Append("w=").Append(Width.ToString());
        //    builder.Append("&h=").Append(Height.ToString());

        //    builder.Append("&devicePixelRatio=").Append(DevicePixelRatio.ToString());
        //    builder.Append("&f=").Append(Format);
        //    builder.Append("&bkg=").Append(Uri.EscapeDataString(BackgroundColor));
        //    builder.Append("&c=").Append(Uri.EscapeDataString(Config));
        //    if (!string.IsNullOrEmpty(Key))
        //    {
        //        builder.Append("&key=").Append(Uri.EscapeDataString(Key));
        //    }
        //    if (!string.IsNullOrEmpty(Version))
        //    {
        //        builder.Append("&v=").Append(Uri.EscapeDataString(Version));
        //    }

        //    return $"{Scheme}://{Host}:{Port}/chart?{builder}";
        //}

        public async Task<string> GetShortUrl(Chart qc)
        {
            if (qc.Config == null)
            {
                throw new NullReferenceException("You must set Config on the QuickChart object before generating a URL");
            }

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.IgnoreNullValues = true;

            string json = JsonSerializer.Serialize(new
            {
                width = qc.Width,
                height = qc.Height,
                backgroundColor = qc.BackgroundColor,
                devicePixelRatio = qc.DevicePixelRatio,
                format = qc.Format,
                chart = qc.Config,
                key = qc.Key,
                version = qc.Version,
            }, options);

            string url = $"{qc.Scheme}://{qc.Host}:{qc.Port}/chart/create";

            HttpResponseMessage response = await Client.PostAsync(
                url,
                new StringContent(json, Encoding.UTF8, "application/json")
            );

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Unsuccessful response from API");
            }

            string responseText = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<QuickChartShortUrlResponse>(responseText);
            return result.url;
        }

        public async Task<byte[]> ToByteArray(Chart qc)
        {
            if (qc.Config == null)
            {
                throw new NullReferenceException("You must set Config on the QuickChart object before generating a URL");
            }

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.IgnoreNullValues = true;

            string json = JsonSerializer.Serialize(new
            {
                width = qc.Width,
                height = qc.Height,
                backgroundColor = qc.BackgroundColor,
                devicePixelRatio = qc.DevicePixelRatio,
                format = qc.Format,
                chart = qc.Config,
                key = qc.Key,
                version = qc.Version,
            }, options);

            string url = $"{qc.Scheme}://{qc.Host}:{qc.Port}/chart";

            HttpResponseMessage response = await Client.PostAsync(
                url,
                new StringContent(json, Encoding.UTF8, "application/json")
            );

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Unsuccessful response from API");
            }

            return response.Content.ReadAsByteArrayAsync().Result;
        }
    }
}
