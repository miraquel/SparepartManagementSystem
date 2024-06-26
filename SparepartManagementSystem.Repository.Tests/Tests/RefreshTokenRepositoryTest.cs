using System.Data.SqlTypes;
using System.Globalization;
using JetBrains.Annotations;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.Interface;
using SparepartManagementSystem.Repository.UnitOfWork;

namespace SparepartManagementSystem.Repository.Tests.Tests;

[TestSubject(typeof(IRefreshTokenRepository))]
public class RefreshTokenRepositoryTest : IAsyncLifetime
{
    private readonly ServiceCollectionHelper _serviceCollectionHelper = new();
    private readonly IUnitOfWork _unitOfWork;
    private readonly User _user = RepositoryTestsHelper.CreateUser();

    public RefreshTokenRepositoryTest()
    {
        _unitOfWork = _serviceCollectionHelper.GetRequiredService<IUnitOfWork>();
    }
    
    public async Task InitializeAsync()
    {
        await _unitOfWork.UserRepository.Add(_user);
        _user.UserId = await _unitOfWork.GetLastInsertedId();
    }

    public async Task DisposeAsync()
    {
        await _unitOfWork.Rollback();
    }

    [Fact]
    public async Task Add_RefreshToken_Success()
    {
        // Arrange
        var refreshToken = RepositoryTestsHelper.CreateRefreshToken();
        refreshToken.UserId = _user.UserId;

        // Act
        await _unitOfWork.RefreshTokenRepository.Add(refreshToken);

        // Assert
        var result = await _unitOfWork.RefreshTokenRepository.GetById(await _unitOfWork.GetLastInsertedId());
        Assert.True(result.RefreshTokenId > 0);
        Assert.Equal(refreshToken.Token, result.Token);
        Assert.Equal(refreshToken.Expires, result.Expires);
        Assert.Equal(refreshToken.Created, result.Created);
        Assert.Equal(refreshToken.Revoked, result.Revoked);
        Assert.Equal(refreshToken.ReplacedByToken, result.ReplacedByToken);
        Assert.Equal(refreshToken.UserId, result.UserId);
    }
    
    [Fact]
    public async Task GetById_RefreshToken_Success()
    {
        // Arrange
        var refreshToken = RepositoryTestsHelper.CreateRefreshToken();
        refreshToken.UserId = _user.UserId;
        await _unitOfWork.RefreshTokenRepository.Add(refreshToken);

        // Act
        var result = await _unitOfWork.RefreshTokenRepository.GetById(await _unitOfWork.GetLastInsertedId());

        // Assert
        Assert.True(result.RefreshTokenId > 0);
        Assert.Equal(refreshToken.Token, result.Token);
        Assert.Equal(refreshToken.Expires, result.Expires);
        Assert.Equal(refreshToken.Created, result.Created);
        Assert.Equal(refreshToken.Revoked, result.Revoked);
        Assert.Equal(refreshToken.ReplacedByToken, result.ReplacedByToken);
        Assert.Equal(refreshToken.UserId, result.UserId);
    }
    
    [Fact]
    public async Task GetByUserId_RefreshToken_Success()
    {
        // Arrange
        var refreshToken = RepositoryTestsHelper.CreateRefreshToken();
        refreshToken.UserId = _user.UserId;
        await _unitOfWork.RefreshTokenRepository.Add(refreshToken);

        // Act
        var result = await _unitOfWork.RefreshTokenRepository.GetByUserId(_user.UserId);

        // Assert
        var refreshTokens = result as RefreshToken[] ?? result.ToArray();
        Assert.NotEmpty(refreshTokens);
        Assert.True(refreshTokens.All(x => x.UserId == _user.UserId));
    }
    
