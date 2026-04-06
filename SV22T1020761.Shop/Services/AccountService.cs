using System.Collections.Concurrent;
using System.Threading.Tasks;
using SV22T1020761.Models.Security;
using SV22T1020761.Shop.Services;
using SV22T1020761.BusinessLayers;
using SV22T1020761.DataLayers.SQLServer.Auth;
using SV22T1020761.DataLayers.SQLServer.Partner;
using SV22T1020761.Models.Partner;

namespace SV22T1020761.Shop.Services
{
    public static class AccountService
    {
        private static readonly ConcurrentDictionary<string, UserAccount> users = new ConcurrentDictionary<string, UserAccount>();
        private static readonly ConcurrentDictionary<string, string> passwords = new ConcurrentDictionary<string, string>();

        private static string? _connectionString => SV22T1020761.BusinessLayers.Configuration.ConnectionString;

        public static async Task<UserAccount?> ValidateUserAsync(string username, string password)
        {
            if (username == null) return null;

            // Try DB repo when connection string available
            if (!string.IsNullOrEmpty(_connectionString))
            {
                try
                {
                    var custRepo = new CustomerRepository(_connectionString);
                    var cust = await custRepo.GetByEmailAsync(username);
                    if (cust != null && cust.IsLocked != true)
                    {
                        // Get stored hash and verify
                        if (!string.IsNullOrEmpty(cust.Password) && PasswordHasher.VerifyHashedPassword(cust.Password, password))
                        {
                            return new UserAccount
                            {
                                UserId = cust.CustomerID.ToString(),
                                UserName = cust.Email,
                                DisplayName = cust.CustomerName,
                                Email = cust.Email,
                                Photo = string.Empty,
                                RoleNames = string.Empty,
                                CustomerName = cust.CustomerName,
                                ContactName = cust.ContactName,
                                Province = cust.Province ?? string.Empty,
                                Address = cust.Address ?? string.Empty,
                                Phone = cust.Phone ?? string.Empty,
                                IsLocked = cust.IsLocked
                            };
                        }
                    }
                }
                catch
                {
                    // ignore and fallback to in-memory
                }
            }

            if (users.TryGetValue(username.ToLowerInvariant(), out var user) && passwords.TryGetValue(username.ToLowerInvariant(), out var hash))
            {
                if (PasswordHasher.VerifyHashedPassword(hash, password))
                    return user;
            }
            return null;
        }

        public static UserAccount? GetUser(string username)
        {
            if (username == null) return null;

            if (users.TryGetValue(username.ToLowerInvariant(), out var u)) return u;

            // try DB lookup
            if (!string.IsNullOrEmpty(_connectionString))
            {
                try
                {
                    var custRepo = new CustomerRepository(_connectionString);
                    var cust = custRepo.GetByEmailAsync(username).GetAwaiter().GetResult();
                    if (cust != null)
                    {
                        return new UserAccount
                        {
                            UserId = cust.CustomerID.ToString(),
                            UserName = cust.Email,
                            DisplayName = cust.CustomerName,
                            Email = cust.Email,
                            Photo = string.Empty,
                            RoleNames = string.Empty,
                            CustomerName = cust.CustomerName,
                            ContactName = cust.ContactName,
                            Province = cust.Province ?? string.Empty,
                            Address = cust.Address ?? string.Empty,
                            Phone = cust.Phone ?? string.Empty,
                            IsLocked = cust.IsLocked
                        };
                    }
                }
                catch
                {
                    // ignore
                }
            }

            return null;
        }

        public static void UpdateUser(UserAccount model)
        {
            if (model == null || string.IsNullOrEmpty(model.UserName)) return;

            // Update in-memory
            users.AddOrUpdate(model.UserName.ToLowerInvariant(), model, (k, v) => model);

            // Update DB if available
            if (!string.IsNullOrEmpty(_connectionString))
            {
                try
                {
                    var custRepo = new CustomerRepository(_connectionString);
                    var cust = custRepo.GetByEmailAsync(model.UserName).GetAwaiter().GetResult();
                    if (cust != null)
                    {
                        cust.CustomerName = model.CustomerName;
                        cust.ContactName = model.ContactName;
                        cust.Email = model.Email;
                        cust.Phone = model.Phone;
                        cust.Province = model.Province;
                        cust.Address = model.Address;
                        custRepo.UpdateAsync(cust).GetAwaiter().GetResult();
                    }
                }
                catch
                {
                    // ignore
                }
            }
        }

        public static bool ValidatePassword(string username, string password)
        {
            if (username == null) return false;

            if (!string.IsNullOrEmpty(_connectionString))
            {
                try
                {
                    var custRepo = new CustomerRepository(_connectionString);
                    var cust = custRepo.GetByEmailAsync(username).GetAwaiter().GetResult();
                    if (cust != null && !string.IsNullOrEmpty(cust.Password))
                    {
                        return PasswordHasher.VerifyHashedPassword(cust.Password, password);
                    }
                }
                catch
                {
                    // fallback
                }
            }

            if (passwords.TryGetValue(username.ToLowerInvariant(), out var hash))
            {
                return PasswordHasher.VerifyHashedPassword(hash, password);
            }
            return false;
        }

        public static void ChangePassword(string username, string newPassword)
        {
            if (username == null) return;

            if (!string.IsNullOrEmpty(_connectionString))
            {
                try
                {
                    var repo = new CustomerAccountRepository(_connectionString);
                    var hashedPassword = PasswordHasher.HashPassword(newPassword);
                    repo.ChangePassword(username, hashedPassword).GetAwaiter().GetResult();
                }
                catch
                {
                    // fallback to in-memory
                }
            }

            var hash = PasswordHasher.HashPassword(newPassword);
            passwords.AddOrUpdate(username.ToLowerInvariant(), hash, (k, v) => hash);
        }

        public static async Task RegisterAsync(UserAccount model, string password)
        {
            if (model == null || string.IsNullOrEmpty(model.UserName))
                throw new InvalidOperationException("UserName không được để trống.");

            if (string.IsNullOrEmpty(_connectionString))
                throw new InvalidOperationException("Không có cấu hình kết nối Database. Đăng ký thất bại.");

            try
            {
                var custRepo = new CustomerRepository(_connectionString);
                var cust = new Customer
                {
                    CustomerName = string.IsNullOrWhiteSpace(model.CustomerName) ? model.UserName : model.CustomerName,
                    ContactName = model.ContactName ?? string.Empty,
                    Email = model.Email,
                    Phone = model.Phone ?? string.Empty,
                    Address = model.Address ?? string.Empty,
                    Province = null,
                    Password = PasswordHasher.HashPassword(password),
                    IsLocked = false
                };
                var id = await custRepo.AddAsync(cust);
                model.UserId = id.ToString();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Đăng ký thất bại. Lỗi Database: {ex.Message}", ex);
            }
        }
    }
}
