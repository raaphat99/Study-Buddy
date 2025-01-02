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
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Authorize]
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

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index(string searchQuery)
        {
            ViewBag.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.Topics = await _topicService.GetAllTopicsAsync();
            ViewBag.SearchQuery = searchQuery;

            IEnumerable<RoomDto> rooms = await _roomService.FilterRoomsAsync(searchQuery);
            return View(rooms);
        }

        [AllowAnonymous]
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
            TempData["SuccessMessage"] = "Room created successfully!";
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

            // Retrieves the unique identifier of the currently authenticated user from their claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                await _roomService.UpdateAsync(id, roomDto, userId);
            }
            catch (UnauthorizedAccessException)
            {
                TempData["UnauthorizedAccess"] = "Access Forbidden! You don't have permission to edit this room.";
                return RedirectToAction("AccessDenied", "Account");
            }

            await _unitOfWork.CompleteAsync();
            TempData["SuccessMessage"] = "Room updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                await _roomService.DeleteAsync(id, userId);
            }
            catch (UnauthorizedAccessException)
            {
                TempData["UnauthorizedAccess"] = "Access Forbidden! You don't have permission to delete this room.";
                return RedirectToAction("AccessDenied", "Account");
            }

            await _unitOfWork.CompleteAsync();
            TempData["SuccessMessage"] = "Room deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
