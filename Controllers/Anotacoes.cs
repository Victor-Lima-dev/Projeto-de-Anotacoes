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
            _context.Anotacoes.Add(anotacao);

             //se entrar uma tag que já existe, ele não cria uma nova tag, ele só adiciona a tag que já existe

            await _context.SaveChangesAsync();

           // return anotacao;

            return CreatedAtAction("GetAnotacao", new { id = anotacao.Id }, anotacao);
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


    }
}