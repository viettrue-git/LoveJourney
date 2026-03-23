using System.Security.Claims;
using LoveJourney.Application.Common.Interfaces;
using LoveJourney.Application.Common.Models;
using LoveJourney.Application.DTOs.Auth;
using LoveJourney.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoveJourney.Application.Services;

public class AuthService
{
    private readonly DbContext _db;
    private readonly ITokenService _tokenService;

    public AuthService(DbContext db, ITokenService tokenService)
    {
        _db = db;
        _tokenService = tokenService;
    }

    public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request)
    {
        if (await _db.Set<Couple>().AnyAsync(c => c.Email == request.Email))
            return Result<AuthResponse>.Fail("Email đã được sử dụng.");

        var couple = new Couple
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Partner1Name = request.Partner1Name,
            Partner2Name = request.Partner2Name,
            StartDate = request.StartDate
        };

        _db.Set<Couple>().Add(couple);

        var refreshToken = CreateRefreshToken(couple.Id);
        _db.Set<RefreshToken>().Add(refreshToken);

        await _db.SaveChangesAsync();

        return Result<AuthResponse>.Ok(new AuthResponse
        {
            AccessToken = _tokenService.GenerateAccessToken(couple.Id, couple.Email),
            RefreshToken = refreshToken.Token,
            CoupleId = couple.Id,
            Email = couple.Email
        });
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request)
    {
        var couple = await _db.Set<Couple>().FirstOrDefaultAsync(c => c.Email == request.Email);
        if (couple == null || !BCrypt.Net.BCrypt.Verify(request.Password, couple.PasswordHash))
            return Result<AuthResponse>.Fail("Email hoặc mật khẩu không đúng.");

        var refreshToken = CreateRefreshToken(couple.Id);
        _db.Set<RefreshToken>().Add(refreshToken);
        await _db.SaveChangesAsync();

        return Result<AuthResponse>.Ok(new AuthResponse
        {
            AccessToken = _tokenService.GenerateAccessToken(couple.Id, couple.Email),
            RefreshToken = refreshToken.Token,
            CoupleId = couple.Id,
            Email = couple.Email
        });
    }

    public async Task<Result<AuthResponse>> RefreshAsync(string refreshTokenStr)
    {
        var storedToken = await _db.Set<RefreshToken>()
            .Include(r => r.Couple)
            .FirstOrDefaultAsync(r => r.Token == refreshTokenStr);

        if (storedToken == null || !storedToken.IsActive)
            return Result<AuthResponse>.Fail("Refresh token không hợp lệ.");

        // Revoke old token
        storedToken.RevokedAt = DateTime.UtcNow;

        // Create new refresh token
        var newRefreshToken = CreateRefreshToken(storedToken.CoupleId);
        _db.Set<RefreshToken>().Add(newRefreshToken);
        await _db.SaveChangesAsync();

        return Result<AuthResponse>.Ok(new AuthResponse
        {
            AccessToken = _tokenService.GenerateAccessToken(storedToken.CoupleId, storedToken.Couple.Email),
            RefreshToken = newRefreshToken.Token,
            CoupleId = storedToken.CoupleId,
            Email = storedToken.Couple.Email
        });
    }

    public async Task LogoutAsync(string refreshTokenStr)
    {
        var token = await _db.Set<RefreshToken>().FirstOrDefaultAsync(r => r.Token == refreshTokenStr);
        if (token != null)
        {
            token.RevokedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }
    }

    public async Task<Result<bool>> ChangePasswordAsync(Guid coupleId, ChangePasswordRequest request)
    {
        var couple = await _db.Set<Couple>().FindAsync(coupleId);
        if (couple == null)
            return Result<bool>.Fail("Không tìm thấy tài khoản.");

        if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, couple.PasswordHash))
            return Result<bool>.Fail("Mật khẩu hiện tại không đúng.");

        couple.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        await _db.SaveChangesAsync();

        return Result<bool>.Ok(true);
    }

    private RefreshToken CreateRefreshToken(Guid coupleId) => new()
    {
        CoupleId = coupleId,
        Token = _tokenService.GenerateRefreshToken(),
        ExpiresAt = DateTime.UtcNow.AddDays(7)
    };
}
