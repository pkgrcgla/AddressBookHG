﻿using AddressBookHG_BL.InterfacesOfManagers;
using AddressBookHG_DL.InterfaceOfRepos;
using AddressBookHG_EL.Entities;
using AddressBookHG_EL.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookHG_BL.ImplementationOfManagers
{
    public class UserForgotPasswordTokensManager :
         Manager<UserForgotPasswordTokensDTO, UserForgotPasswordTokens, int>, IUserForgotPasswordTokensManager
    {
        public UserForgotPasswordTokensManager(IUserForgotPasswordTokensRepo repo, IMapper mapper) : base(repo, mapper, new string[] { "User" })
        {

        }

        public bool AddNewForgotPasswordToken(UserForgotPasswordTokensDTO userToken)
        {
            try
            {
                // token tablosuna ekleme yapılınca önceki tokenların geçersiz olması gerekli
                //1)Tabloya Tokenı ekleyeceğiz 

                var addResult = this.Add(userToken);
                if (addResult.IsSuccess)
                {
                    //2)önceki tokenların geçersiz olması == IsActive =0 yapılacak

                    var oldTokens = this.GetAll(x => x.UserId == userToken.UserId
                    && x.Id != addResult.Data.Id);
                    foreach (var item in oldTokens.Data)
                    {
                        item.User = null;
                        item.IsActive = false;
                        var updateResult = this.Update(item);

                        if (!updateResult.IsSuccess)
                            throw new Exception("UserForgotPasswordTokens tablosuna ekleme yapılamadı!");
                    }

                    return true;
                }
                else
                {
                    throw new Exception("UserForgotPasswordTokens tablosuna ekleme yapılamadı!");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
