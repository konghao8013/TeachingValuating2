using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Web.Administration;

namespace Install
{
    public class IISOperation
    {
        /// <summary> 
        /// Create a new web site.  创建WEB网站
        /// </summary> 
        /// <param name="siteName"></param> 
        /// <param name="bindingInfo">"*:&lt;port&gt;:&lt;hostname&gt;" <example>"*:80:myhost.com"</example></param> 
        /// <param name="physicalPath"></param> 
        public static void CreateSite(string siteName, string bindingInfo, string physicalPath)
        {
            createSite(siteName, "http", bindingInfo, physicalPath, true, siteName + "Pool", ProcessModelIdentityType.NetworkService, null, null, ManagedPipelineMode.Integrated, null);
        }

        private static void createSite(string siteName, string protocol, string bindingInformation, string physicalPath,
                bool createAppPool, string appPoolName, ProcessModelIdentityType identityType,
                string appPoolUserName, string appPoolPassword, ManagedPipelineMode appPoolPipelineMode, string managedRuntimeVersion)
        {
            using (ServerManager mgr = new ServerManager())
            {
                Site site = mgr.Sites.Add(siteName, protocol, bindingInformation, physicalPath);

                // PROVISION APPPOOL IF NEEDED 
                if (createAppPool)
                {
                    ApplicationPool pool = mgr.ApplicationPools.Add(appPoolName);
                    if (pool.ProcessModel.IdentityType != identityType)
                    {
                        pool.ProcessModel.IdentityType = identityType;
                    }
                    if (!String.IsNullOrEmpty(appPoolUserName))
                    {
                        pool.ProcessModel.UserName = appPoolUserName;
                        pool.ProcessModel.Password = appPoolPassword;
                    }
                    if (appPoolPipelineMode != pool.ManagedPipelineMode)
                    {
                        pool.ManagedPipelineMode = appPoolPipelineMode;
                    }

                    site.Applications["/"].ApplicationPoolName = pool.Name;
                }

                mgr.CommitChanges();
            }
        }


        /// <summary> 
        /// Delete an existent web site. 
        /// </summary> 
        /// <param name="siteName">Site name.</param> 
        public static void DeleteSite(string siteName)
        {
            using (ServerManager mgr = new ServerManager())
            {
                Site site = mgr.Sites[siteName];
                if (site != null)
                {
                    mgr.Sites.Remove(site);
                    mgr.CommitChanges();
                }
            }
        }
        /// <summary>
        /// 创建虚拟目录
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="vDirName"></param>
        /// <param name="physicalPath"></param>
        public static void CreateVDir(string siteName, string vDirName, string physicalPath)
        {
            using (ServerManager mgr = new ServerManager())
            {
                Site site = mgr.Sites[siteName];
                if (site == null)
                {
                    throw new ApplicationException(String.Format("Web site {0} does not exist", siteName));
                }
                site.Applications.Add("/" + vDirName, physicalPath);
                mgr.CommitChanges();
            }
        }
        /// <summary>
        /// 删除虚拟目录
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="vDirName"></param>
        public static void DeleteVDir(string siteName, string vDirName)
        {
            using (ServerManager mgr = new ServerManager())
            {
                Site site = mgr.Sites[siteName];
                if (site != null)
                {
                    Microsoft.Web.Administration.Application app = site.Applications["/" + vDirName];
                    if (app != null)
                    {
                        site.Applications.Remove(app);
                        mgr.CommitChanges();
                    }
                }
            }
        }

        /// <summary> 
        /// Delete an existent web site app pool. 删除引用程序池
        /// </summary> 
        /// <param name="appPoolName">App pool name for deletion.</param> 
        public static void DeletePool(string appPoolName)
        {
            using (ServerManager mgr = new ServerManager())
            {
                ApplicationPool pool = mgr.ApplicationPools[appPoolName];
                if (pool != null)
                {
                    mgr.ApplicationPools.Remove(pool);
                    mgr.CommitChanges();
                }
            }
        }
        /// <summary>
        /// 添加站点默认文档
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="defaultDocName"></param>
        public static void AddDefaultDocument(string siteName, string defaultDocName)
        {
            using (ServerManager mgr = new ServerManager())
            {
                Configuration cfg = mgr.GetWebConfiguration(siteName);
                ConfigurationSection defaultDocumentSection = cfg.GetSection("system.webServer/defaultDocument");
                ConfigurationElement filesElement = defaultDocumentSection.GetChildElement("files");
                ConfigurationElementCollection filesCollection = filesElement.GetCollection();

                foreach (ConfigurationElement elt in filesCollection)
                {
                    if (elt.Attributes["value"].Value.ToString() == defaultDocName)
                    {
                        return;
                    }
                }

                try
                {
                    ConfigurationElement docElement = filesCollection.CreateElement();
                    docElement.SetAttributeValue("value", defaultDocName);
                    filesCollection.Add(docElement);
                }
                catch (Exception) { }   //this will fail if existing 

                mgr.CommitChanges();
            }
        }
        /// <summary>
        /// 检查目录是否存在
        /// </summary>
        /// <param name="siteName"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool VerifyVirtualPathIsExist(string siteName, string path)
        {
            using (ServerManager mgr = new ServerManager())
            {
                Site site = mgr.Sites[siteName];
                if (site != null)
                {
                    foreach (Microsoft.Web.Administration.Application app in site.Applications)
                    {
                        if (app.Path.ToUpper().Equals(path.ToUpper()))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        /// <summary>
        /// 检查站点是否存在
        /// </summary>
        /// <param name="siteName"></param>
        /// <returns></returns>
        public static bool VerifyWebSiteIsExist(string siteName)
        {
            using (ServerManager mgr = new ServerManager())
            {
                for (int i = 0; i < mgr.Sites.Count; i++)
                {
                    if (mgr.Sites[i].Name.ToUpper().Equals(siteName.ToUpper()))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        /// <summary>
        ///  检查Bindings信息。
        /// </summary>
        /// <param name="bindingInfo"></param>
        /// <returns></returns>
        public static bool VerifyWebSiteBindingsIsExist(string bindingInfo)
        {
            string temp = string.Empty;
            using (ServerManager mgr = new ServerManager())
            {
                for (int i = 0; i < mgr.Sites.Count; i++)
                {
                    foreach (Microsoft.Web.Administration.Binding b in mgr.Sites[i].Bindings)
                    {
                        temp = b.BindingInformation;
                        if (temp.IndexOf('*') < 0)
                        {
                            temp = "*" + temp;
                        }
                        if (temp.Equals(bindingInfo))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
