namespace MyDemoProjects.Application.Features.Authentication.Commands;

public record LoginUser(LoginUserRequest User1) : IRequest<ApplicationResponse<string>>;

public class LoginUserHandler : IRequestHandler<LoginUser, ApplicationResponse<string>>
{
    private readonly ApplicationDbContext _dataContext;
    private readonly IConfiguration _configuration;

    public LoginUserHandler(ApplicationDbContext applicationDbContext, IConfiguration configuration)
    {
        _dataContext = applicationDbContext;
        _configuration = configuration;
    }
    public async Task<ApplicationResponse<string>> Handle(LoginUser request, CancellationToken cancellationToken)
    {
        var User1 = await _dataContext.Users.AsNoTracking().FirstOrDefaultAsync(e => e.Email.ToLower().Equals(request.User1.Email.ToLower()));
        //Check if User1 exist
        //if (User1 == null)
        //{
        //    return new ServerResponse<string>
        //    {
        //        Message = "User1 not found.",
        //        Status = false
        //    };
        //}

        ////Check if User1 claims who he is by verifying the current password
        //if (!PasswordHash.Verify(request.User1.Password, User1.PasswordHash, User1.PasswordSalt))
        //{
        //    return new ServerResponse<string>
        //    {
        //        Message = "Wrong email or password.",
        //        Status = false
        //    };
        //}

        return new ApplicationResponse<string>
        {
            // Data = CreateToken(User1),
            Message = "Login Successful",
            Status = true
        };

    }


    //private string CreateToken(User1 User1)
    //{
    //    List<Claim> claims = new List<Claim>
    //    {
    //        new Claim(ClaimTypes.NameIdentifier, User1.Id.ToString()),
    //        new Claim(ClaimTypes.Name, User1.Email),
    //        new Claim(ClaimTypes.GivenName, User1.FirstName),
    //        new Claim(ClaimTypes.Surname, User1.LastName),
    //        new Claim(ClaimTypes.Role,User1.Role.ToString())
    //    };

    //    var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
    //        .GetBytes(_configuration.GetSection("MyDemoProjects:SecretKey").Value));

    //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

    //    var token = new JwtSecurityToken(
    //            claims: claims,
    //            expires: DateTime.Now.AddDays(1),
    //            signingCredentials: creds);

    //    var jwt = new JwtSecurityTokenHandler().WriteToken(token);

    //    return jwt;
    //}
}


