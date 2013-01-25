using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using Campus.Configuration;

namespace MdhsBus
{
    public class TemplatePreference
    {
        static private TemplatePreference mTemplatePreference = null;
        private System.Xml.XmlElement mconfig = null;

        //產生空白設定檔
        public void InitialPreference()
        {
            mconfig = new XmlDocument().CreateElement("繳費單樣版");
            mconfig.SetAttribute("Default", "true");

            XmlElement customize = mconfig.OwnerDocument.CreateElement("CustomizeTemplate");

            mconfig.AppendChild(customize);

            SmartSchool.Customization.Data.SystemInformation.Preference["繳費單樣版"] = mconfig;

            //ConfigData cd = Config.App["校車繳費單樣版NEW"];
            //cd["v1"] = "abc";
            //cd["v2"] = "def";
            //cd.Save();

            //string v1 = cd["v1"];

        }

        public void Refresh()
        {
            //從SmartSchool當中取得設定變數，若是為null則重新設定

            mconfig = SmartSchool.Customization.Data.SystemInformation.Preference["繳費單樣版"];
            if (mconfig == null)
                InitialPreference();

        }

        private TemplatePreference()
        {
            Refresh();
        }

        static public TemplatePreference GetInstance()
        {
            if (mTemplatePreference == null)
                mTemplatePreference = new TemplatePreference();

            return mTemplatePreference;
        }

        public bool UseDefaultTemplate
        {
            get
            {
                if (mconfig.GetAttribute("Default").Equals(""))
                    mconfig.SetAttribute("Default", "true");

                return bool.Parse(mconfig.GetAttribute("Default"));
            }
            set
            {
                mconfig.SetAttribute("Default", value.ToString());
            }
        }

        public string CustomizeTemplateString
        {
            set
            {
                XmlElement CustomizeTemplateElm = mconfig.OwnerDocument.CreateElement("CustomizeTemplate");
                CustomizeTemplateElm.InnerText = value;
                mconfig.ReplaceChild(CustomizeTemplateElm, mconfig.SelectSingleNode("CustomizeTemplate"));
            }
        }

        public byte[] CustomizeTemplateByte
        {
            get
            {
                byte[] Buffer = null;
                XmlElement customize = (XmlElement)mconfig.SelectSingleNode("CustomizeTemplate");

                if (customize != null)
                {
                    string templateBase64 = customize.InnerText;
                    Buffer = Convert.FromBase64String(templateBase64);
                    return Buffer;
                }
                else
                    return null;
            }
        }

        public MemoryStream CustomizeTemplate
        {
            get
            {
                MemoryStream Template = null;
                byte[] Buffer = null;
                XmlElement customize = (XmlElement)mconfig.SelectSingleNode("CustomizeTemplate");

                if (customize != null)
                {
                    string templateBase64 = customize.InnerText;
                    Buffer = Convert.FromBase64String(templateBase64);
                    Template = new MemoryStream(Buffer);

                    return Template;
                }
                else
                    return null;
            }
        }
    }
}
