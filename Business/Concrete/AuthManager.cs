using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.Jwt;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        IUserService _userService;
        ITokenHelper _tokenHelper;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
        }

        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
          var accessToken=  _tokenHelper.CreateToken(user, _userService.GetClaims(user));
            return new SuccessDataResult<AccessToken>(accessToken, Messages.AccessTokenCreated);
        }

        public IDataResult<User> Login(UserForLoginDto userForLoginDto)
        {
            var UserToCheck = _userService.GetByMail(userForLoginDto.Email);
            if (UserToCheck==null)
            {
                return new ErrorDataResult<User>(UserToCheck, Messages.UserNotFound);
            }
            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password,UserToCheck.PasswordHash,UserToCheck.PasswordSalt))
            {
                return new ErrorDataResult<User>(UserToCheck, Messages.PasswordError);
            }
            return new SuccessDataResult<User>(UserToCheck, Messages.SuccessfulLogin);
        }

        public IDataResult<User> Register(UserForRegisterDto userForRegisterDto)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(userForRegisterDto.Password, out passwordHash, out passwordSalt);

            var user = new User {
            Email=userForRegisterDto.Email,
            FirstName=userForRegisterDto.FirstName,
            LastName=userForRegisterDto.LastName,
            PasswordHash= passwordHash,
            PasswordSalt= passwordSalt,
            Status=true};

            _userService.Add(user);
            return new SuccessDataResult<User>(user, Messages.UserRegistered);
        }

        public IResult UserExists(string email)
        {
            if (_userService.GetByMail(email)!=null)
            {
                return new SuccessResult( Messages.UserFound);
            }
            return new ErrorResult(Messages.UserNotFound);
        }
    }
}
