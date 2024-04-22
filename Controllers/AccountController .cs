using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models;
using Microsoft.AspNetCore.Authorization;


namespace TaskManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
	
    public class AccountController : ControllerBase
    {
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;
        private readonly AppDbContext _context;
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<Person> userManager, SignInManager<Person> signInManager, ILogger<AccountController> logger, AppDbContext context, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }

         // POST: api/Account/Login

 [HttpPost("login")]
public async Task<IActionResult> Login(LoginModel model)
{
    var user = await _context.Person.FirstOrDefaultAsync(u => u.Email == model.Email);

    if (user == null || user.Password != model.Password)
    {
        return Unauthorized("Invalid email or password.");
    }

    var userName = model.Email?.Substring(0, model.Email.IndexOf('@'));

    // Retrieve tasks for the logged-in user
var token = GenerateJwtToken(user);
    // Return username, email, and isadmin column
    return Ok(new
    {
        token,
        id = user.Id,
        userName = user.UserName,
        email = user.Email,
        isAdmin = user.IsAdmin,
    });
}



private string GenerateJwtToken(Person user)
{
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("GASRTHYTTRHFGSDFERG556Y5")); 
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Email, user.Email),
        
    };

    var token = new JwtSecurityToken(
        issuer: "PostmanTestIssuer",
        audience: "http://localhost:5213", 
        claims: claims,
        expires: DateTime.Now.AddMinutes(30),
        signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}



        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
        
            return Ok("Logout successful.");
        }



        // GET: api/Account
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
        {
            var persons = await _context.Person.ToListAsync();
            return Ok(persons);
        }

        // GET: api/Account/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            var person = await _context.Person.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        // PUT: api/Account/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(int id, Person person)
        {
            if (id != person.Id)
            {
                return BadRequest();
            }

            _context.Entry(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Account
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            _context.Person.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPerson), new { id = person.Id }, person);
        }

        // DELETE: api/Account/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var person = await _context.Person.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.Person.Remove(person);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonExists(int id)
        {
            return _context.Person.Any(e => e.Id == id);
        }
    }
}
