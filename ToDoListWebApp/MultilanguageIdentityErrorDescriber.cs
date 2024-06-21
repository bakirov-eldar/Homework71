using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace ToDoListWebApp;
public class MultilanguageIdentityErrorDescriber : IdentityErrorDescriber
{
    private readonly IStringLocalizer<MultilanguageIdentityErrorDescriber> _localizer;

    public MultilanguageIdentityErrorDescriber(IStringLocalizer<MultilanguageIdentityErrorDescriber> localizer)
    {
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
    }

    private IdentityError LocalizedError(string code, Func<IdentityError> defaultError, params object[] formatArgs)
    {
        var localizedDescription = _localizer[code, formatArgs];
        if (!string.IsNullOrEmpty(localizedDescription))
        {
            return new IdentityError { Code = code, Description = localizedDescription };
        }
        return defaultError();
    }

    public override IdentityError DefaultError() =>
        LocalizedError(nameof(DefaultError), () => base.DefaultError());

    public override IdentityError ConcurrencyFailure() =>
        LocalizedError(nameof(ConcurrencyFailure), () => base.ConcurrencyFailure());

    public override IdentityError PasswordMismatch() =>
        LocalizedError(nameof(PasswordMismatch), () => base.PasswordMismatch());

    public override IdentityError InvalidToken() =>
        LocalizedError(nameof(InvalidToken), () => base.InvalidToken());

    public override IdentityError LoginAlreadyAssociated() =>
        LocalizedError(nameof(LoginAlreadyAssociated), () => base.LoginAlreadyAssociated());

    public override IdentityError InvalidUserName(string userName) =>
        LocalizedError(nameof(InvalidUserName), () => base.InvalidUserName(userName), userName);

    public override IdentityError InvalidEmail(string email) =>
        LocalizedError(nameof(InvalidEmail), () => base.InvalidEmail(email), email);

    public override IdentityError DuplicateUserName(string userName) =>
        LocalizedError(nameof(DuplicateUserName), () => base.DuplicateUserName(userName), userName);

    public override IdentityError DuplicateEmail(string email) =>
        LocalizedError(nameof(DuplicateEmail), () => base.DuplicateEmail(email), email);

    public override IdentityError InvalidRoleName(string role) =>
        LocalizedError(nameof(InvalidRoleName), () => base.InvalidRoleName(role), role);

    public override IdentityError DuplicateRoleName(string role) =>
        LocalizedError(nameof(DuplicateRoleName), () => base.DuplicateRoleName(role), role);

    public override IdentityError UserAlreadyHasPassword() =>
        LocalizedError(nameof(UserAlreadyHasPassword), () => base.UserAlreadyHasPassword());

    public override IdentityError UserLockoutNotEnabled() =>
        LocalizedError(nameof(UserLockoutNotEnabled), () => base.UserLockoutNotEnabled());

    public override IdentityError UserAlreadyInRole(string role) =>
        LocalizedError(nameof(UserAlreadyInRole), () => base.UserAlreadyInRole(role), role);

    public override IdentityError UserNotInRole(string role) =>
        LocalizedError(nameof(UserNotInRole), () => base.UserNotInRole(role), role);

    public override IdentityError PasswordTooShort(int length) =>
        LocalizedError(nameof(PasswordTooShort), () => base.PasswordTooShort(length), length);

    public override IdentityError PasswordRequiresNonAlphanumeric() =>
        LocalizedError(nameof(PasswordRequiresNonAlphanumeric), () => base.PasswordRequiresNonAlphanumeric());

    public override IdentityError PasswordRequiresDigit() =>
        LocalizedError(nameof(PasswordRequiresDigit), () => base.PasswordRequiresDigit());

    public override IdentityError PasswordRequiresLower() =>
        LocalizedError(nameof(PasswordRequiresLower), () => base.PasswordRequiresLower());

    public override IdentityError PasswordRequiresUpper() =>
        LocalizedError(nameof(PasswordRequiresUpper), () => base.PasswordRequiresUpper());
}
