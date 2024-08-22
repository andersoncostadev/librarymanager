using Core.Repositories;
using MediatR;

namespace Core.Queries.v1.User.GetAll
{
    public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, GetAllUserQueryReponse>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetAllUserQueryReponse> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync();

            if (users == null)
                throw new ApplicationException("Error getting users");

            return new GetAllUserQueryReponse(users);
        }
    }
}
