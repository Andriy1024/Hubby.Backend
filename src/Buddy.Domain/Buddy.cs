﻿using System;
using System.Collections.Generic;
using System.Linq;
using Buddy.Domain.Enums;
using Buddy.Domain.Events;
using Core.Domain.Entities;

namespace Buddy.Domain
{
    public class Buddy: Aggregate<Buddy>
    {
        #region Fields

        private const int GenresAmount = 5;
        private List<string> _genreIds;
        private List<Task> _tasks;

        #endregion

        #region Properties

        public string RegionId { get; private set; }
        public IEnumerable<string> GenreIds => _genreIds.AsEnumerable();
        public string CurrentGroupId { get; private set; }

        #endregion

        #region Public Methods

        public void Create(string accountId, string buddyId)
        {
            if (Version > 0)
                throw new InvalidOperationException($"Buddy with Id {accountId} already exists");

            var e = new BuddyCreated(accountId, buddyId);
            Publish(e);
        }

        public void ChooseRegion(string regionId)
        {
            var e = new RegionChosen(Id, regionId);
            Publish(e);
        }

        public void ChooseGenres(IList<string> genreIds)
        {
            if (genreIds.Count != GenresAmount)
                throw new InvalidOperationException($"You have to pick exactly {GenresAmount} genres");

            var e = new GenresChosen(Id, genreIds);
            Publish(e);
        }

        public void JoinGroup(string groupId, BuddyJoinType joinType)
        {
            if(joinType == BuddyJoinType.New && CurrentGroupId != null)
                throw new InvalidOperationException("Can't join group when still being in another one");

            var e = new GroupJoined(Id, groupId);
            Publish(e);
        }

        public void LeaveGroup()
        {
            if (CurrentGroupId == null)
                throw new InvalidOperationException($"Buddy {Id} isn't in a group yet");

            var e = new GroupLeft(Id, CurrentGroupId);
            Publish(e);
        }

        public void UpdateTasks(IEnumerable<Task> tasks)
        {
            var newTasks = tasks.Except(_tasks).ToList();
            if (!newTasks.Any()) return;
            var e = new TasksUpdated(Id, newTasks.ToList());
            Publish(e);
        }

        public void SetTaskStatus(string taskType, TaskStatus newStatus)
        {
            var task = _tasks.FirstOrDefault(x => x.Type == taskType);

            if (task == null)
                throw new InvalidOperationException($"Task with type {taskType} doesn't exists");

            task.SetStatus(newStatus);
            var e = new TaskStatusSet(Id, taskType, newStatus);
            Publish(e);
        }

        #endregion

        #region Private Methods: Events

        private void When(BuddyCreated e)
        {
            Id = e.Id;
            _genreIds = new List<string>();
            _tasks = new List<Task>();
        }

        private void When(RegionChosen e)
        {
            RegionId = e.RegionId;
        }

        private void When(GenresChosen e)
        {
            _genreIds = e.GenreIds.ToList();
        }

        private void When(GroupJoined e)
        {
            CurrentGroupId = e.GroupId;
        }

        private void When(GroupLeft e)
        {
            CurrentGroupId = null;
        }

        private void When(TasksUpdated e)
        {
            _tasks.AddRange(e.Tasks);
        }

        private void When(TaskStatusSet e)
        {
            var index = _tasks.FindIndex(task => task.Type == e.Type);
            _tasks[index].SetStatus(e.NewStatus);
        }

        #endregion
    }
}
