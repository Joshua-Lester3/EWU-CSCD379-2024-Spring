using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;

namespace Rhym.Api;

[SignalRHub]
public class RhymHub : Hub
{
	[SignalRMethod]
	public async Task SendBar(string bar)
	{
		await Clients.All.SendAsync("ReceiveBar", bar);
	}
}
