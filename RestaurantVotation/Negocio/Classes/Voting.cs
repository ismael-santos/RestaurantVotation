using Negocio.Properties;
using RestaurantVotation.Business.Interface;
using RestaurantVotation.Domain.Model;
using RestaurantVotation.Service.Restaurant;
using RestaurantVotation.Service.Staff;
using RestaurantVotation.Service.Voting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace RestaurantVotation.Business.Classes
{
    public class Voting : IVoting
    {
        private readonly StaffService _staffService;
        private readonly RestaurantService _restaurantService;
        private readonly VotingService _votingService;
        private static readonly List<VotingDTO> _winnersOfWeek = new List<VotingDTO>();

        public Voting()
        {
            _staffService = StaffConstrutor.InstanceOf();
            _restaurantService = RestaurantConstrutor.InstanceOf();
            _votingService = VotingConstrutor.InstanceOf();
        }

        public void Vote(VotingDTO voting)
        {
            voting.Staff = _staffService.GetByID(voting.Staff.Id);
            voting.Restaurant = _restaurantService.GetByID(voting.Restaurant.Id);

            VerifyVotingFinished(voting);
            RestartVotingLastWeek(voting);
            VerifyChosenRestaurantOfWeek(voting);
            VerifyIfEmployeeVoted(voting);

            _votingService.Save(voting);
        }

        public IEnumerable<VotingDTO> ListVoting()
        {
            return _votingService.GetAll();
        }

        public IEnumerable<RestaurantDTO> ListRestaurante()
        {
            return _restaurantService.GetAll();
        }

        public IEnumerable<StaffDTO> ListStaff()
        {
            return _staffService.GetAll();
        }

        public string GetWinnerRestaurant()
        {
            var restaurantWinner = ListRataurantMostVoted();

            if (restaurantWinner.Count() == 1)
            {
                var winner = restaurantWinner.First();

                return string.Format(Resource.MsgRestauranteEscolhido, winner.Restaurant.RestaurantName, winner.NumberOfVotes);
            }

            return string.Empty;
        }

        public IEnumerable<VotingDTO> ListWinnersOfWeek()
        {
            return _winnersOfWeek;
        }

        public IEnumerable<VotingDTO> GetPartial()
        {
            return _votingService.GetAll().GroupBy(x => x.Restaurant.Id).Select(x => new VotingDTO
            {
                Restaurant = x.First().Restaurant,
                DateVoting = x.First().DateVoting,
                NumberOfVotes = x.Count()
            }).OrderByDescending(x => x.NumberOfVotes).ToList();
        }

        public void RestartVoting()
        {
            _votingService.ClearAll();
        }

        public void FinishingVoting()
        {
            VotingDTO winner = GetRestaurantDayWinner();

            //Define Restaurante Vendedor
            if (!_winnersOfWeek.Contains(winner))
            {
                winner.IsFinished = true;
                _winnersOfWeek.Add(winner);
            }
        }

        #region private methods

        private void VerifyIfEmployeeVoted(VotingDTO votacao)
        {
            bool voted = _votingService.GetAll()
                            .Any(x => x.Staff.Id == votacao.Staff.Id && x.DateVoting.Date == votacao.DateVoting.Date);

            if (voted)
            {
                string validMsg = string.Format(Resource.MsgJaVotou, votacao.Staff.StaffName);

                throw new AlreadyVotedException(validMsg);
            }
        }

        private VotingDTO GetRestaurantDayWinner()
        {
            var listRestaurant = ListRataurantMostVoted();

            if (listRestaurant.Count() > 1) // Em caso de empate...
            {
                StringBuilder strEmpatados = new StringBuilder();

                strEmpatados.Append(string.Join(",", listRestaurant.Select(x => x.Restaurant.RestaurantName)));

                string restaurantDraw = string.Format(Resource.MsgRestauranteEmpatado, strEmpatados.ToString());

                throw new RestaurantDrawException(restaurantDraw);
            }

            return listRestaurant.First();
        }

        private IEnumerable<VotingDTO> ListRataurantMostVoted()
        {
            var partial = GetPartial();

            int countVotation = partial.First().NumberOfVotes;
            return partial.Where(x => x.NumberOfVotes == countVotation).ToList();
        }

        /// <summary>
        /// Compara se a semana da data passada por parâmtro é a mesma semana atual
        /// </summary>
        private bool CompareWeekWithCurrent(DateTime data, VotingDTO votacao)
        {
            CultureInfo cult = CultureInfo.CurrentCulture;

            int semanaVotacao = cult.Calendar.GetWeekOfYear(data.Date, CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday);
            int semanaVotacaoAtual = cult.Calendar.GetWeekOfYear(votacao.DateVoting, CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday);

            return semanaVotacao == semanaVotacaoAtual;
        }

        private void VerifyVotingFinished(VotingDTO voting)
        {
            bool finishedDate =
                _winnersOfWeek.Any(x => x.DateVoting.ToString(Resource.DataFormat) == voting.DateVoting.ToString(Resource.DataFormat));

            if (finishedDate)
            {
                throw new VotingClosedException(Resource.MsgVotacaoEncerrada);
            }
        }

        private void VerifyChosenRestaurantOfWeek(VotingDTO voting)
        {
            bool weekVote = _winnersOfWeek
                                .Any(x => x.Restaurant.Id == voting.Restaurant.Id && CompareWeekWithCurrent(x.DateVoting, voting));

            if (weekVote)
            {
                throw new RestaurantOfTheWeekException(Resource.MsgRestauranteEscolhidoSemana);
            }
        }

        public void RestartVotingLastWeek(VotingDTO voting = null)
        {
            if (voting == null)
                _winnersOfWeek.Clear();

            bool sameWeek = _winnersOfWeek.Any(x => CompareWeekWithCurrent(x.DateVoting, voting));

            if (!sameWeek)
                _winnersOfWeek.Clear();
        }

        #endregion
    }
}
