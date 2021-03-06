﻿using System;
using System.Collections.Generic;
using System.Text;
using ESBasic.Security;
using ESBasic.ObjectManagement.Managers;
using System.Configuration;
using ESBasic;
using JustLib.Records;
using DataRabbit.DBAccessing.Application;
using DataRabbit.DBAccessing;
using DataRabbit.DBAccessing.ORM;
using DataRabbit;

namespace GGTalk.Server
{  
    /// <summary>
    /// 真实数据库。
    /// </summary>
    public class RealDB : DefaultChatRecordPersister, IDBPersister
    {
        private TransactionScopeFactory transactionScopeFactory;

        public RealDB(string dbName, string dbIP,string saPwd )
        {
            DataConfiguration config = new SqlDataConfiguration(dbIP, "sa", saPwd, dbName);
            this.transactionScopeFactory = new TransactionScopeFactory(config);
            this.transactionScopeFactory.Initialize();
            base.Initialize(this.transactionScopeFactory);
        }

        public void InsertUser(GGUser t)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGUser> accesser = scope.NewOrmAccesser<GGUser>();
                accesser.Insert(t);
                scope.Commit();
            }
        }

        public void InsertGroup(GGGroup t)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGGroup> accesser = scope.NewOrmAccesser<GGGroup>();
                accesser.Insert(t);
                scope.Commit();
            }
        }

        public void DeleteGroup(string groupID)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGGroup> accesser = scope.NewOrmAccesser<GGGroup>();
                accesser.Delete(groupID);
                scope.Commit();
            }
        }

        public void UpdateUser(GGUser t)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGUser> accesser = scope.NewOrmAccesser<GGUser>();
                accesser.Update(t);
                scope.Commit();
            }
        }

        public void UpdateUserFriends(GGUser t)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGUser> accesser = scope.NewOrmAccesser<GGUser>();
                accesser.Update(new ColumnUpdating(GGUser._Friends, t.Friends), new Filter(GGUser._UserID, t.UserID));
                scope.Commit();
            }
        }

        public void UpdateGroup(GGGroup t)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGGroup> accesser = scope.NewOrmAccesser<GGGroup>();
                accesser.Update(t);
                scope.Commit();
            }
        }

        public List<GGUser> GetAllUser()
        {
            List<GGUser> list = new List<GGUser>();
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGUser> accesser = scope.NewOrmAccesser<GGUser>();
                list = accesser.GetAll();
                scope.Commit();
            }
            return list;
        }

        public List<GGGroup> GetAllGroup()
        {
            List<GGGroup> list = new List<GGGroup>();
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGGroup> accesser = scope.NewOrmAccesser<GGGroup>();
                list = accesser.GetAll();
                scope.Commit();
            }
            return list;
        }

        public void ChangeUserPassword(string userID, string newPasswordMD5)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGUser> accesser = scope.NewOrmAccesser<GGUser>();
                accesser.Update(new ColumnUpdating(GGUser._PasswordMD5, newPasswordMD5), new Filter(GGUser._UserID, userID));
                scope.Commit();
            }
        }

        public void ChangeUserGroups(string userID, string groups)
        {
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGUser> accesser = scope.NewOrmAccesser<GGUser>();
                accesser.Update(new ColumnUpdating(GGUser._Groups, groups), new Filter(GGUser._UserID, userID));
                scope.Commit();
            }
        }

        public void UpdateGroupInfo(GGGroup t)
        {
            this.UpdateGroup(t);
        }

        public GGUser GetUser(string userID)
        {
            GGUser user = null;
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGUser> accesser = scope.NewOrmAccesser<GGUser>();
                user = accesser.GetOne(userID);
                scope.Commit();
            }
            return user;
        }

        public string GetUserPassword(string userID)
        {
            object pwd = null;
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGUser> accesser = scope.NewOrmAccesser<GGUser>();
                pwd = accesser.GetColumnValue(userID, GGUser._PasswordMD5);
                scope.Commit();
            }
            if (pwd == null)
            {
                return null;
            }
            return pwd.ToString();
        }

        public GGGroup GetGroup(string groupID)
        {
            GGGroup group = null;
            using (TransactionScope scope = this.transactionScopeFactory.NewTransactionScope())
            {
                IOrmAccesser<GGGroup> accesser = scope.NewOrmAccesser<GGGroup>();
                group = accesser.GetOne(groupID);
                scope.Commit();
            }
            return group;
        }
    }
   
}
