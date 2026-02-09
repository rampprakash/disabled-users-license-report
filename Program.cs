using Azure.Identity;
using Microsoft.Graph;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CheckDisabledUsersLicenses
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string tenantId = "";
            string clientId = "";
            string clientSecret = "";

            try
            {
                var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
                var graphClient = new GraphServiceClient(credential);

                Console.WriteLine("Checking disabled users with licenses...\n");
                var skus = await graphClient.SubscribedSkus.GetAsync();
                var usersPage = await graphClient.Users.GetAsync(config =>
                {
                    config.QueryParameters.Select = new[]
                    {
                        "displayName",
                        "userPrincipalName",
                        "accountEnabled",
                        "licenseAssignmentStates"
                    };
                });

                while (usersPage != null)
                {
                    foreach (var user in usersPage.Value)
                    {
                        bool isDisabled = user.AccountEnabled.HasValue && !user.AccountEnabled.Value;
                        bool hasLicenses = user.LicenseAssignmentStates != null &&
                                           user.LicenseAssignmentStates.Count > 0;

                        if (isDisabled && hasLicenses)
                        {
                            Console.WriteLine("Disabled user still consuming licenses:");
                            Console.WriteLine($"Name: {user.DisplayName}");
                            Console.WriteLine($"UPN : {user.UserPrincipalName}");

                            foreach (var state in user.LicenseAssignmentStates)
                            {
                                var sku = skus.Value.FirstOrDefault(s => s.SkuId == state.SkuId);

                                string licenseName = sku?.SkuPartNumber ?? state.SkuId.ToString();
                                Console.WriteLine($"License: {licenseName}");

                                if (!string.IsNullOrEmpty(state.AssignedByGroup))
                                {
                                    var group = await graphClient.Groups[state.AssignedByGroup].GetAsync();
                                    Console.WriteLine($"Assigned via group: {group.DisplayName} (Group ID: {state.AssignedByGroup})");
                                }
                                else
                                {
                                    Console.WriteLine("Assigned directly to user");
                                }
                            }
                        }
                    }

                    if (usersPage.OdataNextLink != null)
                    {
                        usersPage = await graphClient.Users
                            .WithUrl(usersPage.OdataNextLink)
                            .GetAsync();
                    }
                    else
                    {
                        usersPage = null;
                    }
                }

                Console.WriteLine("\nCheck complete.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
