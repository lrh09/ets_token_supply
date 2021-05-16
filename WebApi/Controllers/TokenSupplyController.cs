using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using Etherscan.Domain.Models;
using Etherscan.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Omu.ValueInjecter;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenSupplyController : ControllerBase
    {
        private readonly ILogger<TokenSupplyController> _logger;
        private readonly IRepository<Token> _tokenRepository;

        public TokenSupplyController(ILogger<TokenSupplyController> logger, IRepository<Token> tokenRepository)
        {
            _logger = logger;
            _tokenRepository = tokenRepository;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_tokenRepository.GetAll());
        }

        [HttpGet]
        [Route("Page")]
        public IActionResult Page(int pageNumber, int limit = 10)
        {
            var list = _tokenRepository.GetAll()
                .Skip((pageNumber - 1) * limit)
                .Take(limit)
                .ToList();
            
            return Ok(list);
        }
        
        [HttpGet]
        [Route("TotalPages")]
        public IActionResult TotalPages(int limit = 10)
        {
            var totalRecords = _tokenRepository.GetAll().Count();
            var totalPages = (int) Math.Ceiling((double) totalRecords / limit);
            
            return Ok(new { TotalPages = totalPages});
        }
        
        [HttpPost]
        [Route("Create")]
        public IActionResult Create(CreateModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);


            var token = new Token();
            token.InjectFrom(model);

            _tokenRepository.Add(token);
            _tokenRepository.SaveChanges();

            return Ok("Created");
        }
        
        [HttpPut]
        [Route("Edit")]
        public IActionResult Edit(int id, [FromBody] UpdateModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != model.Id) return BadRequest("Inconsistent IDs");

            var token = _tokenRepository.Get(id);
            token.InjectFrom<IgnoreNullInjection>(model);
            
            _tokenRepository.Update(token);
            _tokenRepository.SaveChanges();

            return Ok("Updated");
        }

        [HttpDelete]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            _tokenRepository.Delete(id);
            _tokenRepository.SaveChanges();

            return Ok("Deleted");
        }

        [HttpGet]
        [Route("Download")]
        public IActionResult Download()
        {
            var records = _tokenRepository.GetAll();
            
            if (!records.Any()) return BadRequest("No Available Data");
            
            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream))
            using (var csvWriter = new CsvWriter(streamWriter,
                new CsvConfiguration(CultureInfo.InvariantCulture) {HasHeaderRecord = true}))
            {
                csvWriter.WriteRecords(records);
                streamWriter.Flush();
                memoryStream.Position = 0;
                
                return File(memoryStream.ToArray(), "text/csv", "TokenSupply.csv");
            }
        }

        [HttpGet]
        [Route("Statistics")]
        public IActionResult Statistics()
        {
            var tokenSupplyService = new TokenSupplyService(_tokenRepository);
            var list = tokenSupplyService.GetTotalSupplyByName();

            return Ok(list);
        }
    }
}