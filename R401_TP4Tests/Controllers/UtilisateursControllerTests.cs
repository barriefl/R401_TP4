using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using R401_TP4.Controllers;
using R401_TP4.Models.DataManager;
using R401_TP4.Models.EntityFramework;
using R401_TP4.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace R401_TP4.Controllers.Tests
{
    [TestClass()]
    public class UtilisateursControllerTests
    {
        private FilmRatingsDBContext context;
        private UtilisateursController controller;
        private IDataRepository<Utilisateur> dataRepository;

        [TestInitialize] 
        public void InitialisationDesTests() 
        {
            var builder = new DbContextOptionsBuilder<FilmRatingsDBContext>().UseNpgsql();
            context = new FilmRatingsDBContext();
            //controller = new UtilisateursController(context);
            dataRepository = new UtilisateurManager(context);
            controller = new UtilisateursController(dataRepository);
        }

        [TestMethod()]
        public void UtilisateursControllerTest()
        {
            // Arrange.
            // Act.
            // Assert.
            Assert.IsNotNull(context, "Le context est null.");
            Assert.IsNotNull(controller, "Le controller est null.");
        }

        [TestMethod()]
        public void GetUtilisateursTest()
        {
            // Arrange.
            // Act.
            var utilisateursAttendus = context.Utilisateurs.ToList();
            var result = controller.GetUtilisateurs().Result;
            var utilisateursRecuperes = result.Value.ToList();
            // Assert.
            Assert.IsNotNull(result, "Le résultat est null.");
            Assert.IsInstanceOfType(result, typeof(ActionResult<IEnumerable<Utilisateur>>), "Pas un ActionResult.");
            CollectionAssert.AreEqual(utilisateursAttendus, utilisateursRecuperes, "Les utilisateurs récupérés ne sont pas les mêmes.");
        }

        [TestMethod()]
        public void GetUtilisateurByIdTest_OK()
        {
            // Arrange.
            // Act.
            var utilisateurAttendu = context.Utilisateurs.Where(u => u.UtilisateurId == 1).FirstOrDefault();
            var result = controller.GetUtilisateurById(1).Result;
            var utilisateurRecupere = result.Value;
            // Assert.
            Assert.IsInstanceOfType(result, typeof(ActionResult<Utilisateur>), "Pas un ActionResult.");
            Assert.IsNotNull(result, "Le résultat est null.");
            Assert.IsInstanceOfType(utilisateurRecupere, typeof(Utilisateur), "Pas un Utilisateur.");
            Assert.AreEqual(utilisateurAttendu, utilisateurRecupere, "L'utilisateur récupéré n'est pas le même.");
        }

        [TestMethod()]
        public void GetUtilisateurByIdTest_NonOK()
        {
            // Arrange.
            // Act.
            var result = controller.GetUtilisateurById(0).Result;
            var errorResult = result.Result as NotFoundResult;
            // Assert.
            Assert.IsInstanceOfType(result, typeof(ActionResult<Utilisateur>), "Pas un ActionResult.");
            Assert.IsNotNull(result.Result, "Il n'y a pas d'erreur.");
            Assert.AreEqual(errorResult.StatusCode, StatusCodes.Status404NotFound, "Pas une erreur 404.");
            Assert.IsNull(result.Value, "Utilisateur créé alors qu'il y a une erreur.");
        }

        [TestMethod()]
        public void GetUtilisateurByEmailTest_OK()
        {
            // Arrange.
            // Act.
            var utilisateurAttendu = context.Utilisateurs.Where(u => u.Mail == "rrichings1@naver.com").FirstOrDefault();
            var result = controller.GetUtilisateurByEmail("rrichings1@naver.com").Result;
            var utilisateurRecupere = result.Value;
            // Assert.
            Assert.IsInstanceOfType(result, typeof(ActionResult<Utilisateur>), "Pas un ActionResult.");
            Assert.IsNull(result.Result, "Le résultat est null.");
            Assert.IsInstanceOfType(utilisateurRecupere, typeof(Utilisateur), "Pas un Utilisateur.");
            Assert.AreEqual(utilisateurAttendu, utilisateurRecupere, "L'utilisateur récupéré n'est pas le même.");
        }

        [TestMethod()]
        public void GetUtilisateurByEmailTest_NonOK()
        {
            // Arrange.
            // Act.
            var result = controller.GetUtilisateurByEmail("yannislapetitecoquine@gmail.com").Result;
            var errorResult = result.Result as NotFoundResult;
            // Assert.
            Assert.IsInstanceOfType(result, typeof(ActionResult<Utilisateur>), "Pas un ActionResult.");
            Assert.IsNotNull(result.Result, "Il n'y a pas d'erreur."); // Null = pas d'erreur.
            Assert.AreEqual(errorResult.StatusCode, StatusCodes.Status404NotFound, "Pas une erreur 404.");
            Assert.IsNull(result.Value, "Utilisateur créé alors qu'il y a une erreur.");
        }

        [TestMethod()]
        public void PostUtilisateurTest_OK()
        {
            // Arrange.
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            var userController = new UtilisateursController(mockRepository.Object);
            Utilisateur user = new Utilisateur
            {
                Nom = "POISSON",
                Prenom = "Pascal",
                Mobile = "1",
                Mail = "poisson@gmail.com",
                Pwd = "Toto12345678!",
                Rue = "Chemin de Bellevue",
                CodePostal = "74940",
                Ville = "Annecy-le-Vieux",
                Pays = "France",
                Latitude = null,
                Longitude = null
            };
            // Act.
            var actionResult = userController.PostUtilisateur(user).Result;
            // Assert.
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<Utilisateur>), "Pas un ActionResult<Utilisateur>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsInstanceOfType(result.Value, typeof(Utilisateur), "Pas un Utilisateur");
            user.UtilisateurId = ((Utilisateur)result.Value).UtilisateurId;
            Assert.AreEqual(user, (Utilisateur)result.Value, "Utilisateurs pas identiques");
        }

        [TestMethod()]
        [ExpectedException(typeof(AggregateException))]
        public void PostUtilisateurTest_ReturnsAggregateException()
        {
            // Arrange.
            Utilisateur userAtester = new Utilisateur()
            {
                Nom = "SKI",
                Prenom = "Bidi",
                Pwd = "skibidi",
                Rue = "Chemin de Bellevue",
                Ville = "Annecy-le-Vieux",
                Pays = "France",
                Latitude = null,
                Longitude = null
            };
            // Act.
            var result = controller.PostUtilisateur(userAtester).Result; // .Result pour appeler la méthode async de manière synchrone, afin d'attendre l’ajout  
            // Assert.
        }

        [TestMethod()]
        public void PostUtilisateurTest_InvalidModel()
        {
            // Arrange.
            Utilisateur utilisateur = new Utilisateur()
            {
                Nom = "SKI",
                Prenom = "Bidi",
                Mobile = "1",
                Mail = "skibidi@gmail.com",
                Pwd = "Toto1234!",
                Rue = "Chemin de Bellevue",
                CodePostal = "74940",
                Ville = "Annecy-le-Vieux",
                Pays = "France",
                Latitude = null,
                Longitude = null 
            };
            string PhoneRegex = @"^0[0-9]{9}$";
            Regex regex = new Regex(PhoneRegex);
            // Act.
            // Assert.
            if (!regex.IsMatch(utilisateur.Mobile))
            {
                controller.ModelState.AddModelError("Mobile", "Le n° de mobile doit contenir 10 chiffres."); // On met le même message que dans la classe Utilisateur.
            }
        }

        [TestMethod()]
        public void PutUtilisateurTest_OK()
        {
            // Arrange.
            Random rnd = new Random();
            int chiffre = rnd.Next(1, 1000000000);
            // Le mail doit être unique donc 2 possibilités :
            // 1. On s'arrange pour que le mail soit unique en concaténant un random ou un timestamp
            // 2. On supprime le user après l'avoir créé. Dans ce cas, nous avons besoin d'appeler la méthode DELETE de l’API ou remove du DbSet.
            Utilisateur userAtester = new Utilisateur()
            {
                UtilisateurId = 1,
                Nom = "Calida",
                Prenom = "Lilley",
                Mobile = "0653930778",
                Mail = "calida" + chiffre + "@gmail.com",
                Pwd = "Toto12345678!",
                Rue = "Impasse des bergeronnettes",
                CodePostal = "74200",
                Ville = "Allinges",
                Pays = "France",
                Latitude = (float?)46.344795,
                Longitude = (float?)6.4885845
            };
            // Act.
            var result = controller.PutUtilisateur(1, userAtester).Result; // .Result pour appeler la méthode async de manière synchrone, afin d'attendre l’ajout  
            Utilisateur? userRecupere = context.Utilisateurs.Where(u => u.UtilisateurId == 1).FirstOrDefault();
            // Assert.
            Assert.AreEqual(userRecupere, userAtester, "Utilisateurs pas identiques");
        }

        [TestMethod()]
        [ExpectedException(typeof(AggregateException))]
        public void PutUtilisateurTest_ReturnsAggregateException()
        {
            // Arrange.
            Utilisateur userAtester = new Utilisateur()
            {
                UtilisateurId = 1,
                Nom = "Calida",
                Prenom = "Lilley",
                Mobile = "0653930778",
                Pwd = "Toto12345678!",
                Rue = "Impasse des bergeronnettes",
                CodePostal = "74200",
                Ville = "Allinges",
                Pays = "France",
                Latitude = (float?)46.344795,
                Longitude = (float?)6.4885845
            };
            // Act.
            var result = controller.PutUtilisateur(1, userAtester).Result; // .Result pour appeler la méthode async de manière synchrone, afin d'attendre l’ajout  
            // Assert.
        }

        [TestMethod()]
        public void PutUtilisateurTest_InvalidModel()
        {
            // Arrange.
            Utilisateur utilisateur = new Utilisateur()
            {
                UtilisateurId = 1,
                Nom = "Calida",
                Prenom = "Lilley",
                Mobile = "0653930778",
                Mail = "calida@gmail.com",
                Pwd = "Toto12345678!",
                Rue = "Impasse des bergeronnettes",
                CodePostal = "666",
                Ville = "Allinges",
                Pays = "France",
                Latitude = (float?)46.344795,
                Longitude = (float?)6.4885845
            };
            string CodePostalRegex = @"^[0-9]{5}$";
            Regex regex = new Regex(CodePostalRegex);
            // Act.
            // Assert.
            if (!regex.IsMatch(utilisateur.CodePostal))
            {
                controller.ModelState.AddModelError("CodePostal", "Le code postal doit contenir 5 chiffres."); // On met le même message que dans la classe Utilisateur.
            }
        }

        [TestMethod()]
        public void DeleteUtilisateurTest_OK()
        {
            // Arrange.
            var utilisateur = new Utilisateur
            {
                Nom = "Test",
                Prenom = "User",
                Mobile = "0123456789",
                Mail = "testuser@example.com",
                Pwd = "Password123!",
                Rue = "123 Test Street",
                CodePostal = "12345",
                Ville = "Testville",
                Pays = "France",
                Latitude = null,
                Longitude = null
            };
            context.Utilisateurs.Add(utilisateur);
            context.SaveChanges();
            int userId = utilisateur.UtilisateurId;
            // Act.
            var result = controller.DeleteUtilisateur(userId).Result;
            // Assert.
            Assert.IsInstanceOfType(result, typeof(NoContentResult), "Pas un NoContentResult.");

            var deletedUser = context.Utilisateurs.FindAsync(userId).Result;
            Assert.IsNull(deletedUser);
        }
    }
}