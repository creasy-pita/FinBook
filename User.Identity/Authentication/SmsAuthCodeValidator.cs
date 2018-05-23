using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using User.Identity.Services;

namespace User.Identity.Authentication
{
    //public class SmsAuthCodeGrantType : IExtensionGrantValidator
    public class SmsAuthCodeValidator : IExtensionGrantValidator
    {
        public string GrantType => "sms_auth_code";

        public IUserService _userService;
        public IAuthCodeService _authCodeService;

        public SmsAuthCodeValidator(IUserService userService, IAuthCodeService authCodeService)
        {
            _userService = userService;
            _authCodeService = authCodeService;
        }

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var phone = context.Request.Raw["phone"];
            var authcode = context.Request.Raw["authcode"];

            if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(authcode))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
                return;
            }
            if (!_authCodeService.Validate(phone, authcode))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
                return;
            }
            var userId = await _userService.CheckOrCreate(phone);

            if (userId<=0)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
                return;
            }
            context.Result = new GrantValidationResult(userId.ToString(), GrantType);
        }
    }
}
