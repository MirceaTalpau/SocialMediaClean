using Dapper;
using LinkedFit.DOMAIN.Models.Entities.Group;
using SocialMediaClean.INFRASTRUCTURE.Interfaces;
using System.Data;

namespace LinkedFit.PERSISTANCE.Repositories
{
    public class GroupRepository
    {
        private readonly IDbConnectionFactory _db;
        public GroupRepository(IDbConnectionFactory db)
        {
            _db = db;
        }

        public async Task<int> CreateGroupAsync(Group group)
        {
            using (var conn = await _db.CreateDbConnectionAsync())
            {
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Name", group.Name);
                    parameters.Add("@Description", group.Description);
                    parameters.Add("@AuthorID", group.AuthorID);
                    var groupID = await conn.QuerySingleAsync<int>("usp_Group_CreateGroup", parameters, commandType: CommandType.StoredProcedure);
                    return groupID;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public async Task AddGroupMember(int groupId, int userId)
        {
            using(var conn = await _db.CreateDbConnectionAsync())
            {
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@GroupID", groupId);
                    parameters.Add("@UserID", userId);
                    await conn.ExecuteAsync("usp_Group_AddGroupMember", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
