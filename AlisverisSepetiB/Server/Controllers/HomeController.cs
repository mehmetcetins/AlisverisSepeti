using AlisverisSepetiB.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlisverisSepeti.Models;
namespace AlisverisSepetiB.Server.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {


        [HttpGet("Categories")]
        public async Task<List<Category>> Categories()
        {
            int maxDepth;
            List<Urunkategoriler> kategoriler;
            using (var context = new AlisverisSepetiContext())
            {
                maxDepth = await context.Urunkategorilers.AsNoTracking().MaxAsync(kategori => kategori.Depth);
                kategoriler = await context.Urunkategorilers.AsNoTracking().OrderBy(kategori => kategori.Depth).ToListAsync();
                
            }
            List<Category> categories = new List<Category>();
            List<Category> _Categories = new List<Category>();
            Category temp;
            Category category;
            foreach(var kategori in kategoriler)
            {
                if (kategori.PkategoriId != null)
                {
                    temp = _Categories.Where(k => k.id == kategori.PkategoriId).First();
                    if (temp.SubCategory == null)
                    {
                        temp.SubCategory = new List<Category>();
                    }
                    category = new Category()
                    {
                        id = kategori.KategoriId,
                        categoryName = kategori.KategoriAdi,
                        SubCategory = null
                    };
                    _Categories.Add(category);
                    temp.SubCategory.Add(category);
                }
                else
                {
                    category = new Category()
                    {
                        id = kategori.KategoriId,
                        categoryName = kategori.KategoriAdi,
                        SubCategory = null
                    };
                    _Categories.Add(category);
                    categories.Add(category);
                }
                
            }
            return categories;
        }

        [HttpGet]
        public async Task<List<UrunDTO>> Get()
        {
            List<UrunDTO> uruns = new List<UrunDTO>();
            try
            {
                UrunDTO urunt;
                using (var context = new AlisverisSepeti.Models.AlisverisSepetiContext())
                {
                    int c = 0;
                    foreach (var urun in await context.Urunlers.AsNoTracking().Include(urun=> urun.Urunozellikleris).Include(urun => urun.Marka).Include(urun => urun.UrunlerDils.Where(d => d.Dil.Varsayilanmi == true)).ToListAsync())
                    {
                        urunt = new UrunDTO();
                        urunt.UrunAdi = urun.UrunlerDils.FirstOrDefault()?.UrunAdi ?? "Ürün Adi Yok";
                        urunt.MarkaAdi = urun.Marka.MarkaAdi;
                        urunt.Fiyati = urun.Urunozellikleris.Where(ozellik => ozellik.OzellikAdi.Equals("Price")).FirstOrDefault()?.Deger ?? "0 TL";
                        Console.Write(urunt);
                        uruns.Add(urunt);
                    }
                }
                
            }
            catch(Exception e)
            {
                Console.WriteLine(e.InnerException.Message);
                return uruns;
            }
            
            return uruns;
            
        }
    }
}
