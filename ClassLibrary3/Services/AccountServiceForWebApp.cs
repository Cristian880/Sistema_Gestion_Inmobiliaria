using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Sis_Inmobiliaria.Core.Application.Dtos.Email;
using Sis_Inmobiliaria.Core.Application.Dtos.User;
using Sis_Inmobiliaria.Core.Application.Interfaces;
using Sis_Inmobiliaria.Infrastructure.Identity.Entities;
using System.Text;

namespace Sis_Inmobiliaria.Infrastructure.Identity.Services
{
    public class AccountServiceForWebApp : IAccountServiceForWebApp
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;

        public AccountServiceForWebApp(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }
        public async Task<LoginResponseDto> AuthenticateAsync(LoginDto loginDto)
        {
            LoginResponseDto response = new()
            {
                Email = "",
                Id = "",
                LastName = "",
                Name = "",
                UserName = "",
                HasError = false,
                Errors = []
            };

            var user = await _userManager.FindByNameAsync(loginDto.UserName);

            if (user == null)
            {
                response.HasError = true;
                response.Errors.Add($"There is no acccount registered with this username: {loginDto.UserName}");
                return response;
            }

            if (!user.EmailConfirmed)
            {
                response.HasError = true;
                response.Errors.Add($"This account {loginDto.UserName} is not active, you should check your email");
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName ?? "", loginDto.Password, false, true);

            if (!result.Succeeded)
            {
                response.HasError = true;
                if (result.IsLockedOut)
                {
                    response.Errors.Add($"Your account {loginDto.UserName} has been locked due to multiple failed attempts." +
                        $" Please try again in 10 minutes. If you don’t remember your password, you can go through the password " +
                        $"reset process.");
                }
                else
                {
                    response.Errors.Add($"these credentials are invalid for this user: {user.UserName}");
                }
                return response;
            }

            var rolesList = await _userManager.GetRolesAsync(user);

            response.Id = user.Id;
            response.Email = user.Email ?? "";
            response.UserName = user.UserName ?? "";
            response.Name = user.Name;
            response.LastName = user.LastName;
            response.IsVerified = user.EmailConfirmed;
            response.Roles = rolesList.ToList();

            return response;
        }
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task<RegisterResponseDto> RegisterUser(SaveUserDto saveDto, string origin)
        {
            RegisterResponseDto response = new()
            {
                Email = "",
                Id = "",
                LastName = "",
                Name = "",
                UserName = "",
                HasError = false,
                Errors = []
            };

            var userWithSameUserName = await _userManager.FindByNameAsync(saveDto.UserName);
            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Errors.Add($"this username: {saveDto.UserName} is already taken.");
                return response;
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(saveDto.Email);
            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.Errors.Add($"this email: {saveDto.Email} is already taken.");
                return response;
            }

            AppUser user = new AppUser()
            {
                Name = saveDto.Name,
                LastName = saveDto.LastName,
                Email = saveDto.Email,
                UserName = saveDto.UserName,
                ProfileImage = saveDto.ProfileImage,
                EmailConfirmed = false,
                PhoneNumber = saveDto.Phone
            };

            var result = await _userManager.CreateAsync(user, saveDto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, saveDto.Role);
                string verificationUri = await GetVerificationEmailUri(user, origin);
                await _emailService.SendAsync(new EmailRequestDto()
                {
                    To = saveDto.Email,
                    HtmlBody = $"Please confirm yout account visiting this URL {verificationUri}",
                    Subject = "Confirm registration"
                });

                var rolesList = await _userManager.GetRolesAsync(user);

                response.Id = user.Id;
                response.Email = user.Email ?? "";
                response.UserName = user.UserName ?? "";
                response.Name = user.Name;
                response.LastName = user.LastName;
                response.IsVerified = user.EmailConfirmed;
                response.Roles = rolesList.ToList();

