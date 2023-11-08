using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMediaClean.APPLICATION.Contracts;
using SocialMediaClean.APPLICATION.Services;
using SocialMediaClean.DOMAIN.Models.DTOs;
using SocialMediaClean.INFRASTRUCTURE.Interfaces;
using SocialMediaClean.INFRASTRUCTURE.Models;

namespace SocialMediaClean.API.Controllers
{
    [Route("api/v1/account/")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase { 
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;
        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;

        }
        [HttpGet("reset/password")]
        public async Task<ActionResult> SendResetPasswordMailAsync([FromForm]string email)
        {
            _logger.LogInformation($"Send reset password mail at email {email}");
            try
            {
                await _accountService.SendPasswordResetMailAsync(email);
                return Ok("Mail succesfully sent!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception been raised at the payload {email} with the exception message {ex.Message}");
                throw;
            }
            
        }
        
        [HttpPost("confirm/password/{token}")]
        public async Task<ActionResult> ConfirmResetPasswordAsync(string token, [FromBody] ChangePasswordDTO password)
        {
            _logger.LogInformation("Received the confimration request!");
            try
            { 
            if(!_accountService.VerifyPasswordResetHmacCode(token, out Int32 userId))
            {
                _logger.LogWarning($"Token {token} invalid");
                return BadRequest("Invalid, tampered, or expired code used.");
            }
            await _accountService.ChangePassword(password, userId);
            return Ok("Password changed succesfully!");
            }
            catch(Exception ex) 
            {
                var objString = JsonConvert.SerializeObject(password);
                _logger.LogError($"Exception raised at token: {token} and payload: {objString} with exception message {ex.Message} ");
                throw;
            }

        }

        [HttpGet("resend/email/{email}")]
        public async Task<ActionResult> ResendEmailConfirmationAsync(string email)
        {
            _logger.LogInformation("Received the resend confimration request!");
            try
            { 
                await _accountService.ResendEmailConfirmationTokenAsync(email);
                _logger.LogInformation($"Succesfully sent the confirmation email at {email}");
                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Exception raised at the payload: {email} with the exception message :{ex.Message}");
                throw;
            }
        }

        [HttpPost("confirm/email/{email}/{token}")]
        public async Task<ActionResult> ConfirmEmailAsync(string email,string token)
        {
            _logger.LogInformation($"Confirm email request");
            try
            {
                var result = await _accountService.VerifyEmailAsync(email, token);
                if (!result.Success)
                {
                    _logger.LogWarning($"Confirmation failed {email} with token: {token}");
                    return BadRequest("Invalid, tampered, or expired code used.");
                }
                _logger.LogInformation($"Email confirmed succesfully {email}");
                return Ok("Email confirmed succesfully!");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception raised at the {email} with token:{token} with the exception message {ex.Message}");
                throw;
            }
        }

    }
}
