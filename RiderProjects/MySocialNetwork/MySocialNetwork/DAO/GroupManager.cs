using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MySocialNetwork.DTO;

namespace MySocialNetwork.DAO
{
    public class GroupManager
    {
        private SocialNetworkDbContext dbContext = new SocialNetworkDbContext();
        private PageManager pageManager = new PageManager();
        public void AddNewGroup(Group group, int creatorId)
        {
            using (DbContextTransaction transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    Group addedGroup = dbContext.Groups.Add(group);
                    Wall wall = pageManager.CreateWall(WallTypes.Main, "Main");
                    addedGroup.Walls.Add(wall);
                    dbContext.SaveChanges();
                    Subscribe(addedGroup.Id, creatorId, GroupRoles.Admin);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void Subscribe(int groupId, int userId, GroupRoles role)
        {
            GroupRole groupRole = dbContext.GroupRoles.Where(r => r.Title == role.ToString()).First();
            ReaderProfileState state = dbContext.ReaderProfileStates
                .Where(s => s.Title == ReaderProfileStates.FullAccess.ToString()).First();
            ReaderProfile profile = new ReaderProfile()
            {
                GroupId = groupId,
                UserId = userId,
                GroupRoleId = groupRole.Id,
                ReaderProfileStateId = state.Id
            };
            dbContext.ReaderProfiles.Add(profile);
            dbContext.SaveChanges();
        }

        public List<Group> GetGroupsOfUser(int userId)
        {
            List<Group> groups = new List<Group>();
            IEnumerable<ReaderProfile> profiles = dbContext.ReaderProfiles.Where(p => p.UserId == userId);
            foreach (ReaderProfile profile in profiles)
            {
                Group group = dbContext.Groups.Where(g => g.Id == profile.Id).Include(g => g.ReaderProfiles).Include(g => g.Walls).First();
                groups.Add(group);                
            }

            return groups;
        } 
    }
}