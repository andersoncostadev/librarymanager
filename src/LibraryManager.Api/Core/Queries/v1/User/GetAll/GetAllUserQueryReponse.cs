using Core.Entities.v1;

namespace Core.Queries.v1.User.GetAll
{
    public class GetAllUserQueryReponse
    {
        public GetAllUserQueryReponse(IEnumerable<UserEntity>? users)
        {
            Users = users;
        }

        public IEnumerable<UserEntity>? Users { get; set; }
    }
}
