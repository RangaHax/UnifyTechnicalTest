using Moq;
using RichardSzalay.MockHttp; //https://stackoverflow.com/a/36427274
using UnifyTechnicalTest;

namespace NUnitTests
{
    public class AvailablePetClientTests
    {
        private readonly HttpClient _testClient;
        public AvailablePetClientTests()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://petstore.swagger.io/v2/pet/findByStatus?status=available")
                    .Respond("application/json", File.ReadAllText("TestData.json"));
            _testClient = new HttpClient(mockHttp);
        }

        [Test]
        public async Task LoadPetsAsync()
        {
            //arrange
            var sut = new AvailablePetClient(_testClient); //system under test

            //act
            var pets = await sut.LoadPetsAsync(CancellationToken.None);

            //assert
            Assert.That(pets.Count, Is.EqualTo(5));
            Assert.That(pets.First().Category?.Name!, Is.EqualTo("Yankee"));
        }

        [Test]
        public void SortByName()
        {
            //arrange
            var sut = new AvailablePetClient(_testClient); //system under test
            var pets = new List<Pet> { new Pet {Name = "AAA"}, new Pet{Name = "BBB"} };

            //act
            var sorted = sut.SortPets(pets);

            //assert
            Assert.That(sorted.First().Name, Is.SameAs("BBB"));
        }

        [Test]
        public void SortByCategory()
        {
            //arrange
            var sut = new AvailablePetClient(_testClient); //system under test
            var pets = new List<Pet> { new Pet { Name = "AAA", Category = new Category { Name = "XXX" } },
                                       new Pet { Name = "BBB", Category = new Category { Name = "YYY" } } };

            //act
            var sorted = sut.SortPets(pets);

            //assert
            Assert.That(sorted.First().Name, Is.SameAs("AAA"));
        }

        [Test]
        public void OutputFormat()
        {
            //arrange
            var sut = new AvailablePetClient(_testClient); //system under test
            var pets = new List<Pet> { new Pet { Name = "First" }, new Pet { Name = "Second" } };
            using StringWriter sw = new StringWriter();
            Console.SetOut(sw);

            //act
            sut.ListPets(pets);

            //assert
            string expected = "No Category" + Environment.NewLine + "----------" + Environment.NewLine + "First" + Environment.NewLine + "Second" + Environment.NewLine;
            //Assert.That(, Is.SameAs(expected));
            Assert.That(expected, Is.EqualTo(sw.ToString()));
        }


    }
}