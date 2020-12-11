using System.Collections.Generic;
using MySocialNetwork.DAO;
using MySocialNetwork.DTO;
using MySocialNetwork.Utils;

namespace MySocialNetwork.Services
{
    public class GroupService
    {
        private GroupMapper groupMapper = new GroupMapper();
        private GroupManager groupManager = new GroupManager();
        public void AddNewGroup(GroupCreationDto newGroup)
        {
            Group group = groupMapper.GetNewGroup(newGroup);
            groupManager.AddNewGroup(group, newGroup.CreatorId);
        }

        public List<GroupDto> GetGroups(int userId)
        {
            List<Group> groups = groupManager.GetGroupsOfUser(userId);
            List<GroupDto> groupDtos = groupMapper.GetGroupDtoList(groups, userId);
            return groupDtos;
        }
    }
}