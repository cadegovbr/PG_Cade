using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

/// <summary>
/// Summary description for ClaimsUtil
/// </summary>
/// 


public class ClaimsUtil
{
    public static string CPF = "http://schemas.xmlsoap.org/claims/cpf";

    public static string GetClaimsValue(ClaimsIdentity claimsIdentity, string chave)
    {
        // Access claim
        string claim;

        try
        {
            claim = (from c in claimsIdentity.Claims
                     where c.Type == chave
                     select c.Value).Single();
        }
        catch (InvalidOperationException)   // nao existe a claim, retorna null.
        {
            claim = null;
        }
        return claim;
    }
 
    

    public static void AddRoles(ClaimsIdentity claimsIdentity, string[] roles)
    {
        var claimsRoles = roles.Select(r => new Claim(ClaimTypes.Role, r));
        claimsIdentity.AddClaims(claimsRoles);
    }

    public static void RemoveRole(ClaimsIdentity claimsIdentity, string role)
    {
        var claim = (from c in claimsIdentity.Claims
                     where c.Type == ClaimTypes.Role
                           && c.Value == role
                     select c).FirstOrDefault();
        if (claim != null)
            claimsIdentity.RemoveClaim(claim);
    }
}
