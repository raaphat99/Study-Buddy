using Application.DTOs.RoomDTOs;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Application.Interfaces.UOW;
using Application.Services;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.ViewModels;

namespace Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoomService _roomService;
        private readonly ITopicService _topicService;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(IUnitOfWork unitOfWork, IRoomService roomService,
            UserManager<ApplicationUser> userManager, ITopicService topicService)
        {
            _unitOfWork = unitOfWork;
            _roomService = roomService;
            _topicService = topicService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchQuery)
        {
            ViewBag.SearchQuery = searchQuery;
            ViewBag.Topics = await _topicService.GetAllTopicsAsync();
            IEnumerable<RoomDto> rooms = await _roomService.FilterRoomsAsync(searchQuery);
            return View(rooms);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var room = await _roomService.GetByIdAsync(id);
            return View(room);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.ViewAction = "Create";
            ViewBag.SubmitAction = "Insert";
            ViewBag.Users = await _userManager.Users.ToListAsync();
            ViewBag.Topics = await _topicService.GetAllTopicsAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Insert(RoomCreateDto roomDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _roomService.CreateAsync(roomDto);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.ViewAction = "Edit";
            ViewBag.SubmitAction = "Update";
            ViewBag.Users = await _userManager.Users.ToListAsync();
            ViewBag.Topics = await _topicService.GetAllTopicsAsync();

            var room = await _roomService.GetByIdAsync(id);
            RoomCreateDto roomDto = new RoomCreateDto
            {
                Id = room.Id,
                HostId = room.HostId,
                Name = room.Name,
                Description = room.Description,
                TopicId = room.TopicId
            };
            return View(roomDto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, RoomCreateDto roomDto)
        {
            if (!ModelState.IsValid)
                return View("Edit", roomDto);

            await _roomService.UpdateAsync(id, roomDto);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var room = await _roomService.GetByIdAsync(id);

            if (room == null)
            {
                return NotFound();
            }

            DeleteConfirmationViewModel viewModel = new DeleteConfirmationViewModel()
            {
                Id = id,
                Controller = "Home",
                Action = "Destroy",
                EntityName = "Room"
            };

            string RefererUrl = Request.Headers.Referer.ToString();
            ViewBag.RefererUrl = string.IsNullOrEmpty(RefererUrl) ? Url.Action("Index") : RefererUrl;

            return PartialView("_DeleteConfirmation", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Destroy(int id)
        {
            await _roomService.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
