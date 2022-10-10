using Microsoft.IdentityModel.Tokens;
using MyDemoProjects.Application.Shared.Constants;
using MyDemoProjects.Application.Shared.DTOs.Response;
using MyDemoProjects.Application.Shared.Models.Response;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyDemoProjects.UnitTests
{
    public static class HelperMethods
    {
        public static string GenerateDummyToken(ClaimsIdentity claimsIdentity)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8
                   .GetBytes(Constants.TokenKey));
            var signingCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                    claims: claimsIdentity.Claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: signingCredential);
            var generatedJwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return generatedJwtToken;
        }

        public static AnimeListRoot ReturnAnimeListDummyData()
        {
            //var node = new Node(
            //    23,
            //    "test", 
            //    new MainPicture("small","large"),
            //    "test",
            //    );
            //var Data = new Datum(node);

            return new AnimeListRoot(null,null,null);
        }
    }
}
