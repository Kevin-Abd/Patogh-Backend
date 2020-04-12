namespace PatoghBackend.Services
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IdentityModel.Tokens.Jwt;
	using System.Linq;
	using System.Security.Claims;
	using System.Text;
	using System.Threading.Tasks;

	using Exceptions.General;
	using Exceptions.User;

	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Options;
	using Microsoft.IdentityModel.Tokens;

	using PatoghBackend.Contract;
	using PatoghBackend.Core;
	using PatoghBackend.Data;
	using PatoghBackend.Dto.Common;
	using PatoghBackend.Dto.Dorehami;
	using PatoghBackend.Dto.User;
	using PatoghBackend.Entity.Models;

	using Services.Common;

	public class UserService : Service, IUserService
	{
		private readonly ISmsService smsService;

		private readonly IImageService imageService;

		private readonly IDeleteService deleteService;

		private readonly AppSettings appSettings;

		private readonly MainDbContext dbContext;

		public UserService(
				MainDbContext dbContext,
				IOptions<AppSettings> appSettings,
				ISmsService smsService,
				IImageService imageService,
				IDeleteService deleteService)
		{
			this.dbContext = dbContext;
			this.appSettings = appSettings.Value;
			this.smsService = smsService;
			this.imageService = imageService;
			this.deleteService = deleteService;
		}

		public async Task RequestLoginToken(PhoneNumberWrapper phoneNumber)
		{
			var user = await dbContext.Users
				.AsTracking()
				.Where(r => r.PhoneNumber == phoneNumber.PhoneNumber)
				.FirstOrDefaultAsync();

			if (user == null)
			{
				user = new User
				{
					FirstName = string.Empty,
					LastName = string.Empty,
					Email = string.Empty,
					PhoneNumber = phoneNumber.PhoneNumber,
					LoginTokenValue = null,
					LoginTokenExpirationTime = DateTime.MinValue,
					SessionToken = null,
					JoinedDorehamies = new List<JoinUserDorehami>(),
				};

				await dbContext.Users.AddAsync(user);
			}

			user.LoginTokenValue = "1111";
			user.LoginTokenExpirationTime = DateTime.Now.AddHours(12);

			//TODO re-enable
			user.LoginTokenValue = new Random(DateTime.Now.Millisecond)
					.Next(1000, 9999)
					.ToString(CultureInfo.InvariantCulture);
			user.LoginTokenExpirationTime = DateTime.Now.AddMinutes(2);

			await dbContext.SaveChangesAsync();

			var resp = await smsService.SendSms(
				phoneNumber,
				$"کد ورود پاتوق {user.LoginTokenValue}");
		}

		public async Task<StringWrapper> Authenticate(RequestUserLogin request)
		{
			var user = await dbContext
				.Users
				.AsTracking()
				.Where(r => r.PhoneNumber == request.PhoneNumber)
				.FirstOrDefaultAsync();

			if (user == null)
			{
				throw new RecordNotFoundException(nameof(User));
			}

			if (user.LoginTokenValue == null)
			{
				throw new NoLoginTokenException();
			}

			if (DateTime.Now > user.LoginTokenExpirationTime)
			{
				throw new TokenExpiredException();
			}
			if (user.LoginTokenValue != request?.LoginToken)
			{
				throw new InvalidRequestException("Wrong token");
			}
			////--
			// authentication successful so generate jwt token
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(appSettings.Secret);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, user.Id.ToString(CultureInfo.InvariantCulture))
				}),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			var sessionToken = tokenHandler.WriteToken(token);
			////--

			user.LoginTokenValue = null;
			user.SessionToken = sessionToken;
			await dbContext.SaveChangesAsync();
			return new StringWrapper { Value = sessionToken };
		}

		public async Task<ObjectUserDetails> GetUserDetails(ClaimsPrincipal actingUser)
		{
			var user = await Helper.GetUserByClaim(dbContext, actingUser);

			var details = Helper.DetailFromUser(user);
			details.UserLevel = await Helper.CalculateUserLevel(dbContext, user.Id);

			return details;
		}

		public async Task<ObjectUserDetails> EditUserDetails(ObjectUserDetails request, ClaimsPrincipal actingUser)
		{
			var user = await Helper.GetUserByClaim(dbContext, actingUser, true);

			user.FirstName = request.FirstName;
			user.LastName = request.LastName;
			user.Email = request.Email;
			if (user.PhoneNumber != request.PhoneNumber)
			{
				throw new InvalidRequestException("can't change phonenumber");
				//throw new NotImplementedException("Cant change phonenumbers yet");
			}

			await dbContext.SaveChangesAsync();

			return Helper.DetailFromUser(user);
		}

		public async Task DeleteUser(ClaimsPrincipal actingUser)
		{
			var user = await Helper.GetUserByClaim(dbContext, actingUser);
			await deleteService.User(user.Id);
		}
		public async Task<IdStringWrapper> UploadProfilePicture(FormFileWrapper request, ClaimsPrincipal actingUser)
		{
			var user = await Helper.GetUserByClaim(dbContext, actingUser, true);
			var imageServiceReq = new Dto.Image.RequestUploadImage
			{
				File = request.File,
				UserId = user.Id,
				ImageType = Core.Enums.ImageType.Profile,
			};

			var res = await imageService.UploadImage(imageServiceReq);

			user.ProfilePictureId = long.Parse(res.IdString);
			await dbContext.SaveChangesAsync();
			return res;
		}

		#region joinCRUD

		public async Task JoinDorehamiAdd(IdStringWrapper idString, ClaimsPrincipal actingUser)
		{
			var user = await Helper.GetUserByClaim(dbContext, actingUser, true);

			var dId = long.Parse(idString.IdString);

			var dorehami = await dbContext.Dorehamies
				.AsNoTracking()
				.Where(r => r.Id == dId)
				.FirstOrDefaultAsync();

			if (dorehami == null)
			{
				throw new RecordNotFoundException(nameof(Dorehami));
			}

			var joinRecord = await dbContext.JoinUserDorehamies
				.AsNoTracking()
				.Where(r => r.UserId == user.Id && r.DorehamiId == dorehami.Id)
				.FirstOrDefaultAsync();

			if (joinRecord != null)
			{
				throw new DorehamiAlreadyJoinedException();
			}

			JoinUserDorehami newJoin = new JoinUserDorehami
			{
				DorehamiId = dorehami.Id,
				UserId = user.Id,
			};

			user.JoinedDorehamies.Add(newJoin);

			dbContext.JoinUserDorehamies.Add(newJoin);

			await dbContext.SaveChangesAsync();
		}

		public async Task JoinDorehamiRemove(IdStringWrapper idString, ClaimsPrincipal actingUser)
		{
			var user = await Helper.GetUserByClaim(dbContext, actingUser, true);

			var dId = long.Parse(idString.IdString);

			var dorehami = await dbContext.Dorehamies
				.AsNoTracking()
				.Where(r => r.Id == dId)
				.FirstOrDefaultAsync();

			if (dorehami == null)
			{
				throw new RecordNotFoundException(nameof(Dorehami));
			}

			var joinRecord = await dbContext.JoinUserDorehamies
				.AsTracking()
				.Where(r => r.UserId == user.Id && r.DorehamiId == dorehami.Id)
				.FirstOrDefaultAsync();

			if (joinRecord == null)
			{
				throw new DorehamiNotJoinedException();
			}

			dbContext.JoinUserDorehamies.Remove(joinRecord);

			await dbContext.SaveChangesAsync();
		}

		public async Task<List<ObjectDorehamiSummery>> JoinDorehamiGetSummery(ClaimsPrincipal actingUser)
		{
			var user = await Helper.GetUserByClaim(dbContext, actingUser);
			var qjoins = dbContext.JoinUserDorehamies
				.AsNoTracking()
				.AsQueryable()
				.Where(r => r.UserId == user.Id)
				.Include(r => r.Dorehami);

			var qlist = qjoins
				.Select(r => r.Dorehami)
				.Select(Helper.SummeryFromDorehami(user.Id));

			return await qlist.ToListAsync();
		}

		#endregion joinCRUD

		#region favCRUD

		public async Task FavDorehamiAdd(IdStringWrapper idString, ClaimsPrincipal actingUser)
		{
			var user = await Helper.GetUserByClaim(dbContext, actingUser, true);

			var dId = long.Parse(idString.IdString);

			var dorehami = await dbContext.Dorehamies
				.AsNoTracking()
				.Where(r => r.Id == dId)
				.FirstOrDefaultAsync();

			if (dorehami == null)
			{
				throw new RecordNotFoundException(nameof(Dorehami));
			}

			var joinRecord = await dbContext.FavUserDorehamies
				.AsNoTracking()
				.Where(r => r.UserId == user.Id && r.DorehamiId == dorehami.Id)
				.FirstOrDefaultAsync();

			if (joinRecord != null)
			{
				throw new DorehamiAlreadyFavoritedException();
			}

			FavUserDorehami newJoin = new FavUserDorehami
			{
				DorehamiId = dorehami.Id,
				UserId = user.Id,
			};

			user.FavoriteDorehamies.Add(newJoin);

			dbContext.FavUserDorehamies.Add(newJoin);

			await dbContext.SaveChangesAsync();
		}

		public async Task FavDorehamiRemove(IdStringWrapper idString, ClaimsPrincipal actingUser)
		{
			var user = await Helper.GetUserByClaim(dbContext, actingUser, true);

			var dId = long.Parse(idString.IdString);

			var dorehami = await dbContext.Dorehamies
				.AsNoTracking()
				.Where(r => r.Id == dId)
				.FirstOrDefaultAsync();

			if (dorehami == null)
			{
				throw new RecordNotFoundException(nameof(Dorehami));
			}

			var joinRecord = await dbContext.FavUserDorehamies
				.AsTracking()
				.Where(r => r.UserId == user.Id && r.DorehamiId == dorehami.Id)
				.FirstOrDefaultAsync();

			if (joinRecord == null)
			{
				throw new DorehamiNotFavoritedException();
			}

			dbContext.FavUserDorehamies.Remove(joinRecord);

			await dbContext.SaveChangesAsync();
		}

		public async Task<List<ObjectDorehamiSummery>> FavDorehamiGetSummery(ClaimsPrincipal actingUser)
		{
			var user = await Helper.GetUserByClaim(dbContext, actingUser);
			var qfavs = dbContext.FavUserDorehamies
				.AsNoTracking()
				.AsQueryable()
				.Where(r => r.UserId == user.Id)
				.Include(r => r.Dorehami);

			var qlist = qfavs
				.Select(r => r.Dorehami)
				.Select(Helper.SummeryFromDorehami(user.Id));

			return await qlist.ToListAsync();
		}

		#endregion favCRUD

		private bool disposed = false; // To detect redundant calls

		protected override void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects)
					dbContext.Dispose();
					smsService.Dispose();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposed = true;
				base.Dispose(disposing);
			}
		}
	}
}