                return response;
            }
            else
            {
                response.HasError = true;
                response.Errors.AddRange(result.Errors.Select(s => s.Description).ToList());
                return response;
            }
        }
        public async Task<EditResponseDto> EditUser(SaveUserDto saveDto, string origin, bool? isCreated = false)
        {
            bool isNotcreated = !isCreated ?? false;
            EditResponseDto response = new()
            {
                Email = "",
                Id = "",
                LastName = "",
                Name = "",
                UserName = "",
                HasError = false,
                Errors = []
            };

            var userWithSameUserName = await _userManager.Users.FirstOrDefaultAsync(w => w.UserName == saveDto.UserName && w.Id != saveDto.Id);
            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Errors.Add($"this username: {saveDto.UserName} is already taken.");
                return response;
            }

            var userWithSameEmail = await _userManager.Users.FirstOrDefaultAsync(w => w.Email == saveDto.Email && w.Id != saveDto.Id);
            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.Errors.Add($"this email: {saveDto.Email} is already taken.");
                return response;
            }

            var user = await _userManager.FindByIdAsync(saveDto.Id);

            if (user == null)
            {
                response.HasError = true;
                response.Errors.Add($"There is no acccount registered with this user");
                return response;
            }

            user.Name = saveDto.Name;
            user.LastName = saveDto.LastName;
            user.UserName = saveDto.UserName;
            user.ProfileImage = string.IsNullOrWhiteSpace(saveDto.ProfileImage) ? user.ProfileImage : saveDto.ProfileImage;
            user.EmailConfirmed = user.EmailConfirmed && user.Email == saveDto.Email;
            user.Email = saveDto.Email;
            user.PhoneNumber = saveDto.Phone;

            if (!string.IsNullOrWhiteSpace(saveDto.Password) && isNotcreated)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resultChange = await _userManager.ResetPasswordAsync(user, token, saveDto.Password);

                if (resultChange != null && !resultChange.Succeeded)
                {
                    response.HasError = true;
                    response.Errors.AddRange(resultChange.Errors.Select(s => s.Description).ToList());
                    return response;
                }
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var rolesList = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, rolesList.ToList());

                await _userManager.AddToRoleAsync(user, saveDto.Role);


                if (!user.EmailConfirmed && isNotcreated)
                {
                    string verificationUri = await GetVerificationEmailUri(user, origin);
                    await _emailService.SendAsync(new EmailRequestDto()
                    {
                        To = saveDto.Email,
                        HtmlBody = $"Please confirm yout account visiting this URL {verificationUri}",
                        Subject = "Confirm registration"
                    });
                }

                var updatedRolesList = await _userManager.GetRolesAsync(user);

                response.Id = user.Id;
                response.Email = user.Email ?? "";
                response.UserName = user.UserName ?? "";
                response.Name = user.Name;
                response.LastName = user.LastName;
                response.IsVerified = user.EmailConfirmed;
                response.Roles = updatedRolesList.ToList();

                return response;
            }
            else
            {
                response.HasError = true;
                response.Errors.AddRange(result.Errors.Select(s => s.Description).ToList());
                return response;
            }
        }

        public async Task<UserResponseDto> ForgotPasswordAsync(ForgotPasswordRequestDto request)
        {
            UserResponseDto response = new() { HasError = false, Errors = [] };

            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                response.HasError = true;
                response.Errors.Add($"There is no acccount registered with this username {request.UserName}");
                return response;
            }

            var resetUri = await GetResetPasswordUri(user, request.Origin);
            user.EmailConfirmed = false;//desactiva el email confirmado para que el usuario pueda resetear su contraseña

            await _userManager.UpdateAsync(user);

            await _emailService.SendAsync(new EmailRequestDto()
            {
                To = user.Email,
                HtmlBody = $"Please reset your password account visiting this URL {resetUri}",
                Subject = "Reset password"
            });

            return response;
        }

        public async Task<UserResponseDto> ResetPasswordAsync(ResetPasswordRequestDto request)
        {
            UserResponseDto response = new() { HasError = false, Errors = [] };

            var user = await _userManager.FindByIdAsync(request.Id);

            if (user == null)
            {
                response.HasError = true;
                response.Errors.Add($"There is no acccount registered with this user");
                return response;
            }

            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            var result = await _userManager.ResetPasswordAsync(user, token, request.Password);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Errors.AddRange(result.Errors.Select(s => s.Description).ToList());
                return response;
            }

            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            return response;
        }
        public async Task<UserResponseDto> DeleteAsync(string id)
        {
            UserResponseDto response = new() { HasError = false, Errors = [] };
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                response.HasError = true;
                response.Errors.Add($"There is no acccount registered with this user");
                return response;
            }

            await _userManager.DeleteAsync(user);

            return response;
        }
        public async Task<UserDto?> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return null;
            }

            var rolesList = await _userManager.GetRolesAsync(user);

            var userDto = new UserDto()
            {
                Id = user.Id,
                Email = user.Email ?? "",
                LastName = user.LastName,
                Name = user.Name,
                UserName = user.UserName ?? "",
                ProfileImage = user.ProfileImage,
                Phone = user.PhoneNumber,
                isVerified = user.EmailConfirmed,
                Role = rolesList.FirstOrDefault() ?? ""
            };

            return userDto;
        }
        public async Task<UserDto?> GetUserById(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);

            if (user == null)
            {
                return null;
            }

            var rolesList = await _userManager.GetRolesAsync(user);

            var userDto = new UserDto()
            {
                Id = user.Id,
                Email = user.Email ?? "",
                LastName = user.LastName,
                Name = user.Name,
                UserName = user.UserName ?? "",
                ProfileImage = user.ProfileImage,
                Phone = user.PhoneNumber,
                isVerified = user.EmailConfirmed,
                Role = rolesList.FirstOrDefault() ?? ""
            };

            return userDto;
        }
        public async Task<UserDto?> GetUserByUserName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return null;
            }

            var rolesList = await _userManager.GetRolesAsync(user);

            var userDto = new UserDto()
            {
                Id = user.Id,
                Email = user.Email ?? "",
                LastName = user.LastName,
                Name = user.Name,
                UserName = user.UserName ?? "",
                ProfileImage = user.ProfileImage,
                Phone = user.PhoneNumber,
                isVerified = user.EmailConfirmed,
                Role = rolesList.FirstOrDefault() ?? ""
            };

            return userDto;
        }
        public async Task<List<UserDto>> GetAllUser(bool? isActive = true)
        {
            List<UserDto> listUsersDtos = [];

            var users = _userManager.Users;

            if (isActive != null && isActive == true)
            {
                users = users.Where(w => w.EmailConfirmed);
            }

            var listUser = await users.ToListAsync();

            foreach (var item in listUser)
            {
                var roleList = await _userManager.GetRolesAsync(item);

                listUsersDtos.Add(new UserDto()
                {
                    Id = item.Id,
                    Email = item.Email ?? "",
                    LastName = item.LastName,
                    Name = item.Name,
                    UserName = item.UserName ?? "",
                    ProfileImage = item.ProfileImage,
                    Phone = item.PhoneNumber,
                    isVerified = item.EmailConfirmed,
                    Role = roleList.FirstOrDefault() ?? ""
                });
            }

            return listUsersDtos;
        }
        public async Task<string> ConfirmAccountAsync(string userId, string token)
        {

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return "There is no acccount registered with this user";
            }

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));// se descodifica el token de Base64 URL-safe a UTF-8
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return $"Account confirmed for {user.Email}. You can now use the app";
            }
            else
            {
                return $"An error occurred while confirming this email {user.Email}";
            }
        }

        #region "Private methods"

        private async Task<string> GetVerificationEmailUri(AppUser user, string origin)
        {
            //Genera un token único para confirmar el correo del usuario (puede contener caracteres especiales)
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // 2. Codifica el token a Base64 URL-safe (para que sea seguro enviarlo por URL)
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            // 3. Define la ruta de la acción que va a recibir la confirmación (ejemplo: LoginController > ConfirmEmail)
            var route = "Login/ConfirmEmail";

            // 4. Combina el origen (por ejemplo: https://localhost:58296) con la ruta para formar la URL base
            var completeUrl = new Uri(string.Concat(origin, "/", route));// origin = https://localhost:58296 route=Login/ConfirmEmail

            // 5. Agrega el parámetro "userId" con el ID del usuario a la URL como query string
            var verificationUri = QueryHelpers.AddQueryString(completeUrl.ToString(), "userId", user.Id);

            // 6. Agrega también el parámetro "token" (ya codificado) como parte de la URL
            verificationUri = QueryHelpers.AddQueryString(verificationUri.ToString(), "token", token);

            return verificationUri;
        }
        private async Task<string> GetResetPasswordUri(AppUser user, string origin)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var route = "Login/ResetPassword";
            var completeUrl = new Uri(string.Concat(origin, "/", route));// origin = https://localhost:58296 route=Login/ConfirmEmail
            var resetUri = QueryHelpers.AddQueryString(completeUrl.ToString(), "userId", user.Id);
            resetUri = QueryHelpers.AddQueryString(resetUri.ToString(), "token", token);

            return resetUri;
        }
        #endregion
    }
}
