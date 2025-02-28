﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using R401_TP4.Models.DataManager;
using R401_TP4.Models.EntityFramework;
using R401_TP4.Models.Repository;

namespace R401_TP4.Controllers
{
    //[Route("api/[controller]/[action]")]
    [Route("api/[controller]")]
    [ApiController]
    public class UtilisateursController : ControllerBase
    {
        private readonly IDataRepository<Utilisateur> dataRepository;
        //private readonly FilmRatingsDBContext _context;

        /// <summary>
        /// Constructeur pour le contrôleur UtilisateursController.
        /// </summary>
        /// <param name="userManager">Le contexte de la base de données utilisé pour accéder aux utilisateurs.</param>
        //public UtilisateursController(FilmRatingsDBContext context)
        //{
        //    _context = context;
        //}
        public UtilisateursController(IDataRepository<Utilisateur> dataRepo)
        {
            dataRepository = dataRepo;
        }

        /// <summary>
        /// Récupère toutes les utilisateurs.
        /// </summary>
        /// <returns>Une liste d'utilisateurs sous forme de réponse HTTP 200 OK.</returns>
        // GET: api/Utilisateurs
        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<ActionResult<IEnumerable<Utilisateur>>> GetUtilisateurs()
        //{
        //    return await _context.Utilisateurs.ToListAsync();
        //}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Utilisateur>>> GetUtilisateurs()
        {
            return await dataRepository.GetAllAsync();
        }

        /// <summary>
        /// Récupère un utilisateur spécifique en fonction de son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant de l'utilisateur à récupérer.</param>
        /// <returns>Un utilisateur sous forme de réponse HTTP 200 OK ou une erreur 404 Not Found si l'utilisateur n'existe pas.</returns>
        // GET: api/Utilisateurs/5
        //[HttpGet]
        //[ActionName("GetUtilisateurById")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<Utilisateur>> GetUtilisateurById(int id)
        //{
        //    var utilisateur = await _context.Utilisateurs.FindAsync(id);

        //    if (utilisateur == null)
        //    {
        //        return NotFound();
        //    }

        //    return utilisateur;
        //}
        [HttpGet]
        [Route("[action]/{id}")]
        [ActionName("GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Utilisateur>> GetUtilisateurById(int id)
        {
            var utilisateur = await dataRepository.GetByIdAsync(id);
            //var utilisateur = await _context.Utilisateurs.FindAsync(id);
            if (utilisateur == null)
            {
                return NotFound();
            }
            return utilisateur;
        }

        /// <summary>
        /// Récupère un utilisateur spécifique en fonction de son mail.
        /// </summary>
        /// <param name="email">Le mail de l'utilisateur à récupérer.</param>
        /// <returns>Un utilisateur sous forme de réponse HTTP 200 OK ou une erreur 404 Not Found si l'utilisateur n'existe pas.</returns>
        // GET: api/Utilisateurs/Florian.Barrier@etu.univ-smb.fr
        //[HttpGet]
        //[ActionName("GetUtilisateurByEmail")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<Utilisateur>> GetUtilisateurByEmail(string email)
        //{
        //    var utilisateur = await _context.Utilisateurs
        //        .FirstOrDefaultAsync(u => u.Mail == email);

        //    if (utilisateur == null)
        //    {
        //        return NotFound();
        //    }

        //    return utilisateur;
        //}
        [HttpGet]
        [Route("[action]/{email}")]
        [ActionName("GetByEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Utilisateur>> GetUtilisateurByEmail(string email)
        {
            var utilisateur = await dataRepository.GetByStringAsync(email);
            //var utilisateur = await _context.Utilisateurs.FirstOrDefaultAsync(e => e.Mail.ToUpper() == email.ToUpper());
            if (utilisateur == null)
            {
                return NotFound();
            }
            return utilisateur;
        }

        /// <summary>
        /// Met à jour un utilisateur spécifique en fonction de son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant de l'utilisateur à mettre à jour.</param>
        /// <param name="utilisateur">Les nouvelles données de l'utilisateur à mettre à jour.</param>
        /// <returns>Une réponse HTTP 204 No Content si la mise à jour est réussie, ou des erreurs spécifiques selon le cas (400, 404).</returns>
        // PUT: api/Utilisateurs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> PutUtilisateur(int id, Utilisateur utilisateur)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != utilisateur.UtilisateurId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(utilisateur).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UtilisateurExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutUtilisateur(int id, Utilisateur utilisateur)
        {
            if (id != utilisateur.UtilisateurId)
            {
                return BadRequest();
            }
            var userToUpdate = await dataRepository.GetByIdAsync(id);
            if (userToUpdate == null)
            {
                return NotFound();
            }
            else
            {
                await dataRepository.UpdateAsync(userToUpdate.Value, utilisateur);
                return NoContent();
            }
        }

        /// <summary>
        /// Crée un nouvel utilisateur.
        /// </summary>
        /// <param name="serie">Les données de l'utilisateur à créer.</param>
        /// <returns>L'utilisateur créé avec une réponse HTTP 201 Created ou une erreur 400 Bad Request.</returns>
        // POST: api/Utilisateurs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<ActionResult<Utilisateur>> PostUtilisateur(Utilisateur utilisateur)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    _context.Utilisateurs.Add(utilisateur);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetUtilisateurById", new { id = utilisateur.UtilisateurId }, utilisateur);
        //}
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Utilisateur>> PostUtilisateur(Utilisateur utilisateur)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await dataRepository.AddAsync(utilisateur);
            return CreatedAtAction("GetById", new { id = utilisateur.UtilisateurId }, utilisateur); // GetById : nom de l’action
        }

        /// <summary>
        /// Supprime un utilisateur spécifique en fonction de son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant de l'utilisateur à supprimer.</param>
        /// <returns>Une réponse HTTP 204 No Content si la suppression est réussie, ou une erreur 404 Not Found si l'utilisateur n'existe pas.</returns>
        // DELETE: api/Utilisateurs/5
        //[HttpDelete("{id}")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> DeleteUtilisateur(int id)
        //{
        //    var utilisateur = await _context.Utilisateurs.FindAsync(id);
        //    if (utilisateur == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Utilisateurs.Remove(utilisateur);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUtilisateur(int id)
        {
            var utilisateur = await dataRepository.GetByIdAsync(id);
            if (utilisateur == null)
            {
                return NotFound();
            }
            await dataRepository.DeleteAsync(utilisateur.Value);
            return NoContent();
        }

        /// <summary>
        /// Vérifie si un utilisateur existe dans la base de données par son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant de l'utilisateur à vérifier.</param>
        /// <returns>Vrai si l'utilisateur existe, sinon faux.</returns>
        //private bool UtilisateurExists(int id)
        //{
        //    return _context.Utilisateurs.Any(e => e.UtilisateurId == id);
        //}
    }
}
