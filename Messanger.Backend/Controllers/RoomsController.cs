using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

using Messanger.Application.DTOs;
using Messanger.Domain.Entities;
using Messanger.Persistence.Data;
using Messanger.Persistence.Hub;

namespace Messanger.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHubContext<ChatHub> _hubContext;

        public RoomsController(ApplicationDbContext context,
            IMapper mapper,
            IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomDTO>>> Get()
        {
            var rooms = await _context.Rooms
                .Include(r => r.Admin)
                .ToListAsync();

            var roomsViewModel = _mapper.Map<IEnumerable<Room>, IEnumerable<RoomDTO>>(rooms);

            return Ok(roomsViewModel);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> Get(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
                return NotFound();

            var roomViewModel = _mapper.Map<Room, RoomDTO>(room);
            return Ok(roomViewModel);
        }

        [HttpPost]
        public async Task<ActionResult<Room>> Create(RoomDTO viewModel)
        {
            if (_context.Rooms.Any(r => r.Name == viewModel.Name))
                return BadRequest("Invalid room name or room already exists");

            var user = _context.AppUsers.FirstOrDefault(u => u.Username == User.Identity.Name);
            var room = new Room()
            {
                Name = viewModel.Name,
                Admin = user
            };

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            var createdRoom = _mapper.Map<Room, RoomDTO>(room);
            await _hubContext.Clients.All.SendAsync("addChatRoom", createdRoom);

            return CreatedAtAction(nameof(Get), new { id = room.Id }, createdRoom);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, RoomDTO viewModel)
        {
            if (_context.Rooms.Any(r => r.Name == viewModel.Name))
                return BadRequest("Invalid room name or room already exists");

            var room = await _context.Rooms
                .Include(r => r.Admin)
                .Where(r => r.Id == id && r.Admin.Username == User.Identity.Name)
                .FirstOrDefaultAsync();

            if (room == null)
                return NotFound();

            room.Name = viewModel.Name;
            await _context.SaveChangesAsync();

            var updatedRoom = _mapper.Map<Room, RoomDTO>(room);
            await _hubContext.Clients.All.SendAsync("updateChatRoom", updatedRoom);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var room = await _context.Rooms
                .Include(r => r.Admin)
                .Where(r => r.Id == id && r.Admin.Username == User.Identity.Name)
                .FirstOrDefaultAsync();

            if (room == null)
                return NotFound();

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("removeChatRoom", room.Id);
            await _hubContext.Clients.Group(room.Name).SendAsync("onRoomDeleted");

            return Ok();
        }
    }
}
