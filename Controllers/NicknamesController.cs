using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Noodleator.Models;
using Noodleator.Services;

namespace Noodleator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NicknamesController : ControllerBase
    {
        private readonly NicknameService _nickSvc;

        public NicknamesController(NicknameService nicknameService)
        {
            _nickSvc = nicknameService;
        }

        [HttpGet("random/{name}")]
        public ActionResult<Nickname> Random(string name)
        {
            return _nickSvc.GetRandomByName(name);
        }

        [HttpGet]
        public ActionResult<List<Nickname>> Get() => _nickSvc.Get();

        [HttpGet("{id:length(24)}", Name = "GetNickname")]
        public ActionResult<Nickname> Get(string id)
        {
            var nickname = _nickSvc.Get(id);

            if (nickname == null)
            {
                return NotFound();
            }
            return nickname;
        }

        [HttpPost]
        public ActionResult<Nickname> Create(Nickname nickname)
        {
            nickname.Created = nickname.LastUpdated = DateTime.Now;
            if (ModelState.IsValid)
            {
                _nickSvc.Create(nickname);
            }

            return CreatedAtRoute("GetNickname", new { id = nickname.Id.ToString() }, nickname);
        }

        [HttpPut("{id:length(24)}")]
        public ActionResult Update(Nickname nicknameIn)
        {
            var nickname = _nickSvc.Get(nicknameIn.Id);

            if (nickname == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                nicknameIn.LastUpdated = DateTime.Now;
                nicknameIn.Created = nickname.Created.ToLocalTime(); /// gert til að koma í veg fyrir að mongodb fucki upp dagsetningunni
                _nickSvc.Update(nicknameIn);
            }

            return CreatedAtRoute("GetNickname", new { id = nicknameIn.Id.ToString() }, nicknameIn);

        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var nickname = _nickSvc.Get(id);

            if (nickname == null)
            {
                return NotFound();
            }

            _nickSvc.Delete(id);

            return NoContent();
        }

    }
}

