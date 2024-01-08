﻿using CheckedAppProject.DATA;
using Microsoft.AspNetCore.Mvc;
using CheckedAppProject.DATA.Entities;
using CheckedAppProject.DATA.CheckedAppDbContext;
using CheckedAppProject.LOGIC.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using CheckedAppProject.LOGIC.Services;

namespace CheckedAppProject.API.Controllers
{
    [Route("api/itemlist")]
    public class ItemListController : ControllerBase
    {
        private readonly IItemListService _itemListService;
        private readonly ILogger<ItemListController> _logger;

        public ItemListController(IItemListService itemListService, ILogger<ItemListController> logger)
        {
            _itemListService = itemListService;
            _logger = logger;
        }

        [HttpGet("getalllists")]
        public async Task<ActionResult<IEnumerable<ItemListDTO>>> GetAllAsync()
        {
            var itemListsDto = await _itemListService.GetAllAsync();

            if (itemListsDto is null)
            {
                _logger.LogInformation("No Item List found");
                return NotFound();
            }

            return Ok(itemListsDto);
        }

        [HttpGet("user/{userid}")]
        public async Task<ActionResult<IEnumerable<ItemListDTO>>> GetAllByUserIdAsync([FromRoute] int userid)
        {
            var itemListsDto = await _itemListService.GetAllByUserIdAsync(userid);

            if (itemListsDto is null)
            {
                _logger.LogInformation("No Item List found");
                return NotFound();
            }

            return Ok(itemListsDto);
        }

        [HttpGet("getlist/{itemlistid}")]
        public async Task<ActionResult<ItemListDTO>> GetList([FromRoute] int itemlistid)
        {
            var itemList = await _itemListService.GetByIdAsync(itemlistid);

            if (itemList is null)
            {
                _logger.LogInformation($"Item List with id {itemList} not found");
                return NotFound();
            }

            return Ok(itemList);
        }

        [HttpGet("city/{city}")]
        public async Task<ActionResult<ItemListDTO>> GetByCityAsync([FromRoute] string city)
        {
            var itemList = await _itemListService.GetByCityAsync(city);

            if (itemList is null)
            {
                _logger.LogInformation("Item List not found");
                return NotFound();
            }

            return Ok(itemList);
        }

        //[HttpGet("cityanddate/{city}/{date}")]
        //public async Task<AcceptedResult<ItemListDTO>> GetByDateAndCity([FromRoute] string city, [FromRoute] DateTime date)
        //{
        //    var itemList = await _itemListService.GetByCityAndDateAsync(city, date);

        //    if (itemList is null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(itemList);
        //}

        [HttpPost("addlist/{userid}")]
        public async Task<ActionResult> AddList([FromBody] CreateItemListDTO dto, [FromRoute] int userid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _itemListService.CreateAsync(dto, userid);

            _logger.LogInformation($"ItemList {dto.ItemListName} is added");
            return Ok(new { Message = "Item List added successfully" });
        }

        [HttpPost("user/{userid}")]
        public async Task<ActionResult> CopyItemListAsync([FromRoute] int itemListid, [FromRoute] int userid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var copy = await _itemListService.CopyAsync(itemListid, userid);

            _logger.LogInformation($"Item List with id {itemListid} is copied");
            return Ok(copy);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemListAsync([FromBody] UpdateItemListDTO dto, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isUpdated = await _itemListService.UpdateAsync(dto, id);

            if (isUpdated)
            {
                _logger.LogInformation($"Item List {dto.ItemListName} is created");
                return NoContent();
            }

            _logger.LogInformation($"Item List with id {id} is not found");
            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var isDeleted = await _itemListService.DeleteAsync(id);

            if (isDeleted)
            {
                _logger.LogInformation($"Item List with id {id} is deleted");
                return NoContent();
            }

            _logger.LogInformation($"Item List with id {id} is deleted");
            return NotFound();
        }
    }
}
