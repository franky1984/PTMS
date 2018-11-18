using Learun.Util;
using System;
using System.Data;
using System.Collections.Generic;

namespace Learun.Application.TwoDevelopment.LR_CodeDemo
{
    /// <summary>
    /// 版 本 Learun-ADMS V7.0.0 众数敏捷开发框架
    /// Copyright (c) 2013-2018 众数信息技术有限公司
    /// 创 建：超级管理员
    /// 日 期：2018-11-07 15:08
    /// 描 述：临时工管理
    /// </summary>
    public class tempUserBLL : tempUserIBLL
    {
        private tempUserService tempUserService = new tempUserService();

        #region 获取数据

        /// <summary>
        /// 获取页面显示列表数据
        /// <summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<LR_Base_TempUserEntity> GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                return tempUserService.GetPageList(pagination, queryJson);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }

        /// <summary>
        /// 判断被替工人是否在此次活动里
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="keyValue"></param>
        public int CheckReplacement( string orderID, string keyValue )
        {
            return tempUserService.CheckReplacement(orderID, keyValue);
        }

        /// <summary>
        /// 将人员添加到黑名单
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public void AddBlack( string keyValue, F_Base_BlackListEntity entity )
        {
            try
            {
                tempUserService.AddBlack( keyValue, entity );
            }
            catch( Exception ex )
            {
                if( ex is ExceptionEx )
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException( ex );
                }
            }
        }

        /// <summary>
        /// 判断用户是否已加入黑名单
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public string CheckUserBlack( string keyValue )
        {
            try
            {
                return tempUserService.CheckUserBlack( keyValue );
            }
            catch( Exception ex )
            {
                if( ex is ExceptionEx )
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException( ex );
                }
            }
        }

        /// <summary>
        /// 获取LR_Base_TempUser表实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public LR_Base_TempUserEntity GetLR_Base_TempUserEntity(string keyValue)
        {
            try
            {
                return tempUserService.GetLR_Base_TempUserEntity(keyValue);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 删除实体数据
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public void DeleteEntity(string keyValue)
        {
            try
            {
                tempUserService.DeleteEntity(keyValue);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }

        /// <summary>
        /// 保存实体数据（新增、修改）
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public void SaveEntity(string keyValue, LR_Base_TempUserEntity entity)
        {
            try
            {
                tempUserService.SaveEntity(keyValue, entity);
            }
            catch (Exception ex)
            {
                if (ex is ExceptionEx)
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException(ex);
                }
            }
        }

        /// <summary>
        /// 保存实体数据（新增、修改）
        /// <param name="keyValue">主键</param>
        /// <summary>
        /// <returns></returns>
        public int WorkSubstituteSaveForm( string keyValue,string orderID, LR_Base_TempUserEntity entity )
        {
            try
            {
                return tempUserService.WorkSubstituteSaveForm( keyValue,orderID, entity );
            }
            catch( Exception ex )
            {
                if( ex is ExceptionEx )
                {
                    throw;
                }
                else
                {
                    throw ExceptionEx.ThrowBusinessException( ex );
                }
            }
        }

        #endregion

    }
}
