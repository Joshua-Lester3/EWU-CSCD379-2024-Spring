﻿using Wordle.Api.Data;
using Wordle.Api.Requests;

namespace Wordle.Api.Services;

public class LeaderboardService
{
	private readonly AppDbContext _context;
	public LeaderboardService(AppDbContext context)
	{
		_context = context;
	}
	public List<Player> GetTopScores()
	{
		return _context.Players.
			OrderBy(player => player.AverageAttempts).ToList();
	}

	public async Task<Player> PostScore(PlayerRequest request)
	{
		Player? foundPlayer = _context.Players.FirstOrDefault(p => request.Name.Equals(p.Name));
		if (foundPlayer is not null)
		{
			foundPlayer.AverageAttempts = request.AverageAttempts;
			foundPlayer.GameCount = request.GameCount;
			await _context.SaveChangesAsync();
			return foundPlayer;
		}
		else
		{
			Player addedPlayer = new()
			{
				AverageAttempts = request.AverageAttempts,
				GameCount = request.GameCount,
				Name = request.Name,
			};
			await _context.Players.AddAsync(addedPlayer);
			await _context.SaveChangesAsync();
			return addedPlayer;
		}
	}
}