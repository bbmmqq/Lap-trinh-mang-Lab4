using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace bai5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            var url =txtBoxurl.Text="https://nt106.uitiot.vn/auth/token";
            var username = txtBoxUsername.Text;
            var password = txtBoxPass.Text;

            using (var client = new HttpClient())
            {
                var content = new MultipartFormDataContent
                {
                    { new StringContent(username), "username" },
                    { new StringContent(password), "password" }
                };

                try
                {
                    var response = await client.PostAsync(url, content);
                    var responseString = await response.Content.ReadAsStringAsync();
                    var responseObject = JObject.Parse(responseString);

                    if (!response.IsSuccessStatusCode)
                    {
                        var detail = responseObject["detail"].ToString();
                        kq.Text = $"Detail: {detail}";
                        return;
                    }

                    var tokenType = responseObject["token_type"].ToString();
                    var accessToken = responseObject["access_token"].ToString();
                    kq.Text = $"Token Type: {tokenType}\nAccess Token: {accessToken}\n\nĐăng nhập thành công";
                }
                catch (Exception ex)
                {
                    kq.Text = $"Error: {ex.Message}";
                }
            }
        }


        private async void btnRegister_ClickAsync(object sender, EventArgs e)
        {
            var url = txtBoxurl.Text = "https://nt106.uitiot.vn/api/v1/user/signup";
            var username = txtBoxUsername.Text;
            var password = txtBoxPass.Text;
            var email=txtBoxEmail.Text;

            using (var client = new HttpClient())
            {
                var payload = new
                {
                    username = username,
                    password = password,
                    email= email,
                };

                var jsonPayload = JsonConvert.SerializeObject(payload);
                var content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

                try
                {
                    var response = await client.PostAsync(url, content);
                    var responseString = await response.Content.ReadAsStringAsync();
                    var responseObject = JObject.Parse(responseString);

                    if (!response.IsSuccessStatusCode)
                    {
                        var detail = responseObject["detail"]?[0]?["msg"]?.ToString();
                        kq.Text = $"Detail: {detail}";
                        return;
                    }

                    kq.Text = "Đăng ký thành công!";
                }
                catch (Exception ex)
                {
                    kq.Text = $"Error: {ex.Message}";
                }
            }
        }
    }
}