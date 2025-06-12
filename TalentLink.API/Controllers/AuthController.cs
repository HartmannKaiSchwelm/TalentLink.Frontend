// ...existing usings...
using TalentLink.Application.DTOs;
// ...existing code...

[HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginDto dto)
{
    var user = await _userService.AuthenticateAsync(dto.Email, dto.Password);
    if (user == null)
        return Unauthorized("Invalid credentials");

    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        }),
        Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpiresInMinutes"]!)),
        Issuer = _configuration["Jwt:Issuer"],
        Audience = _configuration["Jwt:Audience"],
        SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature)
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    var tokenString = tokenHandler.WriteToken(token);

    // Hole VerifiedByParentId, falls Student
    Guid? verifiedByParentId = null;
    if (user is Student student)
    {
        // Falls in DB Guid.Empty steht, setze auf null!
        verifiedByParentId = student.VerifiedByParentId;
        if (verifiedByParentId == Guid.Empty)
            verifiedByParentId = null;
    }

    // WICHTIG: Role als string serialisieren!
    return Ok(new AuthResponseDto
    {
        Token = tokenString,
        Id = user.Id,
        Name = user.Name,
        Email = user.Email,
        Role = user.Role.ToString(), // <-- string!
        VerifiedByParentId = verifiedByParentId
    });
}
// ...restlicher Code...