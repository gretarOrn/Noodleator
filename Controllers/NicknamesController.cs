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
        public ActionResult Update(Nickname nicknameUpdated)
        {
            var nickname = _nickSvc.Get(nicknameUpdated.Id);

            if (nickname == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                nicknameUpdated.LastUpdated = DateTime.Now;
                nicknameUpdated.Created = nickname.Created.ToLocalTime(); /// gert til að koma í veg fyrir að mongodb fucki upp dagsetningunni
                _nickSvc.Update(nicknameUpdated);
            }

            return CreatedAtRoute("GetNickname", new { id = nicknameUpdated.Id.ToString() }, nicknameUpdated);

        }

    }
}

