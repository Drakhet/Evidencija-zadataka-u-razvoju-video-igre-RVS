using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace SlojPodataka.TehnoloskeKlase
{
    public static class FunkcijeLozinke
    {
        public static string GenerisiSalt(int duzina = 16)
        {
            byte[] saltBajtovi = new byte[duzina];
            using (var randomBroj = RandomNumberGenerator.Create())
            {
                randomBroj.GetBytes(saltBajtovi);
            }
            return Convert.ToBase64String(saltBajtovi);
        }

        public static string IzracunajHash(string lozinka, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                string lozinkaSaSaltom = lozinka + salt;
                byte[] hashBajtovi = sha256.ComputeHash(
                    Encoding.UTF8.GetBytes(lozinkaSaSaltom));
                return Convert.ToBase64String(hashBajtovi);
            }
        }

        public static bool ProveriLozinku(string lozinka, string salt, string hash)
        {
            string izracunatiHash = IzracunajHash(lozinka, salt);
            return izracunatiHash == hash;
        }
    }
}