﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easy.Extend;
using Easy.CMS.Web.Layout;

namespace Easy.CMS.Web.Zone
{
    public static class Helper
    {
        public static ZoneCollection GetZones(string[] html, out LayoutHtmlCollection result)
        {
            ZoneEntity zone = null;
            bool initZoneStart = false;
            result = new LayoutHtmlCollection();
            ZoneCollection zones = new ZoneCollection();
            foreach (string item in html)
            {
                if (item == ZoneEntity.ZoneTag)
                {
                    zone = new ZoneEntity();
                    initZoneStart = true;
                    continue;
                }
                else if (item == ZoneEntity.ZoneEndTag)
                {
                    initZoneStart = false;
                    if (zone.ZoneId.IsNullOrEmpty())
                    {
                        zone.ZoneId = Guid.NewGuid().ToString("N");
                    }
                    zones.Add(zone);
                    result.Add(new LayoutHtml { Html = ZoneEntity.ZoneTag });
                    result.Add(new LayoutHtml { Html = zone.ZoneId });
                    result.Add(new LayoutHtml { Html = ZoneEntity.ZoneEndTag });
                    continue;
                }

                if (!initZoneStart)
                {
                    result.Add(new LayoutHtml { Html = item });
                }
                else
                {
                    if (item.Contains("name=\"ZoneName\"") || item.Contains("name=\"ZoneId\""))
                    {
                        string[] zoneInfo = item.Split(new string[] { "<input" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string zonePart in zoneInfo)
                        {
                            if (zonePart.Contains("name=\"ZoneName\""))
                            {
                                zone.ZoneName = zonePart.GetInnerContent("value=\"", "\"", 0);
                            }
                            else if (zonePart.Contains("name=\"ZoneId\""))
                            {
                                zone.ZoneId = zonePart.GetInnerContent("value=\"", "\"", 0);
                            }
                            else if (zonePart.Contains("name=\"LayoutId\""))
                            {
                                zone.LayoutId = zonePart.GetInnerContent("value=\"", "\"", 0);
                            }

                        }

                    }

                }

            }
            return zones;
        }
    }
}
