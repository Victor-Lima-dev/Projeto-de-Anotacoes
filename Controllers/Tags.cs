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
    [Route("api/[controller]/[action]")]
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

        [HttpDelete("{idAnotacao}/{idTag}")]
        public async Task<ActionResult<Tag>> AnotacaoRemoveTag(Guid idAnotacao, Guid idTag)
        {
            var anotacao = await _context.Anotacoes.Include(a => a.Tags).FirstOrDefaultAsync(a => a.Id == idAnotacao);

            if (anotacao == null)
            {
                return NotFound("Anotação não encontrada");
            }

            // Verificar se a tag existe no bd

            var tagBd = await _context.Tags.FirstOrDefaultAsync(t => t.Id == idTag);

            if (tagBd == null)
            {
                return NotFound("Tag não encontrada");
            }

            var tag = anotacao.Tags.FirstOrDefault(t => t.Id == idTag);

            if (tag == null)
            {
                return NotFound("A anotação não possui essa tag");
            }

            anotacao.Tags.Remove(tag);

            _context.Entry(anotacao).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return tag;
    }

    [HttpDelete("{idTag}")]
    public async Task<ActionResult<Tag>> DeleteTag(Guid idTag)
    {
        var tag = await _context.Tags.Include(t => t.Anotacoes).FirstOrDefaultAsync(t => t.Id == idTag);

        if (tag == null)
        {
            return NotFound();
        }


        _context.Tags.Remove(tag);

        await _context.SaveChangesAsync();

        return tag;
    }

    [HttpGet("{titulo}")]

    public async Task<ActionResult<Tag>> GetTag(string titulo)
    {
        var tag = await _context.Tags.Include(t => t.Anotacoes).FirstOrDefaultAsync(t => t.Titulo == titulo);

        if (tag == null)
        {
            return NotFound();
        }

        return tag;

    }
}}