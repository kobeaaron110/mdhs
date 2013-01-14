﻿using System;
using System.Collections.Generic;
using System.Text;
using SmartSchool.Common;
using FISCA.DSAUtil;
using System.Xml;

namespace 班級學生電子報表
{
    public static class EditElectronicPaper
    {
        public static string Insert(string name, string schoolYear, string semester, string viewerType, Dictionary<string, string> metadata)
        {
            DSXmlHelper dsreq = new DSXmlHelper("Request");
            dsreq.AddElement("ElectronicPaper");
            dsreq.AddElement("ElectronicPaper", "Name", name);
            dsreq.AddElement("ElectronicPaper", "SchoolYear", schoolYear);
            dsreq.AddElement("ElectronicPaper", "Semester", semester);
            dsreq.AddElement("ElectronicPaper", "ViewerType", viewerType);

            if (metadata != null)
            {
                DSXmlHelper hlpmd = new DSXmlHelper("Metadata");
                foreach (KeyValuePair<string, string> each in metadata)
                {
                    XmlElement item = hlpmd.AddElement("Item");
                    item.SetAttribute("Name", each.Key);
                    item.SetAttribute("Value", each.Value);
                }
                dsreq.AddElement("ElectronicPaper", hlpmd.BaseElement);
            }

            DSResponse dsrsp = FeatureBase.CallService("SmartSchool.ElectronicPaper.Insert", new DSRequest(dsreq));
            if (dsrsp.HasContent)
            {
                DSXmlHelper helper = dsrsp.GetContent();
                string newid = helper.GetText("NewID");
                return newid;
            }
            return "";
        }

        public static void UpdatePaperName(string new_name, string id)
        {
            DSXmlHelper dsreq = new DSXmlHelper("Request");
            dsreq.AddElement("ElectronicPaper");
            dsreq.AddElement("ElectronicPaper", "Name", new_name);
            dsreq.AddElement("ElectronicPaper", "Condition");
            dsreq.AddElement("ElectronicPaper/Condition", "ID", id);
            FeatureBase.CallService("SmartSchool.ElectronicPaper.Update", new DSRequest(dsreq));
        }

        public static void Delete(string id)
        {
            DSXmlHelper dsreq = new DSXmlHelper("Request");
            dsreq.AddElement("ElectronicPaper");
            dsreq.AddElement("ElectronicPaper", "ID", id);
            FeatureBase.CallService("SmartSchool.ElectronicPaper.Delete", new DSRequest(dsreq));
        }

        public static void InsertPaperItem(DSXmlHelper request)
        {
            FeatureBase.CallService("SmartSchool.ElectronicPaper.InsertPaperItem", new DSRequest(request));
        }

        public static void DeletePaperItem(params string[] item_ids)
        {
            DSXmlHelper helper = new DSXmlHelper("Request");
            helper.AddElement("Paper");
            foreach (string each_id in item_ids)
                helper.AddElement("Paper", "PaperItemID", each_id);
            FeatureBase.CallService("SmartSchool.ElectronicPaper.DeletePaperItem", new DSRequest(helper));
        }
    }
}