    [Fact]
    public async Task Revoke_RefreshToken_Success()
    {
        // Arrange
        var refreshToken = RepositoryTestsHelper.CreateRefreshToken();
        refreshToken.UserId = _user.UserId;
        refreshToken.Revoked = SqlDateTime.MinValue.Value;
        await _unitOfWork.RefreshTokenRepository.Add(refreshToken);
        var id = await _unitOfWork.GetLastInsertedId();

        // Act
        await _unitOfWork.RefreshTokenRepository.Revoke(id);

        // Assert
        var result = await _unitOfWork.RefreshTokenRepository.GetById(id);
        Assert.NotNull(result);
        Assert.NotEqual(DateTime.MinValue, result.Revoked);
    }
    
    [Fact]
    public async Task RevokeAll_RefreshToken_Success()
    {
        // Arrange
        var refreshTokens = new List<RefreshToken>
        {
            RepositoryTestsHelper.CreateRefreshToken(),
            RepositoryTestsHelper.CreateRefreshToken()
        };
        foreach (var refreshToken in refreshTokens)
        {
            refreshToken.UserId = _user.UserId;
            await _unitOfWork.RefreshTokenRepository.Add(refreshToken);
        }

        // Act
        await _unitOfWork.RefreshTokenRepository.RevokeAll(_user.UserId);

        // Assert
        var result = await _unitOfWork.RefreshTokenRepository.GetByUserId(_user.UserId);
        var refreshTokensArray = result as RefreshToken[] ?? result.ToArray();
        Assert.NotEmpty(refreshTokensArray);
        Assert.True(refreshTokensArray.All(x => x.Revoked != DateTime.MinValue));
    }
    
