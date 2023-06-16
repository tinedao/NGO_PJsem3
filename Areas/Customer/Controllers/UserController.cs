using Microsoft.AspNetCore.Cors;
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
        [HttpPost]
        [Route("CreateUser")]
        public async Task<ActionResult> CreateUser(string username, string password, string imgUser, string email, string fullName, string address, string phoneNumber)
        {
            // Tạo đối tượng User với thông tin người dùng
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

            // Chuyển đối tượng User thành JSON
            var jsonUser = JsonConvert.SerializeObject(user);

            using (var httpClient = new HttpClient())
            {
                // Tạo nội dung yêu cầu POST với JSON người dùng
                var httpContent = new StringContent(jsonUser, Encoding.UTF8, "application/json");

                // Gửi yêu cầu POST tới API
                var response = await httpClient.PostAsync("https://localhost:7296/NGO/User/CreateUser", httpContent);

                // Kiểm tra phản hồi từ API
                if (response.IsSuccessStatusCode)
                {
                    // Người dùng đã được tạo thành công
                    return Ok("User created successfully");
                }
                else
                {
                    // Có lỗi xảy ra trong quá trình tạo người dùng
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return BadRequest(errorMessage);
                }
            }
        }


        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                // Chuyển mật khẩu thành mảng byte
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Tính toán giá trị băm của mật khẩu
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                // Chuyển giá trị băm thành chuỗi hexa
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                // Trả về chuỗi hexa làm mật khẩu đã mã hóa
                return builder.ToString();
            }
        }
    }
}
