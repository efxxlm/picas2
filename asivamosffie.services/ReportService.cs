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

namespace asivamosffie.services
{
    public class ReportService : IReportService
    {
        private readonly devAsiVamosFFIEContext _context;
        private readonly ICommonService _commonService;

        private readonly IOptions<AzureAd> _azureAd;
        private readonly IOptions<PowerBI> _powerBI;

        private readonly string urlPowerBiServiceApiRoot = "https://api.powerbi.com";

        public ReportService(devAsiVamosFFIEContext context, ICommonService commonService, IOptions<AzureAd> azureAd, IOptions<PowerBI> powerBI)
        {
            _commonService = commonService;
            _context = context;
            _azureAd = azureAd;
            _powerBI = powerBI;

        }

        public string GetAccessToken()
        {
            AuthenticationResult authenticationResult = null;
            if (_azureAd.Value.AuthenticationMode.Equals("masteruser", StringComparison.InvariantCultureIgnoreCase))
            {
                // Create a public client to authorize the app with the AAD app
                IPublicClientApplication clientApp = PublicClientApplicationBuilder.Create(_azureAd.Value.ClientId).WithAuthority(_azureAd.Value.AuthorityUri).Build();
                var userAccounts = clientApp.GetAccountsAsync().Result;
                try
                {
                    // Retrieve Access token from cache if available
                    authenticationResult = clientApp.AcquireTokenSilent(_azureAd.Value.Scope, userAccounts.FirstOrDefault()).ExecuteAsync().Result;
                }
                catch (MsalUiRequiredException)
                {
                    SecureString password = new SecureString();
                    foreach (var key in _azureAd.Value.PbiPassword)
                    {
                        password.AppendChar(key);
                    }
                    authenticationResult = clientApp.AcquireTokenByUsernamePassword(_azureAd.Value.Scope, _azureAd.Value.PbiUsername, password).ExecuteAsync().Result;
                }
            }

            // Service Principal auth is the recommended by Microsoft to achieve App Owns Data Power BI embedding
            else if (_azureAd.Value.AuthenticationMode.Equals("serviceprincipal", StringComparison.InvariantCultureIgnoreCase))
            {
                // For app only authentication, we need the specific tenant id in the authority url
                var tenantSpecificUrl = _azureAd.Value.AuthorityUri.Replace("organizations", _azureAd.Value.TenantId);

                // Create a confidential client to authorize the app with the AAD app
                IConfidentialClientApplication clientApp = ConfidentialClientApplicationBuilder
                                                                                .Create(_azureAd.Value.ClientId)
                                                                                .WithClientSecret(_azureAd.Value.ClientSecret)
                                                                                .WithAuthority(tenantSpecificUrl)
                                                                                .Build();
                // Make a client call if Access token is not available in cache
                authenticationResult = clientApp.AcquireTokenForClient(_azureAd.Value.Scope).ExecuteAsync().Result;
            }

            return authenticationResult.AccessToken;
        }

        /// <summary>
        /// Get Power BI client
        /// </summary>
        /// <returns>Power BI client object</returns>
        public PowerBIClient GetPowerBIClient()
        {
            var tokenCredentials = new TokenCredentials(this.GetAccessToken(), "Bearer");
            return new PowerBIClient(new Uri(urlPowerBiServiceApiRoot), tokenCredentials);
        }

