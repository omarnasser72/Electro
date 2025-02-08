using AutoMapper;
using Electro.APIs.DTOs;
using Electro.APIs.Models;
using Electro.Core.Entities;
using Electro.Core.Interfaces;
using Electro.Repository.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Electro.APIs.Controllers
{
    public class AuthenticationController : APIBaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;

        public AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        [HttpPost("Register/admin")]
        public async Task<ActionResult> Signup(RegisterModel registerModel)
        {
            var userNameAlreadyExist = await _userManager.FindByNameAsync(registerModel.UserName);
            var emailAlreadyExist = await _userManager.FindByEmailAsync(registerModel.Email);

            if (userNameAlreadyExist != null)
                return BadRequest(new
                {
                    Success = false,
                    Message = "Username already exists."
                });
            if (emailAlreadyExist != null)
                return BadRequest(new
                {
                    Success = false,
                    Message = "Email already exists."
                });

            var user = _mapper.Map<RegisterModel, Admin>(registerModel);

            if (ModelState.IsValid)
            {
                await _userManager.AddToRoleAsync(user, "Admin");

                var res = await _userManager.CreateAsync(user, user.Password);

                if (res.Succeeded)
                {
                    var UserDto = _mapper.Map<Admin, UserDTO>(user);
                    return Ok(new
                    {
                        Success = true,
                        UserDto
                    });
                }
                else
                {
                    foreach (var error in res.Errors)
                        ModelState.AddModelError("", error.Description);
                    return BadRequest(new
                    {
                        Success = false,
                        ModelState
                    });
                }
            }
            return BadRequest(new
            {
                Success = false,
                ModelState
            });
        }
        [HttpPost("Register")]
        public async Task<ActionResult> Register(RegisterModel registerModel)
        {
            var uerNameAlreadyExist = await _userManager.FindByNameAsync(registerModel.UserName);
            var emailAlreadyExist = await _userManager.FindByEmailAsync(registerModel.Email);

            if (uerNameAlreadyExist != null)
                return BadRequest("Username already exists.");

            if (emailAlreadyExist != null)
                return BadRequest("Email already exists.");

            if (ModelState.IsValid)
            {
                var user = _mapper.Map<RegisterModel, Customer>(registerModel);

                await _userManager.AddToRoleAsync(user, "Customer");

                var res = await _userManager.CreateAsync(user, user.Password);

                if (res.Succeeded)
                {
                    var userDTO = _mapper.Map<Customer, UserDTO>(user);
                    return Ok(new
                    {
                        Success = true,
                        userDTO
                    });
                }
                else
                {
                    foreach (var error in res.Errors)
                        ModelState.AddModelError("", error.Description);
                    return BadRequest(new
                    {
                        Success = false,
                        ModelState
                    });
                }
            }
            return BadRequest(new
            {
                Success = false,
                ModelState
            });
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginModel loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "User doesn't exist!");
                return NotFound(new
                {
                    Success = false,
                    Message = $"{loginModel.Email} doesn't exist!"
                });
            }

            var correctPass = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);
            if (correctPass.Succeeded)
            {
                var loginReq = await _signInManager.PasswordSignInAsync(user, loginModel.Password, true, false);
                if (loginReq.Succeeded)
                {
                    var customer = _mapper.Map<LoginModel, Customer>(loginModel);
                    var userDTO = _mapper.Map<Customer, UserDTO>(customer);
                    return Ok(new
                    {
                        Success = true,
                        user = userDTO
                    });
                }

            }
            else
                ModelState.AddModelError("errors", "Wrong Password!");

            return BadRequest(new
            {
                Success = false,
                ModelState
            });
        }

        [Authorize]
        [HttpGet("Logout")]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new
            {
                Success = true,
                Message = "Logged Out Successfully"
            });
        }
    }
}