    [Fact]
    public async Task Delete_RefreshToken_Success()
    {
        // Arrange
        var refreshToken = RepositoryTestsHelper.CreateRefreshToken();
        refreshToken.UserId = _user.UserId;
        await _unitOfWork.RefreshTokenRepository.Add(refreshToken);
        var id = await _unitOfWork.GetLastInsertedId();

        // Act
        await _unitOfWork.RefreshTokenRepository.Delete(id);

        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _unitOfWork.RefreshTokenRepository.GetById(id));
    }
    
    [Fact]
    public async Task GetAll_RefreshToken_Success()
    {
        // Arrange
        var refreshTokens = new List<RefreshToken>
        {
            RepositoryTestsHelper.CreateRefreshToken(),
            RepositoryTestsHelper.CreateRefreshToken()
        };
        foreach (var refreshToken in refreshTokens)
        {
            refreshToken.UserId = _user.UserId;
            await _unitOfWork.RefreshTokenRepository.Add(refreshToken);
        }

        // Act
        var result = await _unitOfWork.RefreshTokenRepository.GetAll();

        // Assert
        var refreshTokensArray = result as RefreshToken[] ?? result.ToArray();
        Assert.NotEmpty(refreshTokensArray);
        Assert.True(refreshTokensArray.All(x => x.RefreshTokenId > 0));
    }
    
    [Fact]
    public async Task GetById_RefreshToken_ForUpdate_Success()
    {
        // Arrange
        var refreshToken = RepositoryTestsHelper.CreateRefreshToken();
        refreshToken.UserId = _user.UserId;
        await _unitOfWork.RefreshTokenRepository.Add(refreshToken);
        var id = await _unitOfWork.GetLastInsertedId();

        // Act
        var result = await _unitOfWork.RefreshTokenRepository.GetById(id, true);

        // Assert
        Assert.True(result.RefreshTokenId > 0);
    }
    
    [Fact]
    public async Task GetById_RefreshToken_NotFound()
    {
        // Arrange
        var id = RepositoryTestsHelper.Random.Next();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _unitOfWork.RefreshTokenRepository.GetById(id));
    }
    
    [Fact]
    public async Task GetById_RefreshToken_ForUpdate_NotFound()
    {
        // Arrange
        var id = RepositoryTestsHelper.Random.Next();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _unitOfWork.RefreshTokenRepository.GetById(id, true));
    }
    
    [Fact]
    public async Task GetByUserIdAndToken_RefreshToken_Success()
    {
        // Arrange
        var refreshToken = RepositoryTestsHelper.CreateRefreshToken();
        refreshToken.UserId = _user.UserId;
        await _unitOfWork.RefreshTokenRepository.Add(refreshToken);

        // Act
        var result = await _unitOfWork.RefreshTokenRepository.GetByUserIdAndToken(_user.UserId, refreshToken.Token);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(refreshToken.Token, result.Token);
        Assert.Equal(refreshToken.UserId, result.UserId);
    }
    
    [Fact]
    public async Task GetByParams_RefreshToken_Success()
    {
        // Arrange
        var refreshToken = RepositoryTestsHelper.CreateRefreshToken();
        refreshToken.UserId = _user.UserId;
        await _unitOfWork.RefreshTokenRepository.Add(refreshToken);
        refreshToken.RefreshTokenId = await _unitOfWork.GetLastInsertedId();
        var parameters = new Dictionary<string, string>
        {
            { "refreshTokenId", refreshToken.RefreshTokenId.ToString() },
            { "userId", _user.UserId.ToString() },
            { "token", refreshToken.Token },
            { "created", refreshToken.Created.ToString(CultureInfo.InvariantCulture) },
            { "expires", refreshToken.Expires.ToString(CultureInfo.InvariantCulture) },
            { "revoked", refreshToken.Revoked.ToString(CultureInfo.InvariantCulture) },
            { "replacedByToken", refreshToken.ReplacedByToken }
        };

        // Act
        var result = await _unitOfWork.RefreshTokenRepository.GetByParams(parameters);

        // Assert
        var refreshTokens = result as RefreshToken[] ?? result.ToArray();
        Assert.NotEmpty(refreshTokens);
        Assert.True(refreshTokens.All(x => x.UserId == _user.UserId));
    }
    
    [Fact]
    public async Task GetByParams_RefreshToken_NotFound()
    {
        // Arrange
        var parameters = new Dictionary<string, string>
        {
            { "refreshTokenId", RepositoryTestsHelper.Random.Next().ToString() },
            { "userId", RepositoryTestsHelper.Random.Next().ToString() },
            { "token", RepositoryTestsHelper.RandomString(12) },
            { "created", DateTime.Now.ToString(CultureInfo.InvariantCulture) },
            { "expires", DateTime.Now.ToString(CultureInfo.InvariantCulture) },
            { "revoked", DateTime.Now.ToString(CultureInfo.InvariantCulture) },
            { "replacedByToken", RepositoryTestsHelper.RandomString(12) }
        };

        // Act
        var result = await _unitOfWork.RefreshTokenRepository.GetByParams(parameters);

        // Assert
        Assert.Empty(result);
    }
    
    [Fact]
    public async Task Update_RefreshToken_Success()
    {
        // Arrange
        var refreshToken = RepositoryTestsHelper.CreateRefreshToken();
        refreshToken.UserId = _user.UserId;
        await _unitOfWork.RefreshTokenRepository.Add(refreshToken);
        var id = await _unitOfWork.GetLastInsertedId();
        var updatedRefreshToken = RepositoryTestsHelper.CreateRefreshToken();
        updatedRefreshToken.UserId = _user.UserId;
        updatedRefreshToken.RefreshTokenId = id;

        // Act
        await _unitOfWork.RefreshTokenRepository.Update(updatedRefreshToken);

        // Assert
        var result = await _unitOfWork.RefreshTokenRepository.GetById(id);
        Assert.Equal(updatedRefreshToken.RefreshTokenId, result.RefreshTokenId);
        Assert.Equal(updatedRefreshToken.Token, result.Token);
        Assert.Equal(updatedRefreshToken.Expires, result.Expires);
        Assert.Equal(updatedRefreshToken.Created, result.Created);
        Assert.Equal(updatedRefreshToken.Revoked, result.Revoked);
        Assert.Equal(updatedRefreshToken.ReplacedByToken, result.ReplacedByToken);
        Assert.Equal(updatedRefreshToken.UserId, result.UserId);
    }
}