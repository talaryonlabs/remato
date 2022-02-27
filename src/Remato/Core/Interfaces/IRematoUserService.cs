namespace Remato
{
    public interface IRematoUserService : IRematoServiceEntity<UserEntity, IUserParams>
    {
        
    }
    
    public interface IRematoUsersService : IRematoServiceEntities<UserEntity, IUserParams>
    {
        
    }
}