        /// <summary>
        /// Get embed params for a report
        /// </summary>
        /// <returns>Wrapper object containing Embed token, Embed URL, Report Id, and Report name for single report</returns>
        public async Task<List<IndicadorReporte>> GetReportEmbedInfo()
        {
            try
            {
                PowerBIClient pbiClient = this.GetPowerBIClient();

                List<IndicadorReporte> listIndicadorReporte = await _context.IndicadorReporte.Where(r => r.Activo == true).ToListAsync();

                foreach (var item in listIndicadorReporte)
                {
                    // Validate whether all the required configurations are provided in appsettings.json
                    PowerBI powerBI = new PowerBI { 
                        ReportId = item.ReportId,
                        WorkspaceId = item.GroupId
                    };

                    string configValidationResult = ConfigValidatorService.ValidateConfig(_azureAd, powerBI);
                    if (string.IsNullOrEmpty(configValidationResult))
                    {
                        Guid workspaceId = Guid.Parse(item.GroupId);
                        Guid reportId = Guid.Parse(item.ReportId);

                        // Get report info
                        var pbiReport = pbiClient.Reports.GetReportInGroup(workspaceId, reportId);

                        // Create list of datasets
                        var datasetIds = new List<Guid>();

                        // Add dataset associated to the report
                        datasetIds.Add(Guid.Parse(pbiReport.DatasetId));


                        // Add report data for embedding
                        var embedReports = new List<EmbedReport>() {
                        new EmbedReport
                        {
                            ReportId = pbiReport.Id, ReportName = pbiReport.Name, EmbedUrl = pbiReport.EmbedUrl
                        }
                    };

                        // Get Embed token multiple resources
                        var embedToken = GetEmbedToken(reportId, datasetIds, workspaceId);

                        // Capture embed params
                        var embedParams = new EmbedParams
                        {
                            EmbedReport = embedReports,
                            Type = "Report",
                            EmbedToken = embedToken
                        };
                        item.EmbedParams = embedParams;
                        //item.EmbedParams = JsonSerializer.Serialize<EmbedParams>(embedParams);
                    }
                }
                return listIndicadorReporte;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<IndicadorReporte> GetReportEmbedInfoByIndicadorReporteId(int indicadorReporteId)
        {
            try
            {
                PowerBIClient pbiClient = this.GetPowerBIClient();

                IndicadorReporte indicadorReporte = await _context.IndicadorReporte.Where(r => r.IndicadorReporteId == indicadorReporteId && r.Activo == true).FirstOrDefaultAsync();

                if(indicadorReporte != null)
                {
                    // Validate whether all the required configurations are provided in appsettings.json
                    PowerBI powerBI = new PowerBI
                    {
                        ReportId = indicadorReporte.ReportId,
                        WorkspaceId = indicadorReporte.GroupId
                    };

                    string configValidationResult = ConfigValidatorService.ValidateConfig(_azureAd, powerBI);
                    if (string.IsNullOrEmpty(configValidationResult))
                    {
                        Guid workspaceId = Guid.Parse(indicadorReporte.GroupId);
                        Guid reportId = Guid.Parse(indicadorReporte.ReportId);

                        // Get report info
                        var pbiReport = pbiClient.Reports.GetReportInGroup(workspaceId, reportId);

                        // Create list of datasets
                        var datasetIds = new List<Guid>();

                        // Add dataset associated to the report
                        datasetIds.Add(Guid.Parse(pbiReport.DatasetId));


                        // Add report data for embedding
                        var embedReports = new List<EmbedReport>() {
                        new EmbedReport
                        {
                            ReportId = pbiReport.Id, ReportName = pbiReport.Name, EmbedUrl = pbiReport.EmbedUrl
                        }
                    };

                        // Get Embed token multiple resources
                        var embedToken = GetEmbedToken(reportId, datasetIds, workspaceId);

                        // Capture embed params
                        var embedParams = new EmbedParams
                        {
                            EmbedReport = embedReports,
                            Type = "Report",
                            EmbedToken = embedToken
                        };
                        indicadorReporte.EmbedParams = embedParams;
                        //item.EmbedParams = JsonSerializer.Serialize<EmbedParams>(embedParams);
                    }
                }
                return indicadorReporte;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        /// <summary>
        /// Get Embed token for single report, multiple datasets, and an optional target workspace
        /// </summary>
        /// <returns>Embed token</returns>
        public EmbedToken GetEmbedToken(Guid reportId, IList<Guid> datasetIds, [Optional] Guid targetWorkspaceId)
        {
            PowerBIClient pbiClient = this.GetPowerBIClient();

            // Create a request for getting Embed token 
            // This method works only with new Power BI V2 workspace experience
            var tokenRequest = new GenerateTokenRequestV2(

                reports: new List<GenerateTokenRequestV2Report>() { new GenerateTokenRequestV2Report(reportId) },

                datasets: datasetIds.Select(datasetId => new GenerateTokenRequestV2Dataset(datasetId.ToString())).ToList(),

                targetWorkspaces: targetWorkspaceId != Guid.Empty ? new List<GenerateTokenRequestV2TargetWorkspace>() { new GenerateTokenRequestV2TargetWorkspace(targetWorkspaceId) } : null
            );

            // Generate Embed token
            var embedToken = pbiClient.EmbedToken.GenerateToken(tokenRequest);

            return embedToken;
        }

        public async Task<List<IndicadorReporte>> GetIndicadorReporte()
        {
            return await _context.IndicadorReporte.Where(r => r.Activo == true).ToListAsync();
        }


    }
}
