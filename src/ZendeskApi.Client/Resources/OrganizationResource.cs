﻿using System.Threading.Tasks;
using ZendeskApi.Client.Formatters;
using ZendeskApi.Contracts.Models;
using ZendeskApi.Contracts.Requests;
using ZendeskApi.Contracts.Responses;

namespace ZendeskApi.Client.Resources
{
    public class OrganizationResource : IOrganizationResource
    {
        private const string ResourceUri = "api/v2/organizations";
        private readonly IZendeskApiClient _apiClient;

        public OrganizationResource(IZendeskApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IListResponse<Organization>> SearchByExtenalIdsAsync(params string[] externalIds)
        {
            using (var client = _apiClient.CreateClient(ResourceUri))
            {
                var response = await client.GetAsync($"show_many?ids={ZendeskFormatter.ToCsv(externalIds)}").ConfigureAwait(false);
                return await response.Content.ReadAsAsync<OrganizationListResponse>();
            }
        }
        
        public async Task<Organization> GetAsync(long id)
        {
            using (var client = _apiClient.CreateClient(ResourceUri))
            {
                var response = await client.GetAsync(id.ToString()).ConfigureAwait(false);
                return (await response.Content.ReadAsAsync<OrganizationResponse>()).Item;
            }
        }
        
        public async Task<Organization> PutAsync(OrganizationRequest request)
        {
            using (var client = _apiClient.CreateClient(ResourceUri))
            {
                var response = await client.PutAsJsonAsync(request.Item.Id.ToString(), request).ConfigureAwait(false);
                return (await response.Content.ReadAsAsync<OrganizationResponse>()).Item;
            }
        }
        
        public async Task<Organization> PostAsync(OrganizationRequest request)
        {
            using (var client = _apiClient.CreateClient())
            {
                var response = await client.PostAsJsonAsync(ResourceUri, request).ConfigureAwait(false);
                return (await response.Content.ReadAsAsync<OrganizationResponse>()).Item;
            }
        }

        public Task DeleteAsync(long id)
        {
            using (var client = _apiClient.CreateClient(ResourceUri))
            {
                return client.DeleteAsync(id.ToString());
            }
        }
    }
}
