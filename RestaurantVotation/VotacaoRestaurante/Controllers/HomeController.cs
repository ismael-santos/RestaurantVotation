using RestaurantVotation.Business.Interface;
using RestaurantVotation.Domain.Model;
using System.Linq;
using System.Web.Mvc;

namespace VotacaoRestaurante.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVoting _votacao;

        public HomeController(IVoting votacao)
        {
            _votacao = votacao;
        }

        private void LoadDropDown()
        {
            ViewBag.Colaborador = _votacao
                                    .ListStaff()
                                    .Select(x => new SelectListItem { Text = x.StaffName, Value = x.Id.ToString() });

            ViewBag.Restaurante = _votacao
                                    .ListRestaurante()
                                    .Select(x => new SelectListItem { Text = x.RestaurantName, Value = x.Id.ToString() });
        }

        public ActionResult Index()
        {
            LoadDropDown();
            return View();
        }

        [HttpPost]
        public ActionResult Votar(VotingDTO votacao)
        {
            try
            {
                _votacao.Vote(votacao);
            }
            catch (System.Exception e)
            {
                TempData["Error"] = e.Message;
            }

            TempData["ListaVotacao"] = _votacao.ListVoting();
            TempData["Parcial"] = _votacao.GetPartial();
            TempData["VencedorSemana"] = _votacao.ListWinnersOfWeek();
            TempData["Date"] = votacao.DateVoting.ToString("dd/MM/yyyy");

            return Redirect("/");
        }

        public ActionResult Reiniciar()
        {
            _votacao.RestartVoting();

            TempData["VencedorSemana"] = _votacao.ListWinnersOfWeek();
            TempData["Date"] = null;

            return Redirect("/");
        }

        public ActionResult Finalizar()
        {
            if (_votacao.ListVoting().Any())
            {
                try
                {
                    _votacao.FinishingVoting();
                }
                catch (System.Exception e)
                {
                    TempData["Error"] = e.Message;
                }

                TempData["Vencedor"] = _votacao.GetWinnerRestaurant();
                TempData["ListaVotacao"] = _votacao.ListVoting();
                TempData["Parcial"] = _votacao.GetPartial();

                _votacao.RestartVoting();
            }

            TempData["VencedorSemana"] = _votacao.ListWinnersOfWeek();
            return Redirect("/");
        }
    }
}