namespace Services.TokenServices.Interfaces {

    /// <summary>
    /// A token generator that can generate all types of tokens.
    /// </summary>
    public interface IHybridTokenGenerator: IAccessTokenGenerator, IAuthTokenGenerator, IRefreshTokenGenerator {
    
    }
}
