using Dapper;
using RealEstate_Dapper_Api.Dtos.MessageDtos;
using RealEstate_Dapper_Api.Models.DapperContext;

namespace RealEstate_Dapper_Api.Repositories.MessageRepositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly Context _context;

        public MessageRepository(Context context)
        {
            _context = context;
        }

        public async Task<List<ResultInBoxMessageDto>> GetInBoxMessageLast3ListByReciever(int id)
        {
            string query = "Select Top(3) MessageID,Name,Subject,Detail,SendDate,IsRead,UserImageUrl From Message Inner Join AppUser on Message.Sender = AppUser.UserID Where Reciever = @recieverId Order By MessageID Desc";
            var parameters = new DynamicParameters();
            parameters.Add("@recieverId", id);
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<ResultInBoxMessageDto>(query, parameters);
                return values.ToList();
            }
        }
    }
}
