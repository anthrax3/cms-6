﻿using System;
using System.Xml;
using SiteServer.Utils;
using SiteServer.CMS.StlParser.Model;
using SiteServer.CMS.StlParser.Parsers;
using SiteServer.CMS.StlParser.Utility;

namespace SiteServer.CMS.StlParser.StlElement
{
    [StlClass(Usage = "翻页项容器", Description = "通过 stl:pageItems 标签在模板中插入翻页项的容器，当不需要翻页时容器内的内容不显示")]
    public class StlPageItems
    {
        private StlPageItems() { }
        public const string ElementName = "stl:pageItems";

        private static readonly Attr Context = new Attr("context", "所处上下文");

        //对“翻页项容器”（stl:pageItems）元素进行解析，此元素在生成页面时单独解析，不包含在ParseStlElement方法中。
        public static string Parse(string stlElement, PageInfo pageInfo, int channelId, int contentId, int currentPageIndex, int pageCount, int totalNum, EContextType contextType)
        {
            pageInfo.AddPageBodyCodeIfNotExists(PageInfo.Const.Jquery);
            string parsedContent;
            try
            {
                var xmlDocument = StlParserUtility.GetXmlDocument(stlElement, false);
                XmlNode node = xmlDocument.DocumentElement;
                node = node?.FirstChild;

                var ie = node?.Attributes?.GetEnumerator();
                if (ie != null)
                {
                    while (ie.MoveNext())
                    {
                        var attr = (XmlAttribute)ie.Current;
                        if (attr == null) continue;

                        var name = attr.Name;

                        if (StringUtils.EqualsIgnoreCase(name, Context.Name))
                        {
                            contextType = EContextTypeUtils.GetEnumType(attr.Value);
                        }
                    }
                }

                if (pageCount <= 1)
                {
                    return string.Empty;
                }

                bool isXmlContent;
                var index = stlElement.IndexOf(">", StringComparison.Ordinal) + 1;
                var length = stlElement.LastIndexOf("<", StringComparison.Ordinal) - index;
                if (index <= 0 || length <= 0)
                {
                    stlElement = node?.InnerXml;
                    isXmlContent = true;
                }
                else
                {
                    stlElement = stlElement.Substring(index, length);
                    isXmlContent = false;
                }

                parsedContent = StlPageElementParser.ParseStlPageItems(stlElement, pageInfo, channelId, contentId, currentPageIndex, pageCount, totalNum, isXmlContent, contextType);
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, stlElement, ex);
            }

            return parsedContent;
        }

        public static string ParseInSearchPage(string stlElement, PageInfo pageInfo, string ajaxDivId, int channelId, int currentPageIndex, int pageCount, int totalNum)
        {
            string parsedContent;
            try
            {
                var xmlDocument = StlParserUtility.GetXmlDocument(stlElement, false);
                XmlNode node = xmlDocument.DocumentElement;
                node = node?.FirstChild;

                if (pageCount <= 1)
                {
                    return string.Empty;
                }

                //bool isXmlContent;
                var index = stlElement.IndexOf(">", StringComparison.Ordinal) + 1;
                var length = stlElement.LastIndexOf("<", StringComparison.Ordinal) - index;
                if (index <= 0 || length <= 0)
                {
                    stlElement = node?.InnerXml;
                    //isXmlContent = true;
                }
                else
                {
                    stlElement = stlElement.Substring(index, length);
                    //isXmlContent = false;
                }

                parsedContent = StlPageElementParser.ParseStlPageItemsInSearchPage(stlElement, pageInfo, ajaxDivId, channelId, currentPageIndex, pageCount, totalNum);
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, stlElement, ex);
            }

            return parsedContent;
        }

        public static string ParseInDynamicPage(string stlElement, PageInfo pageInfo, string pageUrl, int channelId, int currentPageIndex, int pageCount, int totalNum, bool isPageRefresh, string ajaxDivId)
        {
            string parsedContent;
            try
            {
                var xmlDocument = StlParserUtility.GetXmlDocument(stlElement, false);
                XmlNode node = xmlDocument.DocumentElement;
                node = node?.FirstChild;

                if (pageCount <= 1)
                {
                    return string.Empty;
                }

                var index = stlElement.IndexOf(">", StringComparison.Ordinal) + 1;
                var length = stlElement.LastIndexOf("<", StringComparison.Ordinal) - index;
                if (index <= 0 || length <= 0)
                {
                    stlElement = node?.InnerXml;
                }
                else
                {
                    stlElement = stlElement.Substring(index, length);
                }

                parsedContent = StlPageElementParser.ParseStlPageItemsInDynamicPage(stlElement, pageInfo, pageUrl, channelId, currentPageIndex, pageCount, totalNum, isPageRefresh, ajaxDivId);
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, stlElement, ex);
            }

            return parsedContent;
        }
    }
}
