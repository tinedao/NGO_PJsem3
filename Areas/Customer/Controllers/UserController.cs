using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NGO_PJsem3.Models;
using System.Security.Cryptography;
using System.Text;


namespace NGO_PJsem3.Areas.Customer.Controllers
{
    [EnableCors("AllowAllOrigins")]
    [Route("NGO/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IPasswordHasher<Users> _passwordHasher;

        public UserController(IPasswordHasher<Users> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public UserController(IHttpClientFactory httpClientFactory, IPasswordHasher<Users> passwordHasher)
        {
            _httpClient = httpClientFactory.CreateClient();
            _passwordHasher = passwordHasher;
        }

        [HttpGet("checkExistence")]
        public async Task<IActionResult> CheckExistence(string email, string phoneNumber)
        {
            // Kiểm tra sự tồn tại của email và số điện thoại
            bool isEmailExists = await CheckEmailExistence(email);
            bool isPhoneNumberExists = await CheckPhoneNumberExistence(phoneNumber);

            return Ok(new { EmailExists = isEmailExists, PhoneNumberExists = isPhoneNumberExists });
        }

        private async Task<bool> CheckEmailExistence(string email)
        {
            // Gửi yêu cầu kiểm tra sự tồn tại của email đến API
            var response = await _httpClient.GetAsync($"https://localhost:7296/NGO/Validation/CheckEmailExistence?email={email}");

            if (!response.IsSuccessStatusCode)
            {
                // Xử lý lỗi khi kiểm tra email tồn tại
                return false;
            }

            var result = await response.Content.ReadAsStringAsync();
            return bool.Parse(result);
        }

        private async Task<bool> CheckPhoneNumberExistence(string phoneNumber)
        {
            // Gửi yêu cầu kiểm tra sự tồn tại của số điện thoại đến API
            var response = await _httpClient.GetAsync($"https://localhost:7296/NGO/Validation/CheckPhoneNumberExistence?phoneNumber={phoneNumber}");

            if (!response.IsSuccessStatusCode)
            {
                // Xử lý lỗi khi kiểm tra số điện thoại tồn tại
                return false;
            }

            var result = await response.Content.ReadAsStringAsync();
            return bool.Parse(result);
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult> CreateUser(string username, string password, string imgUser, string email, string fullName, string address, string phoneNumber)
        {
            // Kiểm tra validate trước khi thêm người dùng mới
            bool isEmailExists = await CheckEmailExistence(email);
            bool isPhoneNumberExists = await CheckPhoneNumberExistence(phoneNumber);

            // Kiểm tra xem email và số điện thoại đã tồn tại hay chưa
            if (isEmailExists)
            {
                return BadRequest("Email already exists");
            }

            if (isPhoneNumberExists)
            {
                return BadRequest("Phone number already exists");
            }

            // Tiếp tục xử lý thêm người dùng mới
            var user = new Users
            {
                Username = username,
                Password = HashPassword(password),
                imgUser = imgUser,
                Email = email,
                FullName = fullName,
                Address = address,
                PhoneNumber = phoneNumber
            };

            // Gửi yêu cầu thêm người dùng mới đến API
            var jsonUser = JsonConvert.SerializeObject(user);
            var httpContent = new StringContent(jsonUser, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7296/NGO/User/CreateUser", httpContent);

            if (response.IsSuccessStatusCode)
            {
                return Ok("User created successfully");
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return BadRequest(errorMessage);
            }
        }

        private string HashPassword(string password)
        {
            // Mã hóa mật khẩu sử dụng bcrypt
            var passwordHash = _passwordHasher.HashPassword(null, password);
            return passwordHash;
        }
    }

}
