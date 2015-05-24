using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Web.Administration;

namespace Install
{
    public class IISHelp
    {
        public void CreateWEB(string name, int port, string path)
        {
            DeleteWeb(name);
            RemovePort(port);

            string BindArgs = string.Format("*:{0}:", port); //绑定参数，注意格式
            string apl = "http"; //类型
            //DefaultAppPool
            var sm = new ServerManager();

            sm.Sites.Add(name, apl, BindArgs, path);
            sm.CommitChanges();
            SettingWebApplication(name, name);

        }

        public void SettingWebApplication(string webName, string aName)
        {
            if (IsApplication(aName))
            {
                SetApplicationVersion(aName);

            }
            else
            {
                CreateApplication(aName);
            }
            var sm = new ServerManager();
            foreach (var web in sm.Sites)
            {
                if (web.Name.ToLower() == webName.ToLower())
                {
                    web.ApplicationDefaults.ApplicationPoolName = aName;
                }
            }
            sm.CommitChanges();
        }

        public bool IsApplication(string name)
        {
            var sm = new ServerManager();
            var reset = false;
            foreach (var application in sm.ApplicationPools)
            {
             
                if (application.Name.ToLower() == name.ToLower())
                {
                    reset = true;
                    break;
                }
            }
            sm.CommitChanges();
            return reset;
        }

        public void CreateApplication(string name)
        {
            var sm = new ServerManager();
            sm.ApplicationPools.Add(name);

            sm.CommitChanges();
            SetApplicationVersion(name);
        }

        public void SetApplicationVersion(string name)
        {
            var sm = new ServerManager();
            try
            {
                sm.ApplicationPools[name].ManagedRuntimeVersion = "v4.0";
                sm.ApplicationPools[name].ManagedPipelineMode = ManagedPipelineMode.Integrated;
                //托管模式Integrated为集成 Classic为经典
                sm.ApplicationPools[name].ProcessModel.IdentityType = ProcessModelIdentityType.LocalSystem;
                sm.CommitChanges();
            }
            catch (Exception e)
            {
                throw new Exception("ApplicationPools "+name + e.Message);
            }
        }

        public void DeleteWeb(string name)
        {
            var sm = new ServerManager();
            var length = sm.Sites.Count;
            for (var i = 0; i < length; i++)
            {
                if (sm.Sites[i].Name.ToLower() == name.ToLower())
                {
                    sm.Sites.RemoveAt(i);

                    break;
                }
            }
            sm.CommitChanges();
        }

        public void RemovePort(int port)
        {
            var sm = new ServerManager();
            foreach (var web in sm.Sites)
            {
                var length = web.Bindings.Count;
                for (int i = 0; i < length; i++)
                {
                    if (web.Bindings[i]!=null&&web.Bindings[i].EndPoint.Port == port)
                    {
                        web.Bindings.RemoveAt(i);
                        break;

                    }
                }
            }
            sm.CommitChanges();
        }

    }
}
