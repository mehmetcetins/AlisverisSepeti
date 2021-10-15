using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepetiB.Server.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        [HttpGet]
        public async Task<List<string>> Get()
        {
            List<string> names = new List<string>();
            try
            {
                
                using (var context = new AlisverisSepeti.Models.AlisverisSepetiContext())
                {
                    int c = 0;
                    foreach (var urun in await context.Urunlers.AsNoTracking().Include(urun => urun.UrunlerDils.Where(d => d.Dil.Varsayilanmi == true)).ToListAsync())
                    {
                        names.Add(urun.UrunlerDils.FirstOrDefault()?.UrunAdi ?? "Ürün Adı Yok");

                    }
                }
                
            }
            catch(Exception e)
            {
                
                return names;
            }
            
            return names;
            
        }
    }
}
