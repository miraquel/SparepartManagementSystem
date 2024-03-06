using System.DirectoryServices.Protocols;
using System.Net;
using Microsoft.Extensions.Configuration;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Service.Interface;

namespace SparepartManagementSystem.Service.Implementation;

internal class ActiveDirectoryService: IActiveDirectoryService
{
    private readonly IConfiguration _configuration;

    public ActiveDirectoryService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IEnumerable<User> GetUsersFromActiveDirectory()
    {
        var ldapConnectionString = _configuration.GetSection("ActiveDirectory:ConnectionString").Value;
        var ldapNames = _configuration.GetSection("ActiveDirectory:Names").Value;
        var ldapUsername = _configuration.GetSection("ActiveDirectory:Username").Value;
        var ldapPassword = _configuration.GetSection("ActiveDirectory:Password").Value;

        if (ldapConnectionString == null)
            throw new InvalidOperationException("LDAP Connection String is not found in configuration");

        using var ldapConnection = new LdapConnection(ldapConnectionString);

        ldapConnection.AuthType = AuthType.Basic;
        ldapConnection.Credential = new NetworkCredential(ldapUsername, ldapPassword);

        if (ldapNames == null)
            throw new InvalidOperationException("LDAP Names is not found in configuration");

        string[] attributes = { "givenName", "sn", "sAMAccountName", "userPrincipalName" };

        var searchResponse = (SearchResponse)ldapConnection.SendRequest(
            new SearchRequest(
                ldapNames,
                "(sAMAccountName=*)",
                SearchScope.Subtree, attributes));

        var users = from SearchResultEntry entry in searchResponse.Entries
            let firstName = entry.Attributes["givenName"] != null ? entry.Attributes["givenName"][0].ToString() : ""
            let lastName = entry.Attributes["sn"] != null ? entry.Attributes["sn"][0].ToString() : ""
            let username = entry.Attributes["sAMAccountName"] != null
                ? entry.Attributes["sAMAccountName"][0].ToString()
                : ""
            let email = entry.Attributes["userPrincipalName"] != null
                ? entry.Attributes["userPrincipalName"][0].ToString()
                : ""
            select new User { FirstName = firstName, LastName = lastName, Username = username, Email = email };

        return users;
    }
}