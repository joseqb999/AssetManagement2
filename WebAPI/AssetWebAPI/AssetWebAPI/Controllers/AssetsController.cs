using AssetWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssetWebAPI.DTO;
using AutoMapper;
using AssetWebAPI.AutoMapper;

namespace AssetWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly MyAssetsDBContext _context;
        private readonly IMapper mapper;
        public AssetsController(MyAssetsDBContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ListDTO>>> GetAssets()
        {
            var query = from t1 in _context.Assets
                        join t2 in _context.Models on t1.ModelId equals t2.ModelId
                        join t3 in _context.Manufacturers on t1.ManuFactId equals t3.ManuFactId
                        join t4 in _context.Colors on t1.ColorId equals t4.ColorId
                        select new ListDTO
                        {
                            Name = t1.Name,
                            ManufacturerName = t3.Name,
                            ModelName = t2.Name,
                            ColorName = t4.Name,
                            PurchaseDate = t1.PurchaseDate,
                            Price = t1.Price,
                            InUse = t1.InUse
                        };


            var joinresult = await query.ToListAsync();

            if (joinresult == null)
            {
                return NotFound();
            }
            var result = new List<ListDTO>();
            result = mapper.Map<List<ListDTO>>(joinresult);
            return result;
        }

    }
}
