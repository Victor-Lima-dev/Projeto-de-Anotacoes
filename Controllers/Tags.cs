using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back_End.Context;
using Back_End.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Back_End.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Tags : ControllerBase
    {
        private readonly AppDbContext _context;

        public Tags(AppDbContext context)
        {
            _context = context;
        }

       //listar todas as tags
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> ListarTags()
        {
            var tags = await _context.Tags.ToListAsync();
            return tags;
        }

        [HttpPost("{idAnotacao}")]

        public async Task<ActionResult<Tag>> AnotacaoAddTag(Guid idAnotacao,[FromBody] Tag tag)
        {

            //procurar anotacao pelo id

            

        }

    }
}