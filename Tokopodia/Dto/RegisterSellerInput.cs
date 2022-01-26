namespace Tokopodia.Dto
{
    public record RegisterSellerInput
    (
        string UserId,
        string ShopName,
        string Username,
        string Password,
        string Address
    );
}
