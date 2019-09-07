using System;
using System.Threading.Tasks;
using datingApp.API.Data;
using Microsoft.EntityFrameworkCore;

namespace datingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
           private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<User> Register(User user, string password)
        {
            User user1=new User();
            byte[] passwordHash,passwordSalt;
            CreatePasswordHash(password,out passwordHash,out passwordSalt);
            user.Passwordhash=passwordHash;
            user.Passwordstyle=passwordSalt;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
           
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
          using(var hmac =new System.Security.Cryptography.HMACSHA512()){
passwordSalt=hmac.Key;
passwordHash=hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

          }
        }

        public async Task<User> Login(string username, string password)
        {
            var user =await _context.Users.FirstOrDefaultAsync(x=>x.Username==username);
            if(user==null){
                return null;
            }
            if(VerifyPasswordHash(password,user.Passwordhash,user.Passwordstyle)){
            return user;
                
            }return null;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordhash, byte[] passwordstyle)
        { 
         byte[] passwordHash;

            using(var hmac =new System.Security.Cryptography.HMACSHA512(passwordstyle)){
passwordHash=hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

for(int i=0;i<passwordHash.Length;i++){
    if(passwordHash[i]!=passwordhash[i]){
        return false;
    }
    }return true;
}
            }
        

        public async Task<bool> UserExists(string usename)
        {
            if(await _context.Users.AnyAsync(x=>x.Username==usename)){
return true;
            }
            return false;
        }
     
    }
}