using Application.DTOs.MessageDTOs;
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
    public class RoomController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoomService _roomService;
        private readonly ITopicService _topicService;
        private readonly IMessageService _messageService;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoomController(IUnitOfWork unitOfWork, IRoomService roomService, IMessageService messageService,
            UserManager<ApplicationUser> userManager, ITopicService topicService)
        {
            _unitOfWork = unitOfWork;
            _roomService = roomService;
            _messageService = messageService;
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
            ViewBag.RoomCount = rooms.Count();

            return View(rooms);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            ViewBag.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ChatRoomDto chatRoomDto = await _roomService.GetChatRoomDetailsAsync(id);

            return View(new ChatRoomViewModel() { ChatRoomDto = chatRoomDto });
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Insert(RoomCreateDto roomDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                await _roomService.CreateAsync(roomDto);
                await _unitOfWork.CompleteAsync();
                TempData["SuccessMessage"] = "Room created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ViewData["ErrorMessage"] = "Unexpected error has occured while creating a new room!";
                return View();
            }

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, RoomCreateDto roomDto)
        {
            if (!ModelState.IsValid)
                return View("Edit", roomDto);

            // Retrieves the unique identifier of the currently authenticated user from their claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                await _roomService.UpdateAsync(id, roomDto, userId);
                await _unitOfWork.CompleteAsync();
                TempData["SuccessMessage"] = "Room updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (UnauthorizedAccessException)
            {
                TempData["UnauthorizedAccess"] = "Access Forbidden! You don't have permission to edit this room.";
                return RedirectToAction("AccessDenied", "Account");
            }
            catch (Exception)
            {
                ViewData["ErrorMessage"] = "Unexpected error has occured while updating the room!";
                return View();
            }
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Destroy(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                await _roomService.DeleteAsync(id, userId);
                await _unitOfWork.CompleteAsync();
                TempData["SuccessMessage"] = "Room deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (UnauthorizedAccessException)
            {
                TempData["UnauthorizedAccess"] = "Access Forbidden! You don't have permission to delete this room.";
                return RedirectToAction("AccessDenied", "Account");
            }
            catch (Exception)
            {
                ViewData["ErrorMessage"] = "Unexpected error has occured while deleting the room!";
                return View();
            }
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(ChatRoomViewModel chatRoomVM)
        {
            MessageCreateDto messageDto = chatRoomVM.MessageCreateDto;

            if (!ModelState.IsValid)
                return BadRequest("Invalid message!");

            messageDto.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                await _messageService.CreateAsync(messageDto);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction("Details", new { id = messageDto.RoomId });
            }
            catch (Exception)
            {
                ViewData["ErrorMessage"] = "Unexpected error has occured while sending a message. Please try again.";
                return View();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteMessage(ChatRoomViewModel chatRoomVM)
        {
            MessageDeleteDto messageDto = chatRoomVM.MessageDeleteDto;

            if (!ModelState.IsValid)
                return BadRequest("Invalid message!");

            messageDto.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                await _messageService.DeleteAsync(messageDto.MessageId, messageDto.UserId);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction("Details", new { id = messageDto.RoomId });
            }
            catch (UnauthorizedAccessException)
            {
                TempData["UnauthorizedAccess"] = "Access Forbidden! You don't have permission to delete this message.";
                return RedirectToAction("AccessDenied", "Account");
            }
            catch (Exception)
            {
                ViewData["ErrorMessage"] = "Unexpected error has occured while deleting the message. Please try again.";
                return View();
            }
        }
    }
}
