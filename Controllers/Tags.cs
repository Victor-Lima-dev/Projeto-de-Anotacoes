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
            //verificar se a tag já existe na anotacao
            //verificar se a tag já existe no banco


            var anotacao = await _context.Anotacoes.Include(a => a.Tags).FirstOrDefaultAsync(a => a.Id == idAnotacao);

            if (anotacao == null)
            {
                return NotFound();
            }

            var tagExistente = await _context.Tags.FirstOrDefaultAsync(t => t.Titulo == tag.Titulo);

            if (tagExistente != null)
            {
                // Use a tag existente em vez de criar uma nova
                anotacao.Tags.Add(tagExistente);
            }
            else
            {
                // Se a tag não existir, adicione a nova tag à lista
                anotacao.Tags.Add(tag);

                // Como é uma nova tag, associe a anotação a ela

                tag.Anotacoes.Add(anotacao);

                _context.Tags.Add(tag);
            }

            //modificar o estado da anotacao

            _context.Entry(anotacao).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return tag;
            

        }

    }
}