using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace manager_properties_usa.Utilities
{
    public class UConf
    {
        private readonly IConfiguration _cnf;
        public UConf(IConfiguration cnf)
        {
            _cnf = cnf;
        }
        public UConf()
        {
            _cnf = GetCnf();
        }
        public static IConfigurationRoot GetCnf()
        {
            return new ConfigurationBuilder()
                       .SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appsettings.json")
                       .Build();
        }
        public IConfigurationSection GetSection(string name)
        {
            if (!_cnf.GetSection(name).Exists())
                throw new ArgumentException("No existe la sección " + name, name);
            return _cnf.GetSection(name);
        }
        public string GetSecretName(string sec) => GetSection("SecretsName:" + sec).Value;
        public string GetSecret(string name) => Environment.GetEnvironmentVariable(GetSecretName(name));
       
        public Settings Settings(string name = "AppSettings")
        {
            return SectionSettings(GetSection(name));
        }
        public static Settings SectionSettings(IConfigurationSection section)
        {
            return new Settings()
            {
                Title = section.GetSection("Title").Value,
                Version = section.GetSection("Version").Value,
                Dtap = section.GetSection("Dtap").Value,
                Environment = section.GetSection("Environment").Value
            };
        }
        public string GenerateToken(string userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetSecret("AuthKey")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
            };
            var token = new JwtSecurityToken(GetSecret("AuthIssuer"),
                GetSecret("AuthAudience"),
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
    public class UConfiguration : IUConfiguration
    {
        private readonly IConfiguration _cnf;
        public Boolean Production { get; }
        public Boolean Demo { get; }
        public UConfiguration(IConfiguration cnf)
        {
            _cnf = cnf;
            Demo = bool.Parse(_cnf.GetSection("Demo").Value);
            Production = _cnf.GetSection("AppSettings:Dtap").Value == "Production";
        }
        public IConfigurationSection GetSection(string sec) { return new UConf(_cnf).GetSection(sec); }
        public Settings Settings(string name = "AppSettings") { return new UConf(_cnf).Settings(name); }
        public string GetSecretName(string sec) => new UConf(_cnf).GetSecretName(sec);
        public string GetSecret(string name) => new UConf(_cnf).GetSecret(name);
        public string GenerateToken(string userId) => new UConf(_cnf).GenerateToken(userId);
    }

    public interface IUConfiguration
    {
        Boolean Demo { get; }
        Boolean Production { get; }
        IConfigurationSection GetSection(string sec);
        Settings Settings(string name = "AppSettings");
        string GetSecret(string sec);
        string GetSecretName(string sec);
        string GenerateToken(string userId);
    }
    public class Settings
    {
        public string Title { get; set; }
        public string Version { get; set; }
        public string Environment { get; set; }
        public string Dtap { get; set; }
    }
}