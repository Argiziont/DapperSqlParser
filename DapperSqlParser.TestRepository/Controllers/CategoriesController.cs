using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperSqlParser.TestRepository.Models;
using DapperSqlParser.TestRepository.Service.Repositories.Interfaces;

namespace DapperSqlParser.TestRepository.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        [Route("GetAll")]
        [ProducesResponseType(typeof(IEnumerable<Category>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var response = await _categoryRepository.GetAllAsync();

                return Ok(response);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        [HttpGet]
        [Route("GetById")]
        [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetByIdAsync(int categoryId)
        {
            try
            {
                var response = await _categoryRepository.GetByIdAsync(categoryId);

                return Ok(response);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        [HttpDelete]
        [Route("DeleteById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteByIdAsync(int categoryId)
        {
            try
            {
                await _categoryRepository.DeleteByIdAsync(categoryId);

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        [HttpPost]
        [Route("Insert")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> InsertAsync(Category category)
        {
            try
            {
                await _categoryRepository.InsertAsync(category);

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        [HttpPut]
        [Route("UpdateTitleById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateTitleByIdAsync(int categoryId, string categoryName)
        {
            try
            {
                await _categoryRepository.UpdateNameByIdAsync(categoryId, categoryName);

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }
    }
}
