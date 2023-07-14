using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NGO_PJsem3.Models;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

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
        private readonly ILogger<UserController> _logger;

        //login
        [HttpPost("Login")]
        public async Task<ActionResult> Login(string username, string password)
        {
            try
            {
                // Gửi yêu cầu đăng nhập đến API
                var loginRequest = new
                {
                    Username = username,
                    Password = password
                };

                var jsonRequest = JsonConvert.SerializeObject(loginRequest);
                var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("https://localhost:7296/NGO/User/Login", httpContent);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<Users>(result);
                    

                    return Ok("Login successful");
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Unauthorized("Invalid username or password");
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return BadRequest(errorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while logging in.");
                return StatusCode(500, "Internal server error");
            }
        }


        public UserController(IHttpClientFactory httpClientFactory, IPasswordHasher<Users> passwordHasher, ILogger<UserController> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        [HttpGet("checkExistence")]
        public async Task<IActionResult> CheckExistence(string email, string phoneNumber)
        {
            try
            {
                // Kiểm tra sự tồn tại của email và số điện thoại
                bool isEmailExists = await CheckEmailExistence(email);
                bool isPhoneNumberExists = await CheckPhoneNumberExistence(phoneNumber);

                return Ok(new { EmailExists = isEmailExists, PhoneNumberExists = isPhoneNumberExists });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking existence.");
                return StatusCode(500, "Internal server error");
            }
        }

        private async Task<bool> CheckEmailExistence(string email)
        {
            if (!IsValidEmail(email))
            {
                return false;
            }

            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking email existence.");
                return false;
            }
        }

        private async Task<bool> CheckPhoneNumberExistence(string phoneNumber)
        {
            if (!IsValidPhoneNumber(phoneNumber))
            {
                return false;
            }

            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking phone number existence.");
                return false;
            }
        }
        [HttpPost("CreateUser")]
        public async Task<ActionResult> CreateUser(string username, string password, string imgUser, string email, string fullName, string address, string phoneNumber)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating user.");
                return StatusCode(500, "Internal server error");
            }
        }

        private string HashPassword(string password)
        {
            try
            {
                // Mã hóa mật khẩu sử dụng bcrypt
                var passwordHash = _passwordHasher.HashPassword(null, password);
                return passwordHash;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while hashing password.");
                return null;
            }
        }

        private bool IsValidEmail(string email)
        {
            // Kiểm tra định dạng email bằng regex
            const string emailRegexPattern = @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$";
            var regex = new Regex(emailRegexPattern);
            return regex.IsMatch(email);
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            // Kiểm tra định dạng số điện thoại bằng regex
            const string phoneNumberRegexPattern = @"^(0[0-9]{9})$";
            var regex = new Regex(phoneNumberRegexPattern);
            return regex.IsMatch(phoneNumber);
        }
    }
}
