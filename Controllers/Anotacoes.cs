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
    public class Anotacoes : ControllerBase
    {
        private readonly AppDbContext _context;

        public Anotacoes(AppDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        public async Task<ActionResult<Anotacao>> CriarAnotacao([FromBody] Anotacao anotacao)
        {
            var tagsFinal = new List<Tag>();

            foreach (var tag in anotacao.Tags)
            {
                var tagExistente = await _context.Tags.FirstOrDefaultAsync(t => t.Titulo == tag.Titulo);

                if (tagExistente != null)
                {
                    // Use a tag existente em vez de criar uma nova
                    tagsFinal.Add(tagExistente);
                }
                else
                {
                    // Se a tag não existir, adicione a nova tag à lista
                    tagsFinal.Add(tag);
                    // Como é uma nova tag, associe a anotação a ela
                    tag.Anotacoes.Add(anotacao);
                }
            }

            // Atualize as tags da anotação para as tags finais (existentes ou novas)
            anotacao.Tags = tagsFinal;

            _context.Anotacoes.Add(anotacao);

            await _context.SaveChangesAsync();

            return anotacao;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<Anotacao>>> ListarAnotacoes()
        {
            var anotacoes = await _context.Anotacoes.Select(a => new Anotacao { Id = a.Id, Titulo = a.Titulo, Conteudo = a.Conteudo, Tags = a.Tags }).ToListAsync();
            //var tags = await _context.Tags.Select(t => new Tag { Id = t.Id, Titulo = t.Titulo, Anotacoes = t.Anotacoes }).ToListAsync();

            return anotacoes;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Anotacao>> GetAnotacao(Guid id)
        {
            var anotacao = await _context.Anotacoes
                                         .Include(a => a.Tags)
                                         .FirstOrDefaultAsync(a => a.Id == id);

            if (anotacao == null)
            {
                return NotFound();
            }

            return anotacao;
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> AtualizarAnotacao(Guid id, Anotacao anotacao)
        {

            if (id != anotacao.Id)
            {
                return BadRequest("Id da anotação diferente do id informado");
            }

            _context.Entry(anotacao).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(anotacao);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarAnotacao(Guid id)
        {
            var anotacao = await _context.Anotacoes.FindAsync(id);

            if (anotacao == null)
            {
                return NotFound("Anotação não encontrada");
            }

            _context.Anotacoes.Remove(anotacao);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]

        public async Task<IActionResult> DeletarTodasAnotacoes()
        {

            var anotacoes = await _context.Anotacoes.ToListAsync();

            if (anotacoes.Count == 0)
            {
                return NotFound("Não há anotações para deletar");
            }

            _context.Anotacoes.RemoveRange(anotacoes);

            var tags = await _context.Tags.ToListAsync();

            _context.Tags.RemoveRange(tags);


            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("titulo")]

        public async Task<ActionResult<IEnumerable<Anotacao>>> ListarAnotacoesPorTitulo(string titulo)
        {
            var anotacoes = await _context.Anotacoes
        .Where(a => a.Titulo.Contains(titulo))
        .Select(a => new Anotacao
        {
            Id = a.Id,
            Titulo = a.Titulo,
            Conteudo = a.Conteudo,
            Tags = a.Tags.Select(t => new Tag { Id = t.Id, Titulo = t.Titulo }).ToList() // Ajuste para incluir Tags
        })
        .ToListAsync();


            return anotacoes;
        }

        [HttpGet("idTag")]

        public async Task<ActionResult<IEnumerable<Anotacao>>> ListarAnotacoesPorTag(Guid idTag)
        {
            var anotacoes = await _context.Anotacoes
        .Where(a => a.Tags.Any(t => t.Id == idTag)) // Ajuste para filtrar por ID da tag
        .Select(a => new Anotacao
        {
            Id = a.Id,
            Titulo = a.Titulo,
            Conteudo = a.Conteudo,
            Tags = a.Tags.Select(t => new Tag { Id = t.Id, Titulo = t.Titulo }).ToList() // Inclui Tags
        })
        .ToListAsync();

            return anotacoes;
        }

    }
}