using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantVotation.Business.Classes;
using RestaurantVotation.Business.Interface;
using RestaurantVotation.Domain.Model;

namespace RestaurantVotation.UnitTest
{
    [TestClass]
    public class VotingTest
    {
        private IVoting _voting;

        [TestInitialize]
        public void TestInitialize()
        {
            _voting = new Voting();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _voting.RestartVoting();
            _voting.RestartVotingLastWeek();
        }

        [TestMethod]
        [Description("Votação sendo realizada por dois colaboradores no mesmo restaurante")]
        public void ProcessVotationWithoutErrors()
        {
            //Given
            var votacao1 = VoteConstructor(1, 1);
            var votacao2 = VoteConstructor(1, 2);

            //When
            _voting.Vote(votacao1);
            _voting.Vote(votacao2);

            _voting.FinishingVoting();

            //Then
            Assert.IsTrue(true);
        }

        [TestMethod]
        [Description("Um profissional só pode votar em um restaurante por dia")]
        [ExpectedException(typeof(AlreadyVotedException))]
        public void SameStaffTryToVoteTwiceInADayReturnException()
        {
            //Given
            var votacao1 = VoteConstructor(1, 1);
            var votacao2 = VoteConstructor(2, 1);

            //When
            _voting.Vote(votacao1);
            _voting.Vote(votacao2);

            //Then
            Assert.Fail();
        }

        [TestMethod]
        [Description("Um profissional só pode votar em um restaurante por dia, retorno Ok, votando em dias diferentes")]
        public void OnlyOneStaffToVoteInDiferentDays()
        {
            //Given
            DateTime tomorrow = DateTime.Now.AddDays(1);
            var votacao1 = VoteConstructor(1, 1);
            var votacao2 = VoteConstructor(2, 1, tomorrow);

            //When
            _voting.Vote(votacao1);
            _voting.Vote(votacao2);

            //Then
            Assert.IsTrue(true);
        }

        [TestMethod]
        [Description("Testa se há restaurantes empatados")]
        [ExpectedException(typeof(RestaurantDrawException))]
        public void RestaurantVotationDrawReturnException()
        {
            //Given
            var votacao1 = VoteConstructor(1, 1);
            var votacao2 = VoteConstructor(1, 2);
            var votacao3 = VoteConstructor(2, 3);
            var votacao4 = VoteConstructor(2, 4);

            //When
            _voting.Vote(votacao1);
            _voting.Vote(votacao2);
            _voting.Vote(votacao3);
            _voting.Vote(votacao4);

            _voting.FinishingVoting();

            //Then
            Assert.Fail();
        }

        [TestMethod]
        [Description("Testa se há tentativa de realizar uma votação finalizada")]
        [ExpectedException(typeof(VotingClosedException))]
        public void TryToVoteInClosedVotationReturnException()
        {
            // Given
            var votacao1 = VoteConstructor(1, 1);
            var votacao2 = VoteConstructor(1, 2);
            var votacao3 = VoteConstructor(2, 3);

            //When
            _voting.Vote(votacao1);
            _voting.Vote(votacao2);

            _voting.FinishingVoting();

            _voting.Vote(votacao3); //tentar votar após finalização da votação

            //Then
            Assert.Fail();
        }

        [TestMethod]
        [Description("O mesmo restaurante não pode ser escolhido mais de uma vez durante a semana.")]
        [ExpectedException(typeof(RestaurantOfTheWeekException))]
        public void SameRestauratDoNotReceiveVoteInSameWeekReturnException()
        {
            // Given
            DateTime date = new DateTime(2020, 1, 1);
            DateTime dateTomorrow = new DateTime(2020, 1, 2);

            var votacao1 = VoteConstructor(1, 1, date);
            var votacao2 = VoteConstructor(1, 2, date);
            var votacao3 = VoteConstructor(1, 1, dateTomorrow);

            //When
            _voting.Vote(votacao1);
            _voting.Vote(votacao2);

            _voting.FinishingVoting();

            _voting.RestartVoting(); // Reiniciar a votação para o próximo dia

            _voting.Vote(votacao3);

            //Then
            Assert.Fail();
        }

        [TestMethod]
        [Description("O mesmo restaurante pode ser escolhido em semanas direfentes.")]
        public void SameRestaurantCanBeChosenInDiferentWeek()
        {
            // Given
            DateTime date = new DateTime(2020, 1, 4);         //Dia 04/01/2020
            DateTime dateTomorrow = new DateTime(2020, 1, 7); //Dia 07/01/2020 (semana seguinte)

            var votacao1 = VoteConstructor(1, 1, date);
            var votacao2 = VoteConstructor(1, 2, date);
            var votacao3 = VoteConstructor(1, 1, dateTomorrow);

            //When
            _voting.Vote(votacao1);
            _voting.Vote(votacao2);

            _voting.FinishingVoting();

            _voting.RestartVoting(); // Reiniciar a votação para o próximo dia

            _voting.Vote(votacao3);

            //Then
            Assert.IsTrue(true);
        }

        [TestMethod]
        [Description("Mostrar de alguma forma o resultado da votação.")]
        public void ShowResultVotation()
        {
            // Given
            var votacao1 = VoteConstructor(1, 1);
            var votacao2 = VoteConstructor(1, 2);

            //When
            _voting.Vote(votacao1);
            _voting.Vote(votacao2);

            _voting.FinishingVoting();

            string messageWinner = _voting.GetWinnerRestaurant();

            bool hasMessage = !string.IsNullOrEmpty(messageWinner);

            //Then
            Assert.IsTrue(hasMessage);
        }

        [TestMethod]
        [Description("Não mostrar o restaurant vencedor da votação, pois a partida está empatada.")]
        public void DoNotShowRestaurantWinnerWhileVotationDraw()
        {
            // Given
            var votacao1 = VoteConstructor(1, 1);
            var votacao2 = VoteConstructor(2, 2);

            //When
            _voting.Vote(votacao1);
            _voting.Vote(votacao2);

            string messageWinner = _voting.GetWinnerRestaurant();

            bool hasMessage = string.IsNullOrEmpty(messageWinner);

            //Then
            Assert.IsTrue(hasMessage);
        }

        #region Private Methods

        private VotingDTO VoteConstructor(int idRestaurat, int idStaff, DateTime? voteDate = null)
        {
            return new VotingDTO
            {
                Restaurant = new RestaurantDTO { Id = idRestaurat },
                Staff = new StaffDTO { Id = idStaff },
                DateVoting = voteDate ?? DateTime.Now
            };
        }

        #endregion

    }
}
