using Microsoft.Extensions.DependencyInjection;
using UnifyTechnicalTest;

var services = new ServiceCollection();
services.AddHttpClient();
//services.AddSingleton<IPetStore, PetStore>();
services.AddSingleton<IAvailablePetClient, AvailablePetClient>();

var serviceProvider = services.BuildServiceProvider();
await serviceProvider.GetService<IAvailablePetClient>()!.ListPetsSortedByCategoryAndNameAsync(CancellationToken